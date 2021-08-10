using CsvHelper.Configuration.Attributes;

namespace Economies.Parsing.Mapping
{
    public class LocationsDataMapping
    {
        [Name("Id"), Default("")] public string Id { get; set; }
        [Name("BuildingsIds"), Default("")] public string BuildingsIds { get; set; }
    }
}