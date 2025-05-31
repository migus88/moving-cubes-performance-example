using UnityEngine;

namespace Code
{
    public class MouseRaycastDestroyer : MonoBehaviour
    {
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private Camera _raycastCamera;
        
        private void Start()
        {
            // If no camera is assigned, use the main camera
            if (_raycastCamera == null)
            {
                _raycastCamera = Camera.main;
            }
        }
        
        private void Update()
        {
            // Check for mouse click (left button)
            if (Input.GetMouseButtonDown(0))
            {
                RaycastAndDestroy();
            }
        }
        
        private void RaycastAndDestroy()
        {
            var ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _targetLayer))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
