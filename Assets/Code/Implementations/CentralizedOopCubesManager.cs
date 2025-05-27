using System.Linq;
using UnityEngine;

namespace Code.Implementations
{
    public class CentralizedOopCubesManager : BaseCubesManager
    {
        private Cube[] _cubesArr;

        protected override void InitializeCubes(GameObject[] cubes, Vector3[] velocities)
        {
            _cubesArr = cubes.Select(c => c.AddComponent<Cube>()).ToArray();
            
            for (var i = 0; i < cubes.Length; i++)
            {
                _cubesArr[i].Velocity = velocities[i];
            }
        }

        protected override void OnCubesDestroyed()
        {
            _cubesArr = null;
        }
        
        private void Update()
        {
            if (_cubesArr == null)
            {
                return;
            }
            
            for (var i = 0; i < _cubesArr.Length; i++)
            {
                MoveCube(i);
            }
        }
        
        private void MoveCube(int index)
        {
            var cube = _cubesArr[index];
            
            // Get current position
            var position = cube.Transform.position;
            
            // Apply movement
            position = CubePhysicsUtility.ApplyMovement(position, cube.Velocity, Time.deltaTime);
            
            // Handle collisions with boundaries
            position = CubePhysicsUtility.HandleBoundaryCollisions(position, cube.HalfSize, _boundaries, ref cube.Velocity);
            
            // Handle collisions with other cubes
            position = HandleCubeCollisions(position, cube);
            
            // Update position and store updated data
            cube.Transform.position = position;
            _cubesArr[index] = cube;
        }
        
        private Vector3 HandleCubeCollisions(Vector3 position, Cube cube)
        {
            foreach (var other in _cubesArr)
            {
                if (other == cube)
                {
                    continue;
                }

                Vector3 normal; // Updated to Vector3 for 3D collisions
                if (!CubePhysicsUtility.CheckCubeCollision(
                    position, other.Transform.position, 
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
