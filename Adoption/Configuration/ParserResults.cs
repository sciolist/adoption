using System.Collections.Generic;

namespace Adoption.Configuration
{
    public class ParserResults
    {
        public IList<KeyValuePair<Argument, string>> ValuePairs { get; private set; }
        public IList<string> UnresolvedParts { get; private set; }

        public ParserResults()
        {
            ValuePairs = new List<KeyValuePair<Argument, string>>();
            UnresolvedParts = new List<string>();
        }
    }
}