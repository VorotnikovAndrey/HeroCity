using System;
using System.Collections.Generic;
using DG.Tweening;
using OneLine;
using UnityEngine;

namespace Gameplay.Time
{
    [CreateAssetMenu(fileName = "DayTimeParamsData", menuName = "SO/DayTimeParams")]
    public class DayTimeParams : ScriptableObject
    {
        [OneLine] public List<TimeDataParams> TimeData = new List<TimeDataParams>();
        [Space]
        public float TimeFactor = 1;
        [Space]
        public List<DayTimeVariant> Data = new List<DayTimeVariant>();
        [Space]
        public List<Material> Materials;
        [Space] 
        public float _glassDuration;
        public Color _glassColorOn;
        public Color _glassColorOff;
        public Ease _glassEase = Ease.Linear;
        public List<Material> GlassMaterials = new List<Material>();
    }

    [Serializable]
    public class TimeDataParams
    {
        public DayTimeType Type;
        public int Time;
    }

    [Serializable]
    public class DayTimeVariant
    {
        public DayTimeType Type;
        [Space]
        [Range(0, 24)] public int IntervalStart;
        [Range(0, 24)] public int IntervalEnd;
        [Space]
        public Color MaterialColor = Color.white;
        public Color LightColor = Color.white;
        public Vector3 LightRotation;
        public Ease SwitchEase = Ease.Linear;
        public float SwitchDuratation = 5f;
    }
}