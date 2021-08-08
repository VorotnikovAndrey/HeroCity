using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using Economies.Parsing.Mapping;
using Gameplay.Building;
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

                foreach (BuildingDataMapping line in csv.EnumerateRecords(new BuildingDataMapping()))
                {
                    _target.Data.Add(new BuildingData
                    {
                        Id = line.Id,
                        Stages = line.Stage,
                        State = (BuildingState)Enum.Parse(typeof(BuildingState), line.BuildingState),
                        Type = (BuildingType)Enum.Parse(typeof(BuildingType), line.BuildingType)
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