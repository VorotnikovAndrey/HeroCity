using CsvHelper.Configuration.Attributes;

namespace Economies.Parsing.Mapping
{
    public class ResourcesDataMapping
    {
        [Name("Type"), Default("")] public string Type { get; set; }
        [Name("Value"), Default(0)] public int Value { get; set; }
    }
}