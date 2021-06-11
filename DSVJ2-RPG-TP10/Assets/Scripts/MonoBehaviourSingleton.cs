using UnityEngine;

namespace MonoBehaviourSingletonScript
{
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;
        public static T Get()
        {
            return instance;
        }

        public virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this as T;
            DontDestroyOnLoad(this);
        }
    }
}