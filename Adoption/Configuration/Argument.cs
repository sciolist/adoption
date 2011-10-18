using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adoption.Configuration
{
    public class Argument
    {
        public bool Required { get; set; }
        public string Description { get; set; }
        public IList<string> Aliases { get; private set; }
        public bool Flag { get; set; }
        public bool TakesManyValues { get; set; }
        
        public Argument()
        {
            Aliases = new List<string>();
        }

        public string Help()
        {
            if (string.IsNullOrEmpty(Description)) return string.Empty;
            var output = new StringBuilder();
            output.AppendFormat(" {0}\n", string.Join(", ", Aliases));
            output.AppendFormat("  {0}\n\n", Description);
            return output.ToString();
        }

        public void Verify(IDictionary<Argument, object> processed)
        {
            if (Required && !processed.ContainsKey(this))
            {
                throw new ValueRequiredException(Aliases.First());
            }
        }

        public void Assign(IDictionary<Argument, object> processed, string value)
        {
            object current;
            processed.TryGetValue(this, out current);

            if(TakesManyValues)
            {
                var foundValue = current as IList<string>;
                if(foundValue == null)
                {
                    foundValue = new List<string>();
                    if (current != null) foundValue.Add((string)current);
                    processed[this] = foundValue;
                }
                foundValue.Add(value);
                return;
            }
            processed[this] = value;
        }
    }
}