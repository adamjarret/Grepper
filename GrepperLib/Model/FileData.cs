using System.Collections.Generic;

namespace GrepperLib.Model
{
    public class MatchRange
    {
        public readonly int Index;
        public readonly int Length;

        public MatchRange(int index, int length)
        {
            Index = index;
            Length = length;
        }
    }

    public class LineData
    {
        public string Text { get; private set; }
        public long LineNumber { get; private set; }
        public List<MatchRange> Matches { get; private set; }

        private LineData()
        {
            Matches = new List<MatchRange>();
        }

        public LineData(long lineNumber, string text) : this()
        {
            LineNumber = lineNumber;
            Text = text;
        }
    }

    /// <summary>
    /// Holds a list of one or more LineData objects
    /// representing a single file.
    /// </summary>
    public class FileData
    {
        #region Public Properties

        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileExtension { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        /// <summary>
        /// Read-only property for the LineDataList. Data
        /// must be set through the separate method with the line
        /// number and data both provided.
        /// </summary>
        public IDictionary<long, LineData> LineDataList { get; private set; }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Adds a line of data for this file object consisting of both the
        /// line number and the representative data.
        /// </summary>
        /// <param name="lineNumber">long</param>
        /// <param name="lineData">string</param>
        public void SetLineData(long lineNumber, LineData lineData)
        {
            if (LineDataList == null)
            {
                LineDataList = new Dictionary<long, LineData>();
            }
            LineDataList.Add(lineNumber, lineData);
        }
        
        #endregion
    }
}