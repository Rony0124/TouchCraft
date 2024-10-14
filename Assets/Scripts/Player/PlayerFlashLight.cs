using System;
using UnityEngine;

namespace Player
{
    public class PlayerFlashLight : MonoBehaviour
    {
        private Light flashLight;
        private Transform cameraTransform; 
        void Awake()
        {
            flashLight = GetComponent<Light>();
            if (Camera.main != null) 
                cameraTransform = Camera.main.transform;
        }

        public void SetLight(bool isLightOn)
        {
            flashLight.intensity = isLightOn ? 10 : 0;
        }

        private void LateUpdate()
        {
            transform.rotation = cameraTransform.rotation;
        }
    }
}
