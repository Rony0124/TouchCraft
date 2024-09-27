using System;
using UnityEngine;

namespace GameplayAbility
{
    [CreateAssetMenu(menuName = "Gameplay Ability", fileName = "New Gameplay Ability", order = 100)]
    public class GameplayAbilitySO : ScriptableObject
    {
        [Flags]
        public enum TargetFlag
        {
            Self = 1 << 0,
            Actor = 1 << 1,
        }
        
        [Header("Targeting")] 
        public TargetFlag targetFlag;
        
        [Tooltip("올바른 타겟이 없으면 발동되지 않는다.")]
        public bool targetRequired;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
