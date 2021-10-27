using CsvHelper.Configuration.Attributes;

namespace Economies.Parsing.Mapping
{
    public class WeaponShopDataMapping
    {
        [Name("Id"), Default("")] public string Id { get; set; }
        [Name("Index"), Default(0)] public int Index { get; set; }
        [Name("Title"), Default("")] public string Title { get; set; }
        [Name("Description"), Default("")] public string Description { get; set; }
        [Name("Rarity"), Default("")] public string Rarity { get; set; }
        [Name("IconId"), Default("")] public string IconId { get; set; }
        [Name("EquipSlotType"), Default("")] public string EquipSlotType { get; set; }
        [Name("WeaponType"), Default("")] public string WeaponType { get; set; }
        [Name("Affixes"), Default("")] public string Affixes { get; set; }
        [Name("PriceResourceType"), Default("")] public string PriceResourceType { get; set; }
        [Name("PriceResourceValue"), Default(0)] public int PriceResourceValue { get; set; }
        [Name("TimeCreationUnix"), Default(0)] public long TimeCreationUnix { get; set; }
        [Name("StatsType"), Default("")] public string StatsType { get; set; }
        [Name("StatsValue"), Default("")] public string StatsValue { get; set; }
        [Name("MinDamage"), Default("")] public string MinDamage { get; set; }
        [Name("MaxDamage"), Default("")] public string MaxDamage { get; set; }
    }
}