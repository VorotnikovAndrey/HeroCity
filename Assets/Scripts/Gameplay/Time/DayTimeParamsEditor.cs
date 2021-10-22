using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Time
{
    [CustomEditor(typeof(DayTimeParams))]
    public class DayTimeParamsEditor : Editor
    {
        private DayTimeParams Target;

        public override void OnInspectorGUI()
        {
            Target = target as DayTimeParams;

            if (Target == null)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();

            var variants = Target.Data.ToDictionary(element => element.Type);
            Target.TimeData.Clear();

            for (int i = 0; i < 24; i++)
            {
                Color color = Color.black;

                foreach (var data in variants)
                {
                    switch (data.Key)
                    {
                        case DayTimeType.Morning:
                            if (i >= data.Value.IntervalStart && i < data.Value.IntervalEnd)
                            {
                                color = Color.cyan;
                                Target.TimeData.Add(new TimeDataParams
                                {
                                    Time = i,
                                    Type = DayTimeType.Morning
                                });
                            }
                            break;
                        case DayTimeType.Afternoon:
                            if (i >= data.Value.IntervalStart && i < data.Value.IntervalEnd)
                            {
                                color = Color.yellow;
                                Target.TimeData.Add(new TimeDataParams
                                {
                                    Time = i,
                                    Type = DayTimeType.Afternoon
                                });
                            }
                            break;
                        case DayTimeType.Evening:
                            if (i >= data.Value.IntervalStart && i < data.Value.IntervalEnd)
                            {
                                color = Color.red;
                                Target.TimeData.Add(new TimeDataParams
                                {
                                    Time = i,
                                    Type = DayTimeType.Evening
                                });
                            }
                            break;
                        case DayTimeType.Night:
                            if (i >= data.Value.IntervalStart || i < data.Value.IntervalEnd)
                            {
                                color = Color.blue;
                                Target.TimeData.Add(new TimeDataParams
                                {
                                    Time = i,
                                    Type = DayTimeType.Night
                                });
                            }
                            break;
                    }
                }

                GUI.backgroundColor = color;

                GUILayout.Button(i.ToString(), GUILayout.Height(40));
            }
            
            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;

            EditorGUILayout.Space();

            base.OnInspectorGUI();
        }
    }
}
