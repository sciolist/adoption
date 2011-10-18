using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Adoption.Configuration
{
    public class Parser
    {
        private readonly Processor _processor;
        private readonly string _source;
        private string _key;
        private string _value;

        public Parser(Processor processor, IEnumerable<string> values)
        {
            _processor = processor;
            _source = string.Join(" ", values.Select(Quote));
        }

        private static string Quote(string value)
        {
            if (!value.Contains(' ')) return value;
            return "\"" + value + "\"";
        }

        private void OnKey(Match match)
        {
            _key = match.Groups["key"].ToString();
        }

        private void OnValue(Match match)
        {
            _value = match.Groups["value"].ToString();
        }

        public ParserResults Parse()
        {
            var results = new ParserResults();
            var scanner = new StringScanner(_source);
            var matched = true;
            while (matched && scanner.RemainingLength > 0)
            {
                _key = null;
                var start = scanner.Position;
                scanner.Skip(@"^\s+");
                var keyPart = scanner.Scan(@"^((?<key>-\w)[\s]?|(?<key>--.*?)(\s|$))", OnKey);
                var valuePart = scanner.Scan(@"^(\""(?<value>.*?)\""|(?<value>.*?)(\s|$))", OnValue);
                matched = keyPart != null || valuePart != null;
                var arg = _processor.ArgumentConfiguration(_key ?? _processor.DefaultArgument);

                if(arg != null && arg.Flag && valuePart != null)
                {
                    if(keyPart != null) scanner.Position -= valuePart.Length;
                    valuePart = null;
                }

                if(arg == null)
                {
                    results.UnresolvedParts.Add(_source.Substring(start, scanner.Position - start));
                    continue;
                }

                if (arg.Flag) _value = null;
                if (!arg.TakesManyValues && results.ValuePairs.Any(v => v.Key == arg))
                {
                    results.UnresolvedParts.Add(_source.Substring(start, scanner.Position - start));
                }
                else
                {
                    results.ValuePairs.Add(new KeyValuePair<Argument, string>(arg, _value ?? "True"));
                }
            }
            if (scanner.RemainingLength > 0)
            {
                throw new ParseException(string.Format("Invalid arguments value: {0}", _source));
            }
            return results;
        }
    }
}