using System.Linq;
using UnityEngine;

namespace Code.Implementations
{
    /// <summary>
    /// Decentralized cube manager that uses Unity's physics system for movement and collision detection
    /// </summary>
    public class UnityPhysicsCubesManager : BaseCubesManager
    {
        [SerializeField] private PhysicsMaterial _bouncyMaterial;
        
        private PhysicsCube[] _physicsCubes;
        private GameObject[] _boundaryWalls;

        protected override void InitializeCubes(GameObject[] cubes, Vector3[] velocities)
        {
            // Create boundary walls
            CreateBoundaryWalls();
            
            _physicsCubes = new PhysicsCube[cubes.Length];

            for (var i = 0; i < cubes.Length; i++)
            {
                var cubeObject = cubes[i];
                
                // Add Rigidbody if not present
                cubeObject.AddComponent<Rigidbody>();
                
                var col = cubeObject.GetComponentInChildren<Collider>();
                col.enabled = false;
                cubeObject.AddComponent<BoxCollider>();
                
                // Add PhysicsCube component
                var physicsCube = cubeObject.AddComponent<PhysicsCube>();
                physicsCube.Initialize(velocities[i], _bouncyMaterial);
                
                _physicsCubes[i] = physicsCube;
            }
        }

        protected override void OnCubesDestroyed()
        {
            _physicsCubes = null;
            
            // Clean up boundary walls
            if (_boundaryWalls != null)
            {
                foreach (var wall in _boundaryWalls)
                {
                    if (wall)
                    {
                        Destroy(wall);
                    }
                }
                _boundaryWalls = null;
            }
        }
        
        private void CreateBoundaryWalls()
        {
            _boundaryWalls = new GameObject[6];
            var wallThickness = 1f;
            var bounds = _boundaries;
            
            // Create wall parent for organization
            var wallParent = new GameObject("Boundary Walls");
            wallParent.transform.SetParent(transform);
            
            // Left Wall (X min)
            _boundaryWalls[0] = CreateWall("Left Wall", 
                new Vector3(bounds.min.x - wallThickness * 0.5f, bounds.center.y, bounds.center.z),
                new Vector3(wallThickness, bounds.size.y, bounds.size.z),
                wallParent.transform);
            
            // Right Wall (X max)
            _boundaryWalls[1] = CreateWall("Right Wall",
                new Vector3(bounds.max.x + wallThickness * 0.5f, bounds.center.y, bounds.center.z),
                new Vector3(wallThickness, bounds.size.y, bounds.size.z),
                wallParent.transform);
            
            // Bottom Wall (Y min)
            _boundaryWalls[2] = CreateWall("Bottom Wall",
                new Vector3(bounds.center.x, bounds.min.y - wallThickness * 0.5f, bounds.center.z),
                new Vector3(bounds.size.x, wallThickness, bounds.size.z),
                wallParent.transform);
            
            // Top Wall (Y max)
            _boundaryWalls[3] = CreateWall("Top Wall",
                new Vector3(bounds.center.x, bounds.max.y + wallThickness * 0.5f, bounds.center.z),
                new Vector3(bounds.size.x, wallThickness, bounds.size.z),
                wallParent.transform);
            
            // Back Wall (Z min)
            _boundaryWalls[4] = CreateWall("Back Wall",
                new Vector3(bounds.center.x, bounds.center.y, bounds.min.z - wallThickness * 0.5f),
                new Vector3(bounds.size.x, bounds.size.y, wallThickness),
                wallParent.transform);
            
            // Front Wall (Z max)
            _boundaryWalls[5] = CreateWall("Front Wall",
                new Vector3(bounds.center.x, bounds.center.y, bounds.max.z + wallThickness * 0.5f),
                new Vector3(bounds.size.x, bounds.size.y, wallThickness),
                wallParent.transform);
        }
        
        private GameObject CreateWall(string wallName, Vector3 position, Vector3 size, Transform parent)
        {
            var wall = new GameObject(wallName);
            wall.transform.SetParent(parent);
            wall.transform.position = position;
            
            // Add Box Collider with frictionless material
            var col = wall.AddComponent<BoxCollider>();
            col.size = size;
            col.material = _bouncyMaterial;
            
            return wall;
        }
    }
} 