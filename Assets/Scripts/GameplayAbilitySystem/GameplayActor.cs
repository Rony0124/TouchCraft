using UnityEngine;

namespace GameplayAbilitySystem
{
    public partial class GameplayActor : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            
            foreach (var defaultAbility in defaultAbilities)
            {
                if (defaultAbility == null)
                {
                    Debug.LogWarning($"Default ability is null in {name}");
                    continue;
                }
                
                grantedAbilities.Add(defaultAbility, default);
            }
        }
    }
}
