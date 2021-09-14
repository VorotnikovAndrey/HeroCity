using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using Economies.Parsing.Mapping;
using Gameplay;
using ResourceSystem;
using UnityEditor;
using UnityEngine;

namespace Economies.Editor
{
    [CustomEditor(typeof(ResourcesEconomy))]
    public class ResourcesEconomyEditor : BaseEconomyEditor
    {
        private ResourcesEconomy _target;

        public void OnEnable()
        {
            _target = (ResourcesEconomy) target;
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
                if (FillResourcesData(file))
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
        private bool FillResourcesData(string path)
        {
            try
            {
                using StreamReader reader = new StreamReader(path);
                Configuration info = new Configuration { CultureInfo = CultureInfo.CreateSpecificCulture("en-US") };
                using CsvReader csv = new CsvReader(reader, info);
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.Delimiter = ",";

                foreach (ResourcesDataMapping line in csv.EnumerateRecords(new ResourcesDataMapping()))
                {
                    _target.Data.Add(new ResourcesData
                    {
                        Type = (ResourceType)Enum.Parse(typeof(ResourceType), line.Type),
                        Value = line.Value
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