using System.Linq;
using System.Collections.Generic;

namespace Adoption.Configuration
{
    public class Parser
    {
        private readonly Processor _processor;
        private readonly string _source;

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

        public ParserResults Parse()
        {
            var results = new ParserResults();
            var scanner = new StringScanner(_source);
            var matched = true;
            while (matched && scanner.RemainingLength > 0)
            {
                var start = scanner.Position;
                scanner.Skip(@"^\s+");
                var keyPart = scanner.Scan(@"^((?<key>-\w)[\s]?|(?<key>--.*?)(\s|$))");
                var valuePart = scanner.Scan(@"^(?!-)(\""(?<value>.*?)\""|(?<value>.*?)(\s|$))");
                matched = keyPart != null || valuePart != null;
                var arg = _processor.ArgumentConfiguration(keyPart == null ? _processor.DefaultArgument : keyPart.Groups["key"].Value);

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

                if (arg.Flag) valuePart = null;
                if (!arg.TakesManyValues && results.ValuePairs.Any(v => v.Key == arg))
                {
                    results.UnresolvedParts.Add(_source.Substring(start, scanner.Position - start));
                }
                else
                {
                    results.ValuePairs.Add(new KeyValuePair<Argument, string>(arg, valuePart == null ? "True" : valuePart.Groups["value"].Value));
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