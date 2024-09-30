using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameplayAbilitySystem
{
    public partial class GameplayActor
    {
        [Serializable]
        public struct TargetSpec : IEquatable<TargetSpec>
        {
            // 타겟이 필요한 경우
            public GameplayActor actor;

            public List<GameplayTagSO> tags;
            
            // 버려진 아이템을 주을때 
            public GameObject tagContainer;

            public bool Equals(TargetSpec other)
            {
                return Equals(actor, other.actor)
                       && Equals(tagContainer, other.tagContainer);
            }

            public override bool Equals(object obj) => obj is TargetSpec other && Equals(other);

            public override int GetHashCode() => HashCode.Combine(actor, tagContainer);
        }
        
        [Serializable]
        public struct CastingAbility
        {
            public GameplayAbilitySO ability;
            public uint castStartTick;
            public uint castEndTick;
            public TargetSpec targetSpec;
        }
        
        [Serializable]
        public struct AbilityState
        {
            public uint startTick;
            public UniTask[] tasks;
            public bool isExecuting => startTick != 0;
        }
        
        public List<GameplayAbilitySO> defaultAbilities;
        public Dictionary<GameplayAbilitySO, AbilityState> grantedAbilities = new ();
        
        private CastingAbility _currentCastAbility;
        private Coroutine _castAbilityCoroutine;
        
       private CastAbilityResult CanCastAbility(GameplayAbilitySO ability, TargetSpec targetSpec)
        {
            if (_currentCastAbility.ability == ability && _currentCastAbility.targetSpec.Equals(targetSpec))
                return CastAbilityResult.AlreadyCastingSameAbilityTarget;

            return CanExecuteAbility(ability, targetSpec);
        }

        private CastAbilityResult CanExecuteAbility(GameplayAbilitySO ability, TargetSpec targetSpec)
        {
            if (ability == null || !grantedAbilities.ContainsKey(ability))
                return CastAbilityResult.NotGranted;
         
            //타겟이 존재하지 않는다
            if (ability.targetRequired)
            {
                if (ability.HasTargetFlag(GameplayAbilitySO.TargetFlag.Actor) && targetSpec.actor == null)
                    return CastAbilityResult.NoTarget;
            }
            
            //현재 실행중인 ability 중에 들어온 행위요청에 대한 block권한을 가지고 있는지 체크
            foreach (var kvp in grantedAbilities)
            {
                var grantedAbility = kvp.Key;
                if(!GameplayTag.HasAny(grantedAbility.blockAbilitiesWithTags, ability.activationOwnedTags)) continue;

                if(kvp.Value.isExecuting)
                    return CastAbilityResult.Block;
            }

            //현재 실행중인 ability중에, 들어온 요청에 대해 override 권한을 가지고 있는지 체크, 있다면 실행중인 ability를 cancel해준다
            if (grantedAbilities.TryGetValue(ability, out var state))
            {
                if (state.isExecuting)
                {
                    CastAbilityResult result = CastAbilityResult.AlreadyExecuting;
                    
                    foreach (var kvp in grantedAbilities)
                    {
                        var grantedAbility = kvp.Key;

                        if (GameplayTag.HasAny(grantedAbility.activationOwnedTags, ability.cancelAbilitiesWithTags))
                        {
                            foreach (var task in grantedAbility.tasks)
                            {
                                task.CancelExecuteServer();
                            }

                            result = CastAbilityResult.Success;
                        }
                    }
                    return result;
                }
            }
            
            
            /*
            if (!CheckCastAbilityCost(ability, targetSpec))
                return CastAbilityResult.NoCost;

            if (!CheckCastAbilityCooldown(ability, targetSpec))
                return CastAbilityResult.Cooldown;*/
            
            return CastAbilityResult.Success;
        }
        
        public void StopCastAbility()
        {
            if (_castAbilityCoroutine != null)
                StopCoroutine(_castAbilityCoroutine);
            
            _castAbilityCoroutine = null;
            _currentCastAbility = default;
        }

        public CastAbilityResult CastAbility(GameplayAbilitySO ability, TargetSpec targetSpec)
        {
            CastAbilityResult result = CanCastAbility(ability, targetSpec);
            if(result != CastAbilityResult.Success)
                return result;

            StopCastAbility();

            if (ability.castingDuration > 0)
            {
                CastingAbility castAbility;
                castAbility.ability = ability;
                //tick은 추후에...
                castAbility.castStartTick = 0;
                castAbility.castEndTick = 0;
                castAbility.targetSpec = targetSpec;
                _currentCastAbility = castAbility;
                
                PlayCastingAnimation(ability, targetSpec);
            }
            else
            {
                ExecuteAbility(ability, targetSpec).Forget();
            }
            
            return CastAbilityResult.Success;
        }
        
        private void PlayCastingAnimation(GameplayAbilitySO ability, TargetSpec targetSpec)
        {
            if (PlayCastingAnimationVariation(ability, targetSpec))
                return;
                    
            SetAnimationBoolean(Animator.StringToHash("Casting_" + ability.defaultCastingAnimationBoolean), true);
        }

        private bool PlayCastingAnimationVariation(GameplayAbilitySO ability, TargetSpec targetSpec)
        {
            if (ability.HasTargetFlag(GameplayAbilitySO.TargetFlag.ClueItem))
            {
                foreach (var abilityCastingAnimationBoolean in ability.castingAnimationVariations)
                {
                    if (!GameplayTag.HasTag(targetSpec.tags, abilityCastingAnimationBoolean.requiredTag.tag)) continue;
                        
                    SetAnimationBoolean(Animator.StringToHash("Casting_" + abilityCastingAnimationBoolean.animationBoolean), true);
                    return true;
                }
            }

            return false;
        }
        
        private void SetAnimationBoolean(int paramNameHash, bool value)
        {
            _animator.SetBool(paramNameHash, value);
        }

        private async UniTask ExecuteAbility(GameplayAbilitySO ability, TargetSpec targetSpec)
        {
            foreach (var task in ability.tasks)
                if (task.CanExecute(ability, this, targetSpec) == false)
                    return;
            
            if (CanExecuteAbility(ability, targetSpec) != CastAbilityResult.Success)
                return;
            
            // Ability의 모든 Task는 한번에 실행되고 모든 Task가 완료될때까지 Abiltiy는 실행중으로 간주한다. 
            grantedAbilities.TryGetValue(ability, out var state);
            
            if(state.tasks == null || state.tasks.Length != ability.tasks.Length)
                state.tasks = new UniTask[ability.tasks.Length];
            
            CancellationTokenSource cancellationTokenSource = new();
            for (var i = 0; i < ability.tasks.Length; ++i)
            {
                ability.tasks[i].PreExecute(ability, this, targetSpec, cancellationTokenSource);
                state.tasks[i] = ability.tasks[i].Execute(ability, this, targetSpec);
            }
            
            grantedAbilities[ability] = state;

            // 여기서 Task가 끝날때까지 실행 제어권을 반환하고 기다린다.
            await UniTask.WhenAll(state.tasks);
            
            // 모든 Task가 종료되면 마지막으로 EndExecuteServer를 호출한다.
            for(int i = 0; i < ability.tasks.Length; ++i)
                ability.tasks[i].EndExecute(ability, this, targetSpec);

            state.startTick = 0;
            grantedAbilities[ability] = state;
        }
    }
}
