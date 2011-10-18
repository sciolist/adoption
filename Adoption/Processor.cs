using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adoption.Configuration;

namespace Adoption
{
    public class Processor
    {
        private readonly IDictionary<Argument, object> _processed;
        public string Description { get; set; }
        public string DefaultArgument { get; private set; }
        public IList<Argument> Arguments { get; private set; }
        
        public Processor(string description = null, string defaultArgument = null)
        {
            Description = description;
            DefaultArgument = defaultArgument;
            _processed = new Dictionary<Argument, object>();
            Arguments = new List<Argument>();
            if (!string.IsNullOrEmpty(defaultArgument)) Handle(defaultArgument);
        }

        public Processor Default(string value)
        {
            DefaultArgument = value;
            return this;
        }

        public IList<string> UnresolvedParts { get; set; }

        public Processor Process(IEnumerable<string> args)
        {
            var parsed = new Parser(this, args).Parse();
            UnresolvedParts = parsed.UnresolvedParts;
            _processed.Clear();
            foreach(var pair in parsed.ValuePairs)
            {
                pair.Key.Assign(_processed, pair.Value);
            }
            foreach(var argument in Arguments)
            {
                argument.Verify(_processed);
            }
            return this;
        }

        public object this[Argument key]
        {
            get
            {
                object result;
                _processed.TryGetValue(key, out result);
                return result;
            }
        }

        public object this[string key]
        {
            get { return this[ArgumentConfiguration(key)]; }
        }

        public Argument ArgumentConfiguration(string key)
        {
            return Arguments.FirstOrDefault(a => a.Aliases.Contains(key));
        }

        public ArgumentConfigurator Handle(params string[] aliases)
        {
            var current = aliases.Select(ArgumentConfiguration).FirstOrDefault(v => v != null);
            if (current != null) return new ArgumentConfigurator(current, this);

            var arg = new Argument();
            var cfg = new ArgumentConfigurator(arg, this);
            Arguments.Add(arg);
            foreach (var value in aliases) cfg.Alias(value);
            return cfg;
        }

        public string Help()
        {
            var output = new StringBuilder();
            output.AppendFormat("{0}\n\n", Description);
            foreach(var arg in Arguments)
            {
                output.Append(arg.Help());
            }
            return output.ToString();
        }
    }
}