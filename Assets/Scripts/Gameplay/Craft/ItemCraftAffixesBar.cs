using System.Collections.Generic;
using System.Linq;
using Content;
using UnityEngine;
using Utils;

namespace Gameplay.Craft
{
    public class ItemCraftAffixesBar : MonoBehaviour
    {
        [SerializeField] private List<ItemCraftAffixSlot> _affixes;

        public void SetAffixes(List<string> affixes)
        {
            int count = 0;

            foreach (var affix in affixes)
            {
                SpriteBankElement spriteData = ContentProvider.Graphic.SpriteBank.AffixesIcons.FirstOrDefault(x => x.Id == affix);
                if (spriteData != null)
                {
                    _affixes[count].SetIcon(spriteData.Sprite);
                }
                else
                {
                    Debug.LogError("Affixe is not found".AddColorTag(Color.red));
                }

                count++;
            }
        }
    }
}