using System;
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

        private BuildingState _buildingState;
        private int _stage;

        private void OnEnable()
        {
            _target = target as BuildingView;
            if (_target == null)
            {
                return;
            }

            _locationView = _target.transform.root.GetComponent<LocationView>();
            _locationsEconomy = Resources.LoadAll<LocationsEconomy>("").ToList();

            BuildingStateContainer defaultState = _target.States.FirstOrDefault(x => x.Object.activeSelf);
            int defaultStage = 0;

            if (defaultState?.Stages != null)
            {
                var element = defaultState.Stages.FirstOrDefault(x => x.activeSelf);
                defaultStage = element != null ? element.transform.GetSiblingIndex() : 0;
            }

            _buildingState = defaultState?.State ?? BuildingState.Inactive;
            _stage = defaultStage;
        }

        public override void OnInspectorGUI()
        {
            ShowValidate();
            ShowStateAndStageMenu();

            if (!LocationViewIsNotNull())
            {
                EditorGUILayout.Space();
                DrawId();
                DrawScrollView();
                EditorGUILayout.Space();
            }

            base.OnInspectorGUI();
        }

        private void ShowValidate()
        {
            if (GUILayout.Button("Validate"))
            {
                _target.States.Clear();

                foreach (string element in Enum.GetNames(typeof(BuildingState)))
                {
                    BuildingState state = (BuildingState) Enum.Parse(typeof(BuildingState), element);

                    BuildingStateContainer result = new BuildingStateContainer
                    {
                        State = state,
                        Stages = new List<GameObject>()
                    };

                    Transform[] childs = _target.GetComponentsInChildren<Transform>(true);
                    Transform findObject = childs.FirstOrDefault(x => x.name == element);

                    if (findObject == null)
                    {
                        findObject = new GameObject(element).transform;
                        findObject.SetParent(childs.FirstOrDefault(x => x.name == "States"));
                        findObject.transform.localPosition = Vector3.zero;
                        findObject.transform.localEulerAngles = Vector3.zero;
                    }

                    if (findObject != null)
                    {
                        result.Object = findObject.gameObject;
                    }

                    for (int i = 0; i < result.Object.transform.childCount; i++)
                    {
                        result.Stages.Add(result.Object.transform.GetChild(i).gameObject);
                    }

                    _target.States.Add(result);
                }

                EditorUtility.SetDirty(_target);
            }
        }

        private void ShowStateAndStageMenu()
        {
            EditorGUILayout.Space();

            if (_target == null || _target.States == null || _target.States.Count == 0)
            {
                return;
            }

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();

            var tempBuildingState = (BuildingState)EditorGUILayout.EnumPopup(_buildingState, GUILayout.Width(100));
            foreach (BuildingStateContainer state in _target.States)
            {
                if (state.Object == null || state.Stages == null || state.Stages.Count == 0)
                {
                    continue;
                }

                state.Object?.SetActive(state.State == tempBuildingState);
            }

            if (_buildingState != tempBuildingState)
            {
                EditorUtility.SetDirty(_target);
            }

            _buildingState = tempBuildingState;

            BuildingStateContainer currentState = _target.States.FirstOrDefault(x => x.State == _buildingState);
            if (currentState == null)
            {
                return;
            }

            if (GUILayout.Button("Prev"))
            {
                _stage = Mathf.Clamp(_stage - 1, 0, currentState.Stages.Count - 1);
                SetState(currentState, _stage);
            }

            if (GUILayout.Button("Next"))
            {
                _stage = Mathf.Clamp(_stage + 1, 0, currentState.Stages.Count - 1);
                SetState(currentState, _stage);
            }

            if (GUILayout.Button("Reset"))
            {
                _stage = 0;
                SetState(currentState, _stage);
                _buildingState = BuildingState.Inactive;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            void SetState(BuildingStateContainer container, int state)
            {
                foreach (GameObject element in container.Stages)
                {
                    element.SetActive(element.transform.GetSiblingIndex() == state);
                }

                EditorUtility.SetDirty(_target);
            }
        }

        private bool LocationViewIsNotNull()
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
            foreach (string element in _keys.Where(element => GUILayout.Button(element, GUILayout.Height(20))))
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
                foreach (string buildingId in _locationsEconomy.SelectMany(economy => economy.Data).Where(x => x.Id == _locationView.LocationId).SelectMany(x => x.BuildingsIds))
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