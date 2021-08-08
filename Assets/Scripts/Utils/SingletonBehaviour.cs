using UnityEngine;

namespace Defong.Utils
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T instance;

        public virtual void Awake()
        {
            if (instance)
            {
                Debug.LogWarning("Duplicate subclass of type " + typeof(T) + "! eliminating " + name + " while preserving " + instance.name);
                Destroy(gameObject);
            }
            else
            {
                instance = this as T;
            }
        }

        public virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }  
        }
    }
}