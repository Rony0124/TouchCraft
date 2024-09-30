using System;
using UnityEngine;

namespace Player
{
    public partial class PlayerController : MonoBehaviour
    {
        private PlayerMovement playerMovement;
        
        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
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
