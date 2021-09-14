using CsvHelper.Configuration.Attributes;

namespace Economies.Parsing.Mapping
{
    public class BuildingDataMapping
    {
        [Name("Id"), Default("")] public string Id { get; set; }
        [Name("Stage"), Default("")] public string Stage { get; set; }
        [Name("Type"), Default("")] public string Type { get; set; }
        [Name("State"), Default("")] public string State { get; set; }
        [Name("UpgradeResourceType"), Default("")] public string UpgradeResourceType { get; set; }
        [Name("UpgradeResourceValue"), Default("")] public string UpgradeResourceValue { get; set; }
        [Name("UpgradeDuration"), Default("")] public string UpgradeDuration { get; set; }
        [Name("ImprovementDependencies"), Default("")] public string ImprovementDependencies { get; set; }
        [Name("ImprovementOpen"), Default("")] public string ImprovementOpen { get; set; }
    }
}