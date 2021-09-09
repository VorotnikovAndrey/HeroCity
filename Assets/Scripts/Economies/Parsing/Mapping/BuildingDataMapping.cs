using CsvHelper.Configuration.Attributes;

namespace Economies.Parsing.Mapping
{
    public class BuildingDataMapping
    {
        [Name("Id"), Default("")] public string Id { get; set; }
        [Name("Stage"), Default(0)] public int Stage { get; set; }
        [Name("Type"), Default("")] public string Type { get; set; }
        [Name("State"), Default("")] public string State { get; set; }
    }
}