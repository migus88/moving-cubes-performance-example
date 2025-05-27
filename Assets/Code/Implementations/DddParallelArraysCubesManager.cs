using UnityEngine;

namespace Code.Implementations
{
    public class DddParallelArraysCubesManager : BaseCubesManager
    {
        // Parallel arrays for cube data
        private Vector3[] _positions;
        private Vector3[] _velocities;
        private float[] _halfSizes;
        private Transform[] _transforms;
        
        protected override void InitializeCubes(GameObject[] cubes, Vector3[] velocities)
        {
            _positions = new Vector3[cubes.Length];
            _velocities = new Vector3[cubes.Length];
            _halfSizes = new float[cubes.Length];
            _transforms = new Transform[cubes.Length];
            
            for (var i = 0; i < cubes.Length; i++)
            {
                _positions[i] = cubes[i].transform.position;
                _velocities[i] = velocities[i];
                _halfSizes[i] = cubes[i].transform.localScale.x * 0.5f;
                _transforms[i] = cubes[i].transform;
            }
        }

        protected override void OnCubesDestroyed()
        {
            _positions = null;
            _velocities = null;
            _halfSizes = null;
            _transforms = null;
        }
        
        private void Update()
        {
            if (_positions == null)
            {
                return;
            }
            
            for (var i = 0; i < _positions.Length; i++)
            {
                MoveCube(i);
            }
        }
        
        private void MoveCube(int index)
        {
            // Get current position
            var position = _positions[index];
            
            // Apply movement
            position = CubePhysicsUtility.ApplyMovement(position, _velocities[index], Time.deltaTime);
            
            // Handle collisions with boundaries
            position = CubePhysicsUtility.HandleBoundaryCollisions(position, _halfSizes[index], _boundaries, ref _velocities[index]);
            
            // Handle collisions with other cubes
            position = HandleCubeCollisions(position, index);
            
            // Update position
            _positions[index] = position;
            _transforms[index].position = position;
        }
        
        private Vector3 HandleCubeCollisions(Vector3 position, int cubeIndex)
        {
            var halfSize = _halfSizes[cubeIndex];
            
            for (var i = 0; i < _positions.Length; i++)
            {
                if (i == cubeIndex)
                {
                    continue;
                }

                Vector3 normal; // Updated to Vector3 for 3D collisions
                if (!CubePhysicsUtility.CheckCubeCollision(
                    position, _positions[i], 
                    halfSize, _halfSizes[i], 
                    out var resolvedPosition, out normal))
                {
                    continue;
                }
                
                // Update position and reflect velocity
                position = resolvedPosition;
                _velocities[cubeIndex] = CubePhysicsUtility.ReflectVelocity(_velocities[cubeIndex], normal);
                break; // Handle one collision at a time
            }

            return position;
        }
    }
}
