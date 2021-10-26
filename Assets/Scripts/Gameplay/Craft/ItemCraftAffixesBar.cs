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
            for (int i = 0; i < _affixes.Count; i++)
            {
                if (i >= affixes.Count)
                {
                    _affixes[i].SetIcon(null);
                    continue;
                }

                SpriteBankElement spriteData = ContentProvider.Graphic.SpriteBank.AffixesIcons.FirstOrDefault(x => x.Id == affixes[i]);
                if (spriteData != null)
                {
                    _affixes[i].SetIcon(spriteData.Sprite);
                }
                else
                {
                    Debug.LogError("Affixe is not found".AddColorTag(Color.red));
                }
            }
        }
    }
}