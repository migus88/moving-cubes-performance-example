using System.Linq;
using UnityEngine;

namespace Code.Implementations
{
    public class DecentralizedOopCubesManager : BaseCubesManager
    {
        protected override void InitializeCubes(GameObject[] cubes, Vector3[] velocities)
        {
            var cubeComponents = cubes.Select(c => c.AddComponent<SelfMovingCube>()).ToArray();

            for (var index = 0; index < cubeComponents.Length; index++)
            {
                var cube = cubeComponents[index];
                var velocity = velocities[index];
                cube.Initialize(_boundaries, velocity, cubeComponents);
            }
        }

        protected override void OnCubesDestroyed()
        {
        }
    }
}
