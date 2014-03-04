using GrepperWPF.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GrepperLib.Model;
using GrepperLib.Controller;

namespace GrepperWPF.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var so = new SearchOptions
            {
                search = "foo",
                literal = false,
                matchCase = false,
                matchPhrase = false
            };

            var fileData = new FileData();
            var fc = new FileController();
            fc.SetFormData(so);
            fc.SearchLine(0, "\t\tKung fu is not spelled Kungfoo. That would be FOOLISH", ref fileData);

            LineData lineData;
            fileData.LineDataList.TryGetValue(0, out lineData);

            // ReSharper disable once PossibleNullReferenceException
            Assert.AreEqual(2, lineData.Matches.Count);

            const string expected = "{\\rtf1\\ansi\\ansicpg1252\\uc1\\htmautsp\\deff2{\\fonttbl{\\f0\\fcharset0 Times New Roman;}{\\f2\\fcharset0 Georgia;}{\\f3\\fcharset0 Consolas;}}{\\colortbl\\red0\\green0\\blue0;\\red255\\green255\\blue255;}\\loch\\hich\\dbch\\pard\\plain\\ltrpar\\itap0{\\lang1033\\fs18\\f3\\cf0 \\cf0\\qj{\\f3 {\\ltrch Kung fu is not spelled Kung}{\\b\\ltrch foo}{\\ltrch . That would be }{\\b\\ltrch FOO}{\\ltrch LISH}\\li0\\ri0\\sa0\\sb0\\fi0\\qj\\par}\r\n}\r\n}";
            Assert.AreEqual(expected, RtfHelper.MarkUpText(lineData));
        }
    }
}
