using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameplayAbilitySystem
{
    public abstract class GameplayTask : ScriptableObject
    {
        [NonSerialized]
        protected CancellationTokenSource CancellationTokenSource;
        
        public bool CanExecute(GameplayAbilitySO ability, GameplayActor sourceActor, in GameplayActor.TargetSpec targetSpec)
        {
            return false;
        }
        
        public virtual void PreExecute(GameplayAbilitySO ability, GameplayActor sourceActor, GameplayActor.TargetSpec targetSpec, CancellationTokenSource cancellationTokenSource)
        {
            CancellationTokenSource = cancellationTokenSource;
        }

        public virtual async UniTask Execute(GameplayAbilitySO ability, GameplayActor sourceActor, GameplayActor.TargetSpec targetSpec)
        {
            await UniTask.CompletedTask;
        }
        
        public virtual void EndExecute(GameplayAbilitySO ability, GameplayActor sourceActor, GameplayActor.TargetSpec targetSpec)
        {
            
        }
        
        public virtual void CancelExecuteServer()
        {
            CancellationTokenSource?.Cancel();
        }
    }
}
