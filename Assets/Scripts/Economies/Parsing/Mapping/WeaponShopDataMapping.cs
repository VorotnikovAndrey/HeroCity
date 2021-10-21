using CsvHelper.Configuration.Attributes;

namespace Economies.Parsing.Mapping
{
    public class WeaponShopDataMapping
    {
        [Name("Index"), Default(0)] public int Index { get; set; }
        [Name("Title"), Default("")] public string Title { get; set; }
        [Name("Description"), Default("")] public string Description { get; set; }
        [Name("Rarity"), Default("")] public string Rarity { get; set; }
        [Name("IconId"), Default("")] public string IconId { get; set; }
        [Name("EquipSlotType"), Default("")] public string EquipSlotType { get; set; }
        [Name("WeaponType"), Default("")] public string WeaponType { get; set; }
        [Name("Affixes"), Default("")] public string Affixes { get; set; }
    }
}