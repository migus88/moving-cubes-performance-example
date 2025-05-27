using UnityEngine;

namespace Code.Implementations
{
    public class DddStructsArrayCubesManager : BaseCubesManager
    {
        private CubeData[] _cubesData;
        
        // Struct to hold all necessary cube data
        private struct CubeData
        {
            public Vector3 Position;
            public Vector3 Velocity;
            public float HalfSize;
            public Transform Transform;
        }

        protected override void InitializeCubes(GameObject[] cubes, Vector3[] velocities)
        {
            _cubesData = new CubeData[cubes.Length];
            
            for (var i = 0; i < cubes.Length; i++)
            {
                _cubesData[i] = new CubeData
                {
                    Position = cubes[i].transform.position,
                    Velocity = velocities[i],
                    HalfSize = cubes[i].transform.localScale.x * 0.5f,
                    Transform = cubes[i].transform
                };
            }
        }

        protected override void OnCubesDestroyed()
        {
            _cubesData = null;
        }
        
        private void Update()
        {
            if (_cubesData == null)
            {
                return;
            }
            
            for (var i = 0; i < _cubesData.Length; i++)
            {
                MoveCube(i);
            }
        }
        
        private void MoveCube(int index)
        {
            var cubeData = _cubesData[index];
            
            // Get current position
            var position = cubeData.Position;
            
            // Apply movement
            position = CubePhysicsUtility.ApplyMovement(position, cubeData.Velocity, Time.deltaTime);
            
            // Handle collisions with boundaries
            position = CubePhysicsUtility.HandleBoundaryCollisions(position, cubeData.HalfSize, _boundaries, ref cubeData.Velocity);
            
            // Handle collisions with other cubes
            position = HandleCubeCollisions(position, ref cubeData);
            
            // Update position and store updated data
            cubeData.Position = position;
            cubeData.Transform.position = position;
            _cubesData[index] = cubeData;
        }
        
        private Vector3 HandleCubeCollisions(Vector3 position, ref CubeData cube)
        {
            foreach (var other in _cubesData)
            {
                if (other.Transform == cube.Transform)
                {
                    continue;
                }

                Vector3 normal; // Updated to Vector3 for 3D collisions
                if (!CubePhysicsUtility.CheckCubeCollision(
                    position, other.Position, 
                    cube.HalfSize, other.HalfSize, 
                    out var resolvedPosition, out normal))
                {
                    continue;
                }
                
                // Update position and reflect velocity
                position = resolvedPosition;
                cube.Velocity = CubePhysicsUtility.ReflectVelocity(cube.Velocity, normal);
                break; // Handle one collision at a time
            }

            return position;
        }
    }
}
