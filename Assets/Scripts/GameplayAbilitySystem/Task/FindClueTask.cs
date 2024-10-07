using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameplayAbilitySystem.Task
{
    [CreateAssetMenu(menuName = "Gameplay Task/FindClueTask", fileName = "FindClueTask", order = 100)]
    public class FindClueTask : GameplayTask
    {
        public override bool CanExecute(GameplayAbilitySO ability, GameplayActor sourceActor, in GameplayActor.TargetSpec targetSpec)
        {
            return true;
        }
        
        public override async UniTask Execute(GameplayAbilitySO ability, GameplayActor sourceActor,
            GameplayActor.TargetSpec targetSpec)
        {
            Debug.Log("Find Clue!!");
            await UniTask.CompletedTask;
            
         
        }
    }
}
