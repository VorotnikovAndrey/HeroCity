using System.Collections.Generic;
using System.Linq;
using Defong.Utils;
using Economies;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Building.View.Editor
{
    [CustomEditor(typeof(BuildingView))]
    public class BuildingViewEditor : UnityEditor.Editor
    {
        private BuildingView Target;

        private List<string> _buildingIds;

        public override void OnInspectorGUI()
        {
            Target = (BuildingView)target;

            EditorGUILayout.BeginVertical("Box");
            TryCacheIds();

            SerializedProperty id = serializedObject.FindProperty("Id");

            var result = EditorGUILayout.TextField("Id:", id.stringValue);

            if (id.stringValue != result)
            {
                id.stringValue = result;
                serializedObject.ApplyModifiedProperties();
            }

            EditorGUILayout.EndVertical();

            EditorUtility.SetDirty(Target);

            base.OnInspectorGUI();
        }

        private void TryCacheIds()
        {
            if (_buildingIds != null)
            {
                return;
            }

            var economies = Resources.LoadAll<BuildingsEconomy>("");

            if (economies == null)
            {
                Debug.Log("BuildingsEconomy == null".AddColorTag(Color.red));
                return;
            }

            _buildingIds = new List<string>();

            foreach (BuildingsEconomy economy in economies)
            {
                _buildingIds.AddRange(economy.Data.Select(x => x.Id));

                foreach (var id in _buildingIds)
                {
                    Debug.Log(id.AddColorTag(Color.cyan));
                }
            }
        }
    }
}