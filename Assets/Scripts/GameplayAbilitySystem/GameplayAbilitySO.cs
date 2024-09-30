using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayAbilitySystem
{
    [CreateAssetMenu(menuName = "Gameplay Ability", fileName = "New Gameplay Ability", order = 100)]
    public class GameplayAbilitySO : ScriptableObject
    {
        [Flags]
        public enum TargetFlag
        {
            Self = 1 << 0,
            Actor = 1 << 1,
            ClueItem = 1 << 2,
        }
        
        [Header("Targeting")] 
        public TargetFlag targetFlag;
        
        [Tooltip("올바른 타겟이 없으면 발동되지 않는다.")]
        public bool targetRequired;
        
        public bool HasTargetFlag(TargetFlag flag) => (targetFlag & flag) != 0;
        
        [Header("Tags")]
        [Tooltip("태그로 어빌리티 취소 - 이 어빌리티 실행 도중 이미 실행 중인 어빌리티의 태그가 제공된 목록에 일치하면 취소합니다.")]
        public List<GameplayTagSO> cancelAbilitiesWithTags;
        
        [Tooltip("태그로 어빌리티 차단 - 이 어빌리티 실행 도중 다른 어빌리티의 태그가 일치하면 실행을 차단합니다.")]
        public List<GameplayTagSO> blockAbilitiesWithTags;
        
        [Tooltip("시전 오너 태그 - 이 어빌리티 실행 도중, 그 어빌리티의 오너에 이 태그 세트를 붙입니다.")]
        public List<GameplayTagSO> activationOwnedTags;
        
        [Tooltip("시전 필수 태그 - 이 어빌리티는 시전하는 액터 또는 컴포넌트에 이 태그가 전부 있을 때만 시전할 수 있습니다.")]
        public List<GameplayTagSO> activationRequiredTags;
        
        [Tooltip("시전 차단 태그 - 이 어빌리티는 시전하는 액터 또는 컴포넌트에 이 태그가 하나도 없을 때만 시전할 수 있습니다.")]
        public List<GameplayTagSO> activationBlockedTags;
        
        [Tooltip("대상 필수 태그 - 이 어빌리티는 대상 액터 또는 컴포넌트에 이 태그가 전부 있을 때만 시전할 수 있습니다.")]
        public List<GameplayTagSO> targetRequiredTags;
        
        [Tooltip("대상 차단 태그 - 이 어빌리티는 대상 액터 또는 컴포넌트에 이 태그가 하나도 없을 때만 시전할 수 있습니다.")]
        public List<GameplayTagSO> targetBlockedTags;
        
        [Serializable]
        public struct CastingAnimationVariation
        {
            public GameplayTagSO requiredTag;
            public string animationBoolean;
        }
        
        [Header("Casting")]
        public float castingDuration;
        public string defaultCastingAnimationBoolean;
        public CastingAnimationVariation[] castingAnimationVariations;

        [Header("Tasks")] 
        [SerializeReference]
        public GameplayTask[] tasks;
    }
}
