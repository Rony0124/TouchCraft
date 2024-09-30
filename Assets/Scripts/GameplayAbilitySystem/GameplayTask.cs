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
        
        public bool CanExecute(GameplayAbilitySO ability, GameplayActor sourceActor)
        {
            return false;
        }

        public virtual async UniTask Execute(GameplayAbilitySO ability, GameplayActor sourceActor, GameplayActor targetSpec)
        {
            await UniTask.CompletedTask;
        }
    }
}
