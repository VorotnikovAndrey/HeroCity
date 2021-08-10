using System.Collections.Generic;
using System.Linq;
using Economies;
using Gameplay.Locations.View;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Building.View.Editor
{
    [CustomEditor(typeof(BuildingView))]
    public class BuildingViewEditor : UnityEditor.Editor
    {
        private BuildingView _target;
        private LocationView _locationView;
        private List<LocationsEconomy> _locationsEconomy;
        private Vector2 _scrollViewPosition = Vector2.zero;
        private string _lastId = string.Empty;
        private readonly List<string> _keys = new List<string>();

        private void OnEnable()
        {
            _target = target as BuildingView;
            if (_target == null)
            {
                return;
            }

            _locationView = _target.transform.root.GetComponent<LocationView>();
            _locationsEconomy = Resources.LoadAll<LocationsEconomy>("").ToList();
        }

        public override void OnInspectorGUI()
        {
            if (Check())
            {
                return;
            }

            DrawId();
            DrawScrollView();

            EditorGUILayout.Space();

            base.OnInspectorGUI();
        }

        private bool Check()
        {
            GUIStyle errorStyle = new GUIStyle
            {
                normal = new GUIStyleState
                {
                    textColor = Color.red
                },
                fontStyle = FontStyle.Bold,
                fontSize = 15,
                alignment = TextAnchor.MiddleCenter
            };

            if (_locationView == null)
            {
                EditorGUILayout.LabelField("LocationView is not found!", errorStyle);
                return true;
            }

            if (string.IsNullOrEmpty(_locationView.LocationId))
            {
                EditorGUILayout.LabelField("LocationId is null or empty!", errorStyle);
                return true;
            }

            if (_locationsEconomy == null)
            {
                EditorGUILayout.LabelField("LocationsEconomy is null!", errorStyle);
                return true;
            }

            return false;
        }

        private void DrawId()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Id:", GUILayout.Width(20));
            _target.BuildingId = EditorGUILayout.TextArea(_target.BuildingId);
            if (GUILayout.Button("Clear", GUILayout.Width(75)))
            {
                Apply(string.Empty);
            }
            EditorGUILayout.EndHorizontal();

            if (_lastId == _target.BuildingId && _target.BuildingId != string.Empty)
            {
                return;
            }

            _lastId = _target.BuildingId;
            FindKeys();
        }

        private void DrawScrollView()
        {
            if (_keys.Count == 0)
            {
                return;
            }

            EditorGUILayout.BeginVertical();
            _scrollViewPosition = EditorGUILayout.BeginScrollView(_scrollViewPosition, GUILayout.MaxHeight(_keys.Count > 5 ? 100 : 20 * _keys.Count));
            foreach (var element in _keys.Where(element => GUILayout.Button(element, GUILayout.Height(20))))
            {
                Apply(element);
                break;
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void FindKeys()
        {
            _keys.Clear();

            if (string.IsNullOrEmpty(_target.BuildingId))
            {
                _keys.AddRange(_locationsEconomy.SelectMany(economy => economy.Data)
                    .Where(id => id.Id == _locationView.LocationId).SelectMany(x => x.BuildingsIds));
            }
            else
            {
                foreach (var buildingId in _locationsEconomy.SelectMany(economy => economy.Data).Where(x => x.Id == _locationView.LocationId).SelectMany(x => x.BuildingsIds))
                {
                    if (buildingId.Substring(0, Mathf.Clamp(_target.BuildingId.Length, 0, buildingId.Length)) == _target.BuildingId &&
                        _target.BuildingId != buildingId)
                    {
                        _keys.Add(buildingId);
                    }
                }
            }
        }

        private void Apply(string id)
        {
            GUI.FocusControl(null);
            _target.BuildingId = id;
            _keys.Clear();
            EditorUtility.SetDirty(_target);
        }
    }
}