using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Defong.Utils;
using Economies.Parsing.Mapping;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Economies.Editor
{
    [CustomEditor(typeof(LocationsEconomy))]
    public class LocationsEconomyEditor : BaseEconomyEditor
    {
        private LocationsEconomy _target;

        public void OnEnable()
        {
            _target = (LocationsEconomy) target;
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
                if (FillLocationsData(file))
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
        private bool FillLocationsData(string path)
        {
            try
            {
                using StreamReader reader = new StreamReader(path);
                Configuration info = new Configuration { CultureInfo = CultureInfo.CreateSpecificCulture("en-US") };
                using CsvReader csv = new CsvReader(reader, info);
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.Delimiter = ",";

                string lastId = string.Empty;

                foreach (LocationsDataMapping line in csv.EnumerateRecords(new LocationsDataMapping()))
                {
                    if (!string.IsNullOrEmpty(line.Id))
                    {
                        lastId = line.Id;
                    }

                    LocationData data = _target.Data.FirstOrDefault(x => x.Id == lastId);

                    if (data == null)
                    {
                        data = new LocationData {Id = lastId};
                        _target.Data.Add(data);
                    }

                    if (!data.BuildingsIds.Contains(line.BuildingsIds))
                    {
                        data.BuildingsIds.Add(line.BuildingsIds);
                    }
                    else
                    {
                        Debug.LogError($"LocationData has duplicate! {line.BuildingsIds.AddColorTag(Color.yellow)}".AddColorTag(Color.red));
                    }
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