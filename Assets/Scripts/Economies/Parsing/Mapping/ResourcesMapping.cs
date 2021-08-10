using CsvHelper.Configuration.Attributes;

namespace Economies.Parsing.Mapping
{
    public class ResourcesMapping
    {
        [Name("Type"), Default("")] public string Type { get; set; }
        [Name("Value"), Default(0)] public int Value { get; set; }
    }
}