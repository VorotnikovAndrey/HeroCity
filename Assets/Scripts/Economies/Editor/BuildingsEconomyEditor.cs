using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Economies.Parsing.Mapping;
using Gameplay;
using Gameplay.Building;
using ResourceSystem;
using UnityEditor;
using UnityEngine;

namespace Economies.Editor
{
    [CustomEditor(typeof(BuildingsEconomy))]
    public class BuildingsEconomyEditor : BaseEconomyEditor
    {
        private BuildingsEconomy _target;

        public void OnEnable()
        {
            _target = (BuildingsEconomy) target;
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
                if (FillBuildingsData(file))
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
        private bool FillBuildingsData(string path)
        {
            try
            {
                using StreamReader reader = new StreamReader(path);
                Configuration info = new Configuration { CultureInfo = CultureInfo.CreateSpecificCulture("en-US") };
                using CsvReader csv = new CsvReader(reader, info);
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.Delimiter = ",";

                string lastId = string.Empty;
                int lastStage = -1;

                foreach (BuildingDataMapping line in csv.EnumerateRecords(new BuildingDataMapping()))
                {
                    if (!string.IsNullOrEmpty(line.Id))
                    {
                        lastId = line.Id;
                    }

                    if (!string.IsNullOrEmpty(line.Stage))
                    {
                        lastStage = Convert.ToInt32(line.Stage);
                    }

                    var element = _target.Data.FirstOrDefault(x => x.Id == lastId);
                    if (element == null)
                    {
                        element = new BuildingData
                        {
                            Id = line.Id,
                            State = (BuildingState)Enum.Parse(typeof(BuildingState), line.State),
                            Type = (BuildingType)Enum.Parse(typeof(BuildingType), line.Type)
                        };

                        _target.Data.Add(element);
                    }

                    var upgrade = element.Upgrades.FirstOrDefault(x => x.Stage == lastStage);
                    if (upgrade == null)
                    {
                        upgrade = new BuildingUpgradeData
                        {
                            Stage = lastStage,
                            Duration = Convert.ToInt32(line.UpgradeDuration)
                        };

                        element.Upgrades.Add(upgrade);
                    }

                    upgrade.Price.Add(new ResourcesData
                    {
                        Type = (ResourceType)Enum.Parse(typeof(ResourceType), line.UpgradeResourceType),
                        Value = Convert.ToInt32(line.UpgradeResourceValue)
                    });
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