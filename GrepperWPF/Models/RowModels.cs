using System.Globalization;
using GrepperLib.Model;
using GrepperWPF.Helpers;

namespace GrepperWPF.Models
{
    class FileMatchRow
    {
        // ReSharper disable MemberCanBePrivate.Global, UnusedAutoPropertyAccessor.Global
        public string Count { get; set; }
        public string Path { get; set; }
        // ReSharper restore MemberCanBePrivate.Global, UnusedAutoPropertyAccessor.Global

        public FileMatchRow(string count, string path)
        {
            Count = count;
            Path = path;
        }
    }

    class LineMatchRow
    {
        private readonly LineData _lineData;
        private string _markedUpText;

        // ReSharper disable MemberCanBePrivate.Global, UnusedAutoPropertyAccessor.Global
        public string Line { get; private set; }
        public string Text { get; private set; }
        // ReSharper restore MemberCanBePrivate.Global, UnusedAutoPropertyAccessor.Global

        // ReSharper disable once UnusedMember.Global (used as binding in MainWindow)
        public string MarkedUpText
        {
            get { return _markedUpText ?? (_markedUpText = RtfHelper.MarkUpText(_lineData)); }
        }

        public LineMatchRow(LineData lineData)
        {
            _markedUpText = null;
            _lineData = lineData;
            Line = lineData.LineNumber.ToString(CultureInfo.InvariantCulture);
            Text = lineData.Text;
        }

    }

}
