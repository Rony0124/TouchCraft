using System;
using GameplayAbilitySystem;
using UnityEngine;

namespace Player
{
    public partial class PlayerController : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        private GameplayActor _actor;
        
        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
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
