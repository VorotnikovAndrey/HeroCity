using System;
using System.Collections.Generic;
using System.Linq;
using CameraSystem;
using Economies;
using Gameplay.Locations.View;
using UI.Popups.Components;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Gameplay.Building.View.Editor
{
    [CustomEditor(typeof(BuildingView))]
    public class BuildingViewEditor : UnityEditor.Editor
    {
        private BuildingView _target;
        private LocationView _locationView;
        private List<LocationsEconomy> _locationsEconomy;
        private BuildingsEconomy _buildingsEconomy;
        private Vector2 _scrollViewPosition = Vector2.zero;
        private string _lastId = string.Empty;
        private readonly List<string> _keys = new List<string>();

        private BuildingState _buildingState;
        private int _stage;
        private GUIStyle _errorStyle;

        private void OnEnable()
        {
            _target = target as BuildingView;
            if (_target == null)
            {
                return;
            }

            _errorStyle = new GUIStyle
            {
                normal = new GUIStyleState
                {
                    textColor = Color.red
                },
                fontStyle = FontStyle.Bold,
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter
            };

            _locationView = _target.transform.root.GetComponent<LocationView>();
            _buildingsEconomy = Resources.LoadAll<BuildingsEconomy>("").ToList().FirstOrDefault();
            _locationsEconomy = Resources.LoadAll<LocationsEconomy>("").ToList();

            BuildingState defaultState = BuildingState.Inactive;
            int defaultStage = 0;

            foreach (var state in _target.States)
            {
                if (state.Object != null && state.Object.activeSelf)
                {
                    defaultState = (BuildingState) Enum.Parse(typeof(BuildingState), state.Object.name);
                    var element = state.Stages.FirstOrDefault(x => x.Object != null && x.Object.activeSelf);
                    defaultStage = element != null ? element.Stage : 0;
                    break;
                }
            }

            _buildingState = defaultState;
            _stage = defaultStage;

            if (_target.States.Any(x => x.Object == null))
            {
                _target.States = new List<BuildingStateContainer>();
            }
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
            else
            {
                DrawCustomId();
            }

            base.OnInspectorGUI();
        }

        private void DrawCustomId()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Enter custom building id", GUILayout.Width(140));
            _target.BuildingId = EditorGUILayout.TextArea(_target.BuildingId);

            EditorGUILayout.EndHorizontal();
        }

        private void ShowValidate()
        {
            if (_target.UpgradeBar == null)
            {
                var upgradeBar = _target.gameObject.GetComponentInChildren<UpgradeBar>(true);

                if (upgradeBar == null)
                {
                    EditorGUILayout.LabelField("UpgradeBar is not found".AddColorTag(Color.red), _errorStyle);
                }
                else
                {
                    _target.UpgradeBar = upgradeBar;
                }
            }

            if (GUILayout.Button("Autofill States and stages"))
            {
                var data = _buildingsEconomy.Data.FirstOrDefault(x => x.Id == _target.BuildingId);
                if (data == null)
                {
                    EditorGUILayout.LabelField($"{_target.BuildingId.AddColorTag(Color.yellow)} is not found in BuildingEconomy".AddColorTag(Color.red), _errorStyle);
                    return;
                }

                _target.States.Clear();

                var child = _target.transform.GetComponentsInChildren<Transform>(true);
                var graphics = child.FirstOrDefault(x => x.name == "Graphics");
                if (graphics == null)
                {
                    graphics = new GameObject("Graphics").transform;
                    graphics.SetParent(_target.transform);
                    graphics.localPosition = Vector3.zero;
                    graphics.localEulerAngles = Vector3.zero;
                }

                var states = graphics.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "States");
                if (states == null)
                {
                    states = new GameObject("States").transform;
                    states.SetParent(graphics);
                    states.localPosition = Vector3.zero;
                    states.localEulerAngles = Vector3.zero;
                }

                foreach (string enumType in Enum.GetNames(typeof(BuildingState)))
                {
                    BuildingState state = (BuildingState) Enum.Parse(typeof(BuildingState), enumType);

                    BuildingStateContainer result = new BuildingStateContainer
                    {
                        State = state,
                        Stages = new List<BuildingStageElement>()
                    };

                    Transform stateHolder = _target.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == enumType);
                    if (stateHolder == null)
                    {
                        stateHolder = new GameObject(enumType).transform;
                        stateHolder.SetParent(states);
                        stateHolder.transform.localPosition = Vector3.zero;
                        stateHolder.transform.localEulerAngles = Vector3.zero;
                    }

                    result.Object = stateHolder.gameObject;

                    int stageCount = data.Upgrades.Count;

                    var stageElements = result.Object.transform.GetComponentsInChildren<BuildingStageElement>(true);

                    for (int i = 0; i < stageCount; i++)
                    {
                        var index = i;

                        if (state == BuildingState.Active)
                        {
                            index++;
                        }
                        else if ((state == BuildingState.NotAvailable || state == BuildingState.Inactive) && index != 0)
                        {
                            continue;
                        }

                        BuildingStageElement stage = stageElements.FirstOrDefault(x => x.Stage == index);
                        if (stage == null)
                        {
                            var newStage = new GameObject().transform;
                            newStage.SetParent(result.Object.transform);
                            newStage.transform.localPosition = Vector3.zero;
                            newStage.transform.localEulerAngles = Vector3.zero;
                            stage = newStage.transform.AddComponent<BuildingStageElement>();
                            stage.Object = newStage.gameObject;
                            stage.Stage = index;
                        }

                        result.Stages.Add(stage);
                        stage.gameObject.name = $"Stage_{index}";
                        stage.Object.transform.SetAsLastSibling();
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
                if (state.Object == null)
                {
                    continue;
                }

                state.Object.SetActive(state.State == tempBuildingState);
            }

            if (_buildingState != tempBuildingState)
            {
                EditorUtility.SetDirty(_target);
            }

            _buildingState = tempBuildingState;

            BuildingStateContainer container = _target.States.FirstOrDefault(x => x.State == _buildingState);
            if (container?.Stages == null)
            {
                return;
            }

            var temp = container.Stages.FirstOrDefault(x => x.Object != null && x.Object.activeSelf);
            _stage = temp != null ? temp.Stage : 0;

            int minStage = -1;
            int maxStage = -1;

            foreach (var element in container.Stages)
            {
                if (element.Stage < minStage || minStage == -1)
                {
                    minStage = element.Stage;
                }

                if (element.Stage > maxStage || maxStage == -1)
                {
                    maxStage = element.Stage;
                }
            }

            if (GUILayout.Button("Prev"))
            {
                _stage = Mathf.Clamp(_stage - 1, minStage, maxStage);
                SetState(container, _stage);
            }

            if (GUILayout.Button("Next"))
            {
                _stage = Mathf.Clamp(_stage + 1, minStage, maxStage);
                SetState(container, _stage);
            }

            if (GUILayout.Button("Reset"))
            {
                _stage = minStage;
                SetState(container, _stage);
                _buildingState = BuildingState.Inactive;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private void SetState(BuildingStateContainer container, int state)
        {
            foreach (var element in container.Stages)
            {
                element.Object.SetActive(element.Stage == state);
            }

            EditorUtility.SetDirty(_target);
        }

        private bool LocationViewIsNotNull()
        {
            if (_locationView == null)
            {
                EditorGUILayout.LabelField("LocationView is not found!", _errorStyle);
                return true;
            }

            if (string.IsNullOrEmpty(_locationView.LocationId))
            {
                EditorGUILayout.LabelField("LocationId is null or empty!", _errorStyle);
                return true;
            }

            if (_locationsEconomy == null)
            {
                EditorGUILayout.LabelField("LocationsEconomy is null!", _errorStyle);
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