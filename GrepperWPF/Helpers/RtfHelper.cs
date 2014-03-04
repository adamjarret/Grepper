using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using GrepperLib.Model;

namespace GrepperWPF.Helpers
{
    public static class RtfHelper
    {
        /// <summary>
        /// Render the matched line as RTF, highlighting the matching segments of the string
        /// </summary>
        /// <param name="lineData">a LineData object containing Matches</param>
        /// <returns>an RTF version of lineData.Text with the macthing segments highlighted</returns>
        public static string MarkUpText(LineData lineData)
        {
            var text = lineData.Text;
            try
            {
                // Create a Paragraph node to hold the line (love isn't always on time)
                var p = new Paragraph
                {
                    FontFamily = new FontFamily("Consolas"),
                    FontSize = 12.0
                };

                // Iterate over the list of matches to break the line into segments
                int i = 0, offset = 0;
                string segment;
                foreach (var match in lineData.Matches)
                {
                    // Get the length of the next segment relative to the offset
                    int endPos = match.Index - offset;

                    // Add the part of the string that comes before this match
                    //  Trim leading whitespace if this is the first segment
                    segment = text.Substring(offset, endPos);
                    AddSegment(i == 0 ? segment.TrimStart() : segment, new Span(), ref p);

                    // Add this match (with different formatting)
                    segment = text.Substring(match.Index, match.Length);
                    AddSegment(segment, new Bold(), ref p);

                    // Only process the portion of the string after this match on the next pass
                    offset += endPos + match.Length;
                    i++;
                }
                // Add the part of the string that comes after the last match
                segment = text.Substring(offset, text.Length - offset);
                AddSegment(segment, new Span(), ref p);

                // Build document
                var doc = new FlowDocument();
                doc.Blocks.Add(p);
                return RenderRtf(doc);
            }
            catch (Exception e)
            {
                return RenderRtf(ErrorDocument(e));
            }
        }

        /// <summary>
        /// Render a FlowDocument as an RTF string
        /// </summary>
        /// <param name="doc">a FlowDocument to be rendered</param>
        /// <returns>an RTF document as a string</returns>
        private static string RenderRtf(FlowDocument doc)
        {
            // Get TextRange from document
            var tr = new TextRange(doc.ContentStart, doc.ContentEnd);

            // Render RTF to MemoryStream
            var ms = new MemoryStream();
            tr.Save(ms, DataFormats.Rtf);

            // Encode MemoryStream containing RTF as string
            return Encoding.Default.GetString(ms.ToArray());
        }

        private static void AddSegment(string segment, Span s, ref Paragraph p)
        {
            s.Inlines.Add(new Run(segment));
            p.Inlines.Add(s);
        }

        private static FlowDocument ErrorDocument(Exception e)
        {
            var p = new Paragraph(new Run(e.Message))
            {
                Foreground = Brushes.Crimson,
                FontFamily = new FontFamily("Segoe UI")
            };
            return new FlowDocument(p);
        }
    }
}
