using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Time
{
    [CreateAssetMenu(fileName = "DayTimeParamsData", menuName = "SO/DayTimeParams")]
    public class DayTimeParams : ScriptableObject
    {
        public List<DayTimeVariant> Data = new List<DayTimeVariant>();
        [Space]
        public List<Material> Materials;
        [Space]
        public List<Material> GlassMaterials = new List<Material>();
        [Space]
        public Color _glassColorOn;
        public Color _glassColorOff;
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