using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GrepperLib.Model;
using GrepperLib.Utility;

namespace GrepperLib.Controller
{
    public class FileController
    {
        #region Private Members________

        private string _baseSearchPath;
        volatile private IList<FileData> _fileDataList;
        public BackgroundWorker Worker { get; set; }

        #endregion Private Members
        #region Public Properties______

        public string BaseSearchPath
        {
            get
            {
                return _baseSearchPath;
            }
            set
            {
                // match a drive letter pattern only
                var reg = new Regex("^[a-zA-Z][:]{1}");
                if (reg.IsMatch(value))
                    _baseSearchPath = value;
                else
                    _baseSearchPath = string.Empty;
            }
        }

        public IList<FileData> FileDataList
        {
            get
            {
                return _fileDataList;
            }
        }

        public string FileExtensions { get; set; }

        public bool RecursiveSearch { get; set; }

        public bool MatchCase { get; set; }

        public bool MatchPhrase { get; set; }

        public bool LiteralSearch { get; set; }

        public IList<string> MessageList { get; private set; }

        public string SearchCriteria { get; set; }

        public int TotalMatches
        {
            get
            {
                return FileDataList == null
                    ? 0
                    : FileDataList.Sum(fd => fd.LineDataList.Count);
            }
        }

        #endregion
        #region Constructor____________

        public FileController()
        {
            _baseSearchPath = string.Empty;
            FileExtensions = string.Empty;
            RecursiveSearch = true;
            MatchCase = false;
            MatchPhrase = false;
            MessageList = new List<string>();
            _fileDataList = new List<FileData>();
        }

        #endregion Constructor
        #region Public Methods_________

        public void SetFormData(SearchOptions so)
        {
            SearchCriteria = (so.Search ?? "").Trim();
            MatchCase = so.MatchCase;
            MatchPhrase = so.MatchPhrase;
            FileExtensions = so.Extensions ?? "";
            RecursiveSearch = so.Recursive;
            BaseSearchPath = so.Path ?? "";
            LiteralSearch = so.Literal;
        }

        /// <summary>
        /// Creates a list of FileData objects and
        /// </summary>
        public void GenerateFileData()
        {
            MessageList = new List<string>();

            // if no criteria or no file extensions or no base path, there is no way data can be generated
            if (string.IsNullOrEmpty(SearchCriteria)) MessageList.Add("No search criteria provided.");
            if (string.IsNullOrEmpty(FileExtensions)) MessageList.Add("No file filters provided.");
            if (string.IsNullOrEmpty(BaseSearchPath)) MessageList.Add("No search path provided.");
            if (MessageList.Count > 0) return;

            _fileDataList = new List<FileData>();

            var wildcards = StringHelper.ConvertStringToList(FileExtensions);
            var searchPatterns = wildcards.Where(e =>
                !String.IsNullOrWhiteSpace(e) && !e.Trim().StartsWith("-")
                ).ToList();
            if (searchPatterns.Count < 1)
            {
                searchPatterns.Add("*.*"); // If only exclude patterns are specified, assume *.* as base
            }
            var excludedPatterns = wildcards.Where(e => e.Trim().StartsWith("-"))
                .Select(e => WildcardHelper.ConvertToRegex(e.Substring(1)))
                .ToList();

            if (!MatchCase) SearchCriteria = SearchCriteria.ToLower();
            foreach (var searchPattern in searchPatterns)
            {
                try
                {
                    SearchFiles(searchPattern, excludedPatterns);
                }
                catch (PathTooLongException ptle)
                {
                    MessageList.Add(ptle.Message);
                }
                catch (IOException ioe)
                {
                    MessageList.Add(ioe.Message);
                }
                catch (UnauthorizedAccessException uax)
                {
                    MessageList.Add(uax.Message);
                }
            }
        }

        #endregion Public Methods
        #region Private Methods________

        private void SearchFiles(string searchPattern, IEnumerable<Regex> excludedPatterns)
        {
            SearchOption so = RecursiveSearch ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var files = (from file in Directory.EnumerateFiles(BaseSearchPath, searchPattern, so)
                         where excludedPatterns.All(e => !e.IsMatch(file))
                         select file).ToList();

            double i = 0;
            double total = files.Count;
            int percent = 0;
            foreach (var f in files)
            {
                i++;
                if (Worker != null)
                {
                    percent = (int)Math.Round((i / total) * 100.0);
                    Worker.ReportProgress(percent);

                    // Allow cancellation
                    if (Worker.CancellationPending)
                    {
                        return;
                    }
                }

                if (!f.Contains('\\')) continue;
                var fileData = new FileData
                {
                    FileExtension = searchPattern,
                    FileName = f.Substring(f.LastIndexOf('\\') + 1),
                    FilePath = f.Trim()
                };

                using (var sr = new StreamReader(f))
                {
                    string line;
                    uint lineNumber = 1;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // Allow cancellation mid-file
                        if (Worker != null && Worker.CancellationPending)
                        {
                            return;
                        }

                        SearchLine(lineNumber, line, ref fileData);
                        lineNumber++;
                    }
                }
                if (fileData.LineDataList != null && fileData.LineDataList.Count > 0)
                {
                    _fileDataList.Add(fileData);
                    if (Worker != null)
                    {
                        Worker.ReportProgress(percent, fileData);
                    }
                }
            }
        }

        public void SearchLine(long lineNumber, string line, ref FileData fileData)
        {
            var regOptions = (MatchCase) ? RegexOptions.None : RegexOptions.IgnoreCase;
            if (LiteralSearch)
            {
                if (MatchPhrase)
                {
                    // criteria to find search pattern that ignores certain boundaries
                    var phrase = String.Format(@"(\b)({0}+(\b|\n|\s))", SearchCriteria);
                    var reg = new Regex(phrase, regOptions);
                    if (reg.IsMatch(line))
                    {
                        var lineData = new LineData(lineNumber, line);
                        foreach (Match match in reg.Matches(line))
                        {
                            lineData.Matches.Add(new MatchRange(match.Index, match.Length));
                        }
                        fileData.SetLineData(lineNumber, lineData);
                    }
                }
                else
                {
                    int len = SearchCriteria.Length;
                    string lowerline = line;
                    string lowersc = SearchCriteria;
                    if (!MatchCase)
                    {
                        lowerline = lowerline.ToLower();
                        lowersc = lowersc.ToLower();
                    }

                    if (lowerline.Contains(lowersc))
                    {
                        var lineData = new LineData(lineNumber, line);
                        int pos, startIdx = 0;
                        do
                        {
                            pos = lowerline.IndexOf(lowersc, startIdx, StringComparison.Ordinal);
                            if (pos >= 0)
                            {
                                lineData.Matches.Add(new MatchRange(pos, len));
                                startIdx = pos + len;
                            }
                        }
                        while(pos >= 0);
                        fileData.SetLineData(lineNumber, lineData);
                    }
                }
            }
            else
            {
                // pattern treated as REGEX
                var reg = new Regex(SearchCriteria, regOptions);
                if (reg.IsMatch(line))
                {
                    var lineData = new LineData(lineNumber, line);
                    foreach (Match match in reg.Matches(line))
                    {
                        lineData.Matches.Add(new MatchRange(match.Index, match.Length));
                    }
                    fileData.SetLineData(lineNumber, lineData);
                }
            }
            
        }

        #endregion Private Methods
    }
}