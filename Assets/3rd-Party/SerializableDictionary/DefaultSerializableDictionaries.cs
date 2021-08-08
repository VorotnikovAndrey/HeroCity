using System;
using UnityEngine;

[Serializable]
public class StringStringDictionary : SerializableDictionary<string, string> { }

[Serializable]
public class IntStringDictionary : SerializableDictionary<int, string> { }

[Serializable]
public class StringTransformDictionary : SerializableDictionary<string, Transform> { }

[Serializable]
public class StringGameObjectDictionary : SerializableDictionary<string, GameObject> { }

[Serializable]
public class GameObjectPairDictionary : SerializableDictionary<GameObject, GameObject> { }

[Serializable]
public class GameObjectPSDictionary : SerializableDictionary<GameObject, ParticleSystem> { }