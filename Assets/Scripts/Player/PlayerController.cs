using System;
using GameplayAbilitySystem;
using UnityEngine;

namespace Player
{
    public partial class PlayerController : MonoBehaviour
    {
        private CharacterMovement characterMovement;
        private GameplayActor _actor;
        
        private void Awake()
        {
            characterMovement = GetComponent<CharacterMovement>();
            _actor = GetComponent<GameplayActor>();
        }

        private void OnEnable()
        {
            InitInput();
        }

        private void OnDisable()
        {
            DisableInput();
        }
    }
}
