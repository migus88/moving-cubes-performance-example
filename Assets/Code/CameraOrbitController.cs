using UnityEngine;

namespace Code
{
    public class CameraOrbitController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _returnSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 3f;

        private float _distance;
        private bool _isDragging;
        private Transform _transform;
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;
        private Vector2 _currentRotation;
        private Vector2 _initialRotation;
        private Vector3 _lastMousePosition;
        private Vector3 _initialOffset;

        private void Awake()
        {
            _transform = transform;
        }

        private void Start()
        {
            _originalPosition = _transform.position;
            _originalRotation = _transform.rotation;
            _distance = Vector3.Distance(_originalPosition, _target.position);
            
            var initialEuler = _originalRotation.eulerAngles;
            _initialRotation = new Vector2(initialEuler.x, initialEuler.y);
            _initialOffset = Quaternion.Inverse(_originalRotation) * (_originalPosition - _target.position);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                _isDragging = true;
                _lastMousePosition = Input.mousePosition;
                _currentRotation = Vector2.zero;
            }

            if (Input.GetMouseButtonUp(1))
            {
                _isDragging = false;
            }
        }

        private void LateUpdate()
        {
            if (_isDragging)
            {
                DragWithMouse();
            }
            else
            {
                ReturnToPosition();
            }
        }

        private void ReturnToPosition()
        {
            _currentRotation.x = Mathf.Lerp(_currentRotation.x, 0, Time.deltaTime * _returnSpeed);
            _currentRotation.y = Mathf.Lerp(_currentRotation.y, 0, Time.deltaTime * _returnSpeed);
            
            _transform.position = Vector3.Lerp(_transform.position, _originalPosition, Time.deltaTime * _returnSpeed);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, _originalRotation, Time.deltaTime * _returnSpeed);
        }

        private void DragWithMouse()
        {
            const float smoothStep = 0.01f;
        
            var delta = Input.mousePosition - _lastMousePosition;
            _lastMousePosition = Input.mousePosition;

            _currentRotation.x -= delta.y * _rotationSpeed * smoothStep;
            _currentRotation.y += delta.x * _rotationSpeed * smoothStep;

            var finalRotX = _initialRotation.x + _currentRotation.x;
            var finalRotY = _initialRotation.y + _currentRotation.y;
            var rotation = Quaternion.Euler(finalRotX, finalRotY, 0);
            
            var offset = rotation * _initialOffset;
            var newPosition = _target.position + offset.normalized * _distance;
            
            _transform.position = newPosition;
            _transform.rotation = rotation;
        }
    }
}
