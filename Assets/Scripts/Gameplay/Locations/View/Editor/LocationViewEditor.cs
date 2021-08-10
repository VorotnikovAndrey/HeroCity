using System.Collections.Generic;
using System.Linq;
using Economies;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Locations.View.Editor
{
    [CustomEditor(typeof(LocationView))]
    public class LocationViewEditor : UnityEditor.Editor
    {
        private LocationView _target;
        private List<LocationsEconomy> _locationsEconomy;
        private Vector2 _scrollViewPosition = Vector2.zero;
        private string _lastId = string.Empty;
        private readonly List<string> _keys = new List<string>();

        private void OnEnable()
        {
            _target = target as LocationView;
            if (_target == null)
            {
                return;
            }

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
            _target.LocationId = EditorGUILayout.TextArea(_target.LocationId);
            if (GUILayout.Button("Clear", GUILayout.Width(75)))
            {
                Apply(string.Empty);
            }
            EditorGUILayout.EndHorizontal();

            if (_lastId == _target.LocationId && _target.LocationId != string.Empty)
            {
                return;
            }

            _lastId = _target.LocationId;
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

            if (string.IsNullOrEmpty(_target.LocationId))
            {
                _keys.AddRange(_locationsEconomy.SelectMany(economy => economy.Data).Select(x => x.Id));
            }
            else
            {
                foreach (LocationData data in _locationsEconomy.SelectMany(economy => economy.Data))
                {
                    if (data.Id.Substring(0, Mathf.Clamp(_target.LocationId.Length, 0, data.Id.Length)) == _target.LocationId &&
                        _target.LocationId != data.Id)
                    {
                        _keys.Add(data.Id);
                    }
                }
            }
        }

        private void Apply(string id)
        {
            GUI.FocusControl(null);
            _target.LocationId = id;
            _keys.Clear();
            EditorUtility.SetDirty(_target);
        }
    }
}
