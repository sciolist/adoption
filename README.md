Adoption
========

Adoption is a simple option parser for .Net command line applications, intended to parse out strings or flags.

Code
----------

    public static void Main()
    {
        var processor = new Adoption.Processor(defaultArgument: "--value");
        var key = processor.Handle("--key", "-k").Required().Describe("Which key to set.");
        var value = processor.Handle("--value", "-v").Describe("The value to set for the key.");
        var remove = processor.Handle("--remove", "-r").TakesValue(false).Describe("Remove the old key");
        try
        {
            processor.Process(new[]{ "--key", "id", "100" });
        }
        catch(AdoptionException ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(processor.Help());
            return;
        }

        var values = new Dictionary<string, string>();
        if(remove.Enabled) values.Remove(key.Value.ToString());
        if(value.Enabled) {
            values[key.Value.ToString()] = value.Value.ToString();
        }
    }

