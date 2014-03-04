namespace GrepperLib.Model
{
    public class SearchOptions
    {
        public bool MatchCase { get; set; }
        public bool MatchPhrase { get; set; }
        public bool Recursive { get; set; }
        public bool Literal { get; set; }
        public string Search { get; set; }
        public string Path { get; set; }
        public string Extensions { get; set; }

        public SearchOptions()
        {
            Extensions = "*.*";
        }
    }
}
