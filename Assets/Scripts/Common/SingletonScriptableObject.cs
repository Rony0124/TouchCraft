using System;
using System.Linq;
using UnityEngine;

namespace Common
{
    public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
    {
        [SerializeField]
        private bool isNotDestroyed = false;
        public bool IsNotDestroyed => isNotDestroyed;
        
        private static bool _isInstantiated = false;
        public static bool IsInstantiated => _isInstantiated;
        
        private static T _instance;
        public static T Instance {
            get {
                if (_instance == null) {
                    T[] assets = Resources.LoadAll<T>("SSO");
                    if (assets == null || assets.Length < 1) {
                        Debug.LogError($"There is not {typeof(T).Name} in Resources/Managers");
                    } else if (assets.Length > 1) {
                        Debug.LogWarning($"There is more than one {typeof(T).Name} in Resources/Managers");
                    } else
                    {
                        foreach (var asset in assets)
                        {
                            if(asset.name != typeof(T).Name)
                                continue;
                            
                            _instance = Instantiate(assets.First());
                            _isInstantiated = true;
                            
                            if (_instance.IsNotDestroyed) {
                                DontDestroyOnLoad(_instance);
                            }
                        }
                    }
                }

                return _instance;
            }
        }
        
        private void Awake()
        {
            Initialize();
        }

        public virtual void Initialize() { }
    }
}
