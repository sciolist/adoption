namespace Adoption.Configuration
{
    public class ArgumentConfigurator
    {
        public Argument Argument { get; private set; }
        public Processor Processor { get; private set; }

        public ArgumentConfigurator(Argument arg, Processor processor)
        {
            Argument = arg;
            Processor = processor;
        }

        public bool Enabled
        {
            get { return Value != null; }
        }
        public object Value
        {
            get { return Processor[Argument]; }
        }

        public ArgumentConfigurator Flag(bool flag = true)
        {
            Argument.Flag = flag;
            return this;
        }

        public ArgumentConfigurator Required(bool required = true)
        {
            Argument.Required = required;
            return this;
        }

        public ArgumentConfigurator TakesManyValues(bool manyValues = true)
        {
            if(manyValues) Flag(false);
            Argument.TakesManyValues = manyValues;
            return this;
        }

        public ArgumentConfigurator Alias(string value)
        {
            Argument.Aliases.Add(value);
            return this;
        }

        public ArgumentConfigurator Describe(string description)
        {
            Argument.Description = description;
            return this;
        }
    }
}