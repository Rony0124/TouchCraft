using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameplayAbilitySystem.Task
{
    [CreateAssetMenu(menuName = "Gameplay Task/FindClueTask", fileName = "FindClueTask", order = 100)]
    public class FindClueTask : GameplayTask
    {
        public override async UniTask Execute(GameplayAbilitySO ability, GameplayActor sourceActor,
            GameplayActor.TargetSpec targetSpec)
        {
            await UniTask.CompletedTask;
        }
    }
}
