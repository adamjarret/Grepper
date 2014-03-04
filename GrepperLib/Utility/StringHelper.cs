using System.Collections.Generic;
using System.Linq;

namespace GrepperLib.Utility
{
    public static class StringHelper
    {
        /// <summary>
        /// Converts a list of white-space or comma-delimited string values into a list of string types.
        /// </summary>
        /// <param name="stringList"></param>
        public static IList<string> ConvertStringToList(string stringList)
        {
            if (stringList.Length <= 0)
            {
                return new List<string>();
            }

            char[] delimeters = { ' ', ',' };
            var extensions = stringList.Split(delimeters);

            return extensions.Select(word => word.Trim()).ToList();
        }
    }
}
