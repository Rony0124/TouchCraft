using UnityEngine;

namespace Player
{
    public class PlayerInteractableFinder : MonoBehaviour
    {
        [HideInInspector]
        public GameObject targetItem;
        public LayerMask targetItemLayer;
        public float castDistance = 0.5f;

        void Update()
        {
            if (Camera.main == null)
                return;

            var screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            Camera main;
            var cameraRay = (main = Camera.main).ScreenPointToRay(screenCenter);

            var distance = (transform.position - main.transform.position).magnitude;

            if (!Physics.Raycast(cameraRay, out var hit, distance + castDistance, targetItemLayer))
            {
                targetItem = null;
                return;
            }

            var fieldItem = hit.collider.gameObject;

            targetItem = fieldItem;
        }
    }
}
