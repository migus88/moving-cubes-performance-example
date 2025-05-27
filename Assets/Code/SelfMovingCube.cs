using UnityEngine;

namespace Code
{
    public class SelfMovingCube : MonoBehaviour
    {
        // Internal fields
        private Vector3 _velocity;
        private Transform _cachedTransform;
        private Bounds _boundaries;
        private float _halfSize;
        private SelfMovingCube[] _otherCubes;
        
        private void Awake()
        {
            // Cache transform for better performance
            _cachedTransform = transform;
        }
        
        private void Update()
        {
            Move(Time.deltaTime);
        }
        
        public void Initialize(Bounds boundaries, float minSpeed, float maxSpeed, Vector3 velocity, SelfMovingCube[] otherCubes)
        {
            // Cache boundary values
            _boundaries = boundaries;
            _otherCubes = otherCubes;
            _velocity = velocity;
            
            _halfSize = transform.localScale.x * 0.5f;
        }
        
        // Legacy initialization method for backward compatibility
        public void Initialize(Rect boundariesRect, float minSpeed, float maxSpeed, Vector3 velocity, SelfMovingCube[] otherCubes)
        {
            // Convert 2D rect to 3D bounds
            _boundaries = new Bounds(
                new Vector3((boundariesRect.xMin + boundariesRect.xMax) * 0.5f, 0, (boundariesRect.yMin + boundariesRect.yMax) * 0.5f),
                new Vector3(boundariesRect.width, 20, boundariesRect.height)
            );
            
            _otherCubes = otherCubes;
            _velocity = velocity;
            _halfSize = transform.localScale.x * 0.5f;
        }
        
        private void Move(float deltaTime)
        {
            // Get current position
            var position = _cachedTransform.position;
            
            // Apply movement
            position = CubePhysicsUtility.ApplyMovement(position, _velocity, deltaTime);
            
            // Handle collisions
            position = CubePhysicsUtility.HandleBoundaryCollisions(position, _halfSize, _boundaries, ref _velocity);
            position = HandleCubeCollisions(position);
            
            // Update position
            _cachedTransform.position = position;
        }
        
        private Vector3 HandleCubeCollisions(Vector3 position)
        {
            if (_otherCubes == null) return position;
            
            foreach (var cube in _otherCubes)
            {
                if (!cube || cube == this)
                {
                    continue;
                }

                Vector3 normal; // Updated to Vector3 for 3D collisions
                if (!CubePhysicsUtility.CheckCubeCollision(
                    position, cube._cachedTransform.position, 
                    _halfSize, cube._halfSize, 
                    out var resolvedPosition, out normal))
                {
                    continue;
                }
                
                // Update position and reflect velocity
                position = resolvedPosition;
                _velocity = CubePhysicsUtility.ReflectVelocity(_velocity, normal);
                break; // Handle one collision at a time
            }
            
            return position;
        }
    }
}
