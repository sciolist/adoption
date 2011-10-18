using System;
using System.Text.RegularExpressions;

namespace Adoption.Configuration
{
    public class StringScanner
    {
        private readonly string _source;

        public StringScanner(string source)
        {
            _source = source;
        }

        public int Position { get; set; }

        public string Remaining { get { return _source.Substring(Position); } }
        public int RemainingLength { get { return _source.Length - Position; } }

        public Match Scan(string regex, Action<Match> handle = null)
        {
            return Scan(new Regex(regex), handle);
        }

        public bool Skip(string regex)
        {
            return Skip(new Regex(regex));
        }

        public Match Scan(Regex regex, Action<Match> handle = null)
        {
            var match = regex.Match(Remaining);
            if (!match.Success) return null;
            Position += match.Index + match.Length;
            if (handle != null) handle(match);
            return match;
        }

        public bool Skip(Regex regex)
        {
            var match = regex.Match(Remaining);
            if (!match.Success) return false;
            Position += match.Index + match.Length;
            return true;
        }
    }
}