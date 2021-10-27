using System;
using Gameplay.Characters;
using Source;
using UnityEditor;

[CustomPropertyDrawer(typeof(StatTypeDictionary))]
[CustomPropertyDrawer(typeof(IntStringDictionary))]
[CustomPropertyDrawer(typeof(StringStringDictionary))]
[CustomPropertyDrawer(typeof(GameObjectPairDictionary))]
[CustomPropertyDrawer(typeof(StringGameObjectDictionary))]
[CustomPropertyDrawer(typeof(GameObjectPSDictionary))]
[CustomPropertyDrawer(typeof(StringTransformDictionary))]
[CustomPropertyDrawer(typeof(MapWaypointVector3Dictionary))]

public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }

[Serializable]
public class StatTypeDictionary : SerializableDictionary<StatType, float> { }