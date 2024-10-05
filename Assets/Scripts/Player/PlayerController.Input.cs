using System.Linq;
using GameplayAbilitySystem;
using InGame.Item;
using PlayerInput;
using UnityEngine;
using UnityEngine.Rendering;
using Util;

namespace Player
{
    public partial class PlayerController
    {
        [SerializeField]
        private SerializedDictionary<GameplayTagSO, GameplayAbilitySO> interactAbilities;

        [SerializeField] 
        private PlayerInteractableFinder interactableFinder;
        
        private void InitInput()
        {
            PlayerInputHandler.Instance.OnInputTriggered += OnInputAction;
        }
        
        private void DisableInput()
        {
            PlayerInputHandler.Instance.OnInputTriggered -= OnInputAction;
        }

        private void OnInputAction(PlayerInputKey playerInputKey)
        {
            switch (playerInputKey)
            {
                case PlayerInputKey.Interact:
                    OnInteract();
                    
                    break;
                case PlayerInputKey.Run:
                    OnRun();
                    
                    break;
                case PlayerInputKey.None:
                default:
                    break;
            }
        }

        private void OnInteract()
        {
            if(interactableFinder == null)
                return;
            
            var target = interactableFinder.targetItem;
            if(!target)
                return;

            var interactable = target.GetComponent<Interactable>();
            var targetActor = target.GetComponent<GameplayActor>();

            if (interactAbilities.TryGetValue(interactable.interactTag, out var gameplayAbility))
            {
                GameplayActor.TargetSpec targetSpec = new();
                targetSpec.tags.Add(interactable.interactTag);
                targetSpec.actor = targetActor;
                _actor.CastAbility(gameplayAbility, targetSpec);
            }
        }

        private void OnRun()
        {
            _playerMovement.isRun = true;
        }
    }
}
