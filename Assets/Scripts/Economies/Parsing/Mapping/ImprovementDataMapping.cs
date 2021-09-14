using CsvHelper.Configuration.Attributes;

namespace Economies.Parsing.Mapping
{
    public class ImprovementDataMapping
    {
        [Name("Id"), Default("")] public string Id { get; set; }
        [Name("Description"), Default("")] public string Description { get; set; }
        [Name("SpriteBankId"), Default("")] public string SpriteBankId { get; set; }
    }
}