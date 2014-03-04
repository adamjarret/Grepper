using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrepperLib.Model
{
    public class SearchOptions
    {
        public bool matchCase { get; set; }
        public bool matchPhrase { get; set; }
        public bool recursive { get; set; }
        public bool literal { get; set; }
        public string search { get; set; }
        public string path { get; set; }
        public string extensions { get; set; }
    }
}
