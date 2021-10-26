using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Economies.Parsing.Mapping;
using Gameplay.Craft;
using Gameplay.Equipments;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Economies.Editor
{
    [CustomEditor(typeof(WeaponShopEconomy))]
    public class WeaponShopEconomyEditor : BaseEconomyEditor
    {
        private WeaponShopEconomy _target;

        public void OnEnable()
        {
            _target = (WeaponShopEconomy) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("Data"), true);

            serializedObject.ApplyModifiedProperties();
        }

        protected override void ParseFolder(params string[] files)
        {
            _target.Data.Clear();

            foreach (string file in files)
            {
                // duplicate this
                if (FillWeaponShopData(file))
                {
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.UpdateIfRequiredOrScript();
                    continue;
                }

                serializedObject.UpdateIfRequiredOrScript();
                EditorUtility.DisplayDialog("parsing error", file, "OK");
                Debug.LogException(new Exception($"parsing error {file}"));
            }

            EditorWindow.focusedWindow.ShowNotification(new GUIContent("Data was imported!"));

            EditorUtility.SetDirty(_target);
        }
        private bool FillWeaponShopData(string path)
        {
            try
            {
                using StreamReader reader = new StreamReader(path);
                Configuration info = new Configuration { CultureInfo = CultureInfo.CreateSpecificCulture("en-US") };
                using CsvReader csv = new CsvReader(reader, info);
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.Delimiter = ",";

                WeaponShopData data = null;

                foreach (WeaponShopDataMapping line in csv.EnumerateRecords(new WeaponShopDataMapping()))
                {
                    if (!string.IsNullOrEmpty(line.Id))
                    {
                        data = new WeaponShopData
                        {
                            Index = line.Index,
                            Item = new WeaponItem
                            {
                                Id = line.Id,
                                Title = line.Title,
                                Description = line.Description,
                                Rarity = (Rarity)Enum.Parse(typeof(Rarity), line.Rarity),
                                IconId = line.IconId,
                                EquipSlotType = (EquipSlotType)Enum.Parse(typeof(EquipSlotType), line.EquipSlotType),
                                WeaponType = (WeaponType)Enum.Parse(typeof(WeaponType), line.WeaponType),
                                AffixesIds = line.Affixes.Split(',').Select(x => x.Replace(" ", string.Empty)).Where(x => !string.IsNullOrEmpty(x)).ToList(),
                                Equipped = false
                            }
                        };

                        _target.Data.Add(data);
                    }

                    data?.Item.Price.Add(EconomyUtils.GetPrice(line.PriceResourceType, line.PriceResourceValue));
                }

                return true;
            }
            catch (Exception)
            {
                // ignored
            }

            return false;
        }
    }
}