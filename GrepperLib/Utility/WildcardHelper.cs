using System;
using System.Text.RegularExpressions;

namespace GrepperLib.Utility
{
    // Thanks http://stackoverflow.com/a/4375058
    public static class WildcardHelper
    {
        private static readonly Regex HasQuestionMarkRegEx = new Regex(@"\?", RegexOptions.Compiled);
        private static readonly Regex IlegalCharactersRegex = new Regex("[" + @"\/:<>|" + "\"]", RegexOptions.Compiled);
        private static readonly Regex CatchExtentionRegex = new Regex(@"^\s*.+\.([^\.]+)\s*$", RegexOptions.Compiled);
        private const string NonDotCharacters = @"[^.]*";

        /// <summary>
        /// Converts a wildcard to a regex.
        /// </summary>
        /// <param name="pattern">The wildcard pattern to convert.</param>
        /// <returns>A regex equivalent of the given wildcard.</returns>
        public static Regex ConvertToRegex(string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException();
            }

            pattern = pattern.Trim();
            
            if (pattern.Length == 0)
            {
                throw new ArgumentException("Pattern is empty.");
            }
            
            if (IlegalCharactersRegex.IsMatch(pattern))
            {
                throw new ArgumentException("Patterns contains ilegal characters.");
            }
            
            bool hasExtension = CatchExtentionRegex.IsMatch(pattern);

            bool matchExact = false;
            if (HasQuestionMarkRegEx.IsMatch(pattern))
            {
                matchExact = true;
            }
            else if (hasExtension)
            {
                matchExact = CatchExtentionRegex.Match(pattern).Groups[1].Length != 3;
            }
            
            string regexString = Regex.Escape(pattern);
            regexString = "^" + Regex.Replace(regexString, @"\\\*", ".*");
            regexString = Regex.Replace(regexString, @"\\\?", ".");
            if (!matchExact && hasExtension)
            {
                regexString += NonDotCharacters;
            }
            regexString += "$";
            
            return new Regex(regexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
    }
}
