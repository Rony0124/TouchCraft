using Unity.Collections;
using UnityEditor;
using UnityEngine;

namespace GameplayAbility
{
    [CreateAssetMenu(menuName = "Gameplay Tag", fileName = "New Gameplay Tag", order = 101)]
    public class GameplayTagSO : ScriptableObject
    {
        [SerializeField, ReadOnly]
        private GameplayTag _serializedTag;
        public GameplayTag tag => _serializedTag;
                
#if UNITY_EDITOR
        public void OnValidate()
        {
            _serializedTag = GameplayTag.FromString(name);
            
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
