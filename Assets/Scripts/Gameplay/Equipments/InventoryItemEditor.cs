using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Gameplay.Equipments
{
    [CustomEditor(typeof(InventoryItem))]
    public class InventoryItemEditor : Editor
    {
        private InventoryItem _target;
        private SpriteBank _spriteBank;

        private void OnEnable()
        {
            _spriteBank = Resources.LoadAll<SpriteBank>("SO/SpriteBank/").ToList().FirstOrDefault();
        }

        public override void OnInspectorGUI()
        {
            _target = target as InventoryItem;

            ShowButtons();

            base.OnInspectorGUI();
        }

        private void ShowButtons()
        {
            EditorGUILayout.BeginHorizontal();

            foreach (var rarity in Enum.GetNames(typeof(Rarity)))
            {
                var data = _spriteBank.ItemsRarity.FirstOrDefault(x => x.Rarity.ToString() == rarity);
                if (data == null)
                {
                    continue;

                }

                if (GUILayout.Button($"Set {rarity}"))
                {
                    _target.SetRarityFromEditor(data.ColorBackground, data.Sprite);
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}
