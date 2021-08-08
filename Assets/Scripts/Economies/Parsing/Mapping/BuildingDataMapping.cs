using CsvHelper.Configuration.Attributes;

namespace Economies.Parsing.Mapping
{
    public class BuildingDataMapping
    {
        [Name("Id"), Default("")] public string Id { get; set; }
        [Name("Stage"), Default(0)] public int Stage { get; set; }
        [Name("BuildingType"), Default("")] public string BuildingType { get; set; }
        [Name("BuildingState"), Default("")] public string BuildingState { get; set; }
    }
}