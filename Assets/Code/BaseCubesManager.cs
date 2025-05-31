using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code
{
    public abstract class BaseCubesManager : MonoBehaviour
    {
        [SerializeField] private int _seed = 123;
        [SerializeField] protected int _cubesAmount = 600;
        [SerializeField] protected GameObject _cubePrefab;
        [SerializeField] protected Bounds _boundaries = new(Vector3.zero, new Vector3(15, 15, 15));
        [SerializeField] protected float _minSpeed = 5f;
        [SerializeField] protected float _maxSpeed = 10f;
        [SerializeField] private float _minSize = 0.5f;
        [SerializeField] private float _maxSize = 1.3f;
        
        private GameObject[] _cubes;
        private Vector3[] _velocities;

        private void OnEnable()
        {
            Application.targetFrameRate = 500;
            CreateCubes();
            InitializeCubes(_cubes, _velocities);
        }

        private void OnDisable()
        {
            DestroyCubes();
            OnCubesDestroyed();
        }
        
        public void SetAmount(string amountStr)
        {
            if (string.IsNullOrWhiteSpace(amountStr) || !int.TryParse(amountStr, out var amount) || amount <= 0)
            {
                throw new ArgumentException("Invalid cubes amount provided.");
            }

            _cubesAmount = amount;
        }
        
        public void SetMinSize(float sizeStr) => _minSize = sizeStr;
        public void SetMaxSize(float sizeStr) => _maxSize = sizeStr;
        

        protected abstract void InitializeCubes(GameObject[] cubes, Vector3[] velocities);
        protected abstract void OnCubesDestroyed();

        private void DestroyCubes()
        {
            // Clean up cube instances
            if (_cubes == null)
            {
                return;
            }
            
            foreach (var cube in _cubes)
            {
                if (cube)
                {
                    Destroy(cube.gameObject);
                }
            }
                
            _cubes = null;
        }
        
        private void CreateCubes()
        {
            if (!_cubePrefab)
            {
                throw new Exception("Cube prefab is not assigned.");
            }

            if (_cubesAmount <= 0)
            {
                throw new Exception("Cubes amount must be greater than zero.");
            }

            _cubes = new GameObject[_cubesAmount];
            _velocities = new Vector3[_cubesAmount];

            Random.InitState(_seed);
            
            for (var i = 0; i < _cubesAmount; i++)
            {
                var cubeObject = Instantiate(_cubePrefab, transform);
                var meshRenderer = cubeObject.GetComponent<Renderer>();
                
                if(!meshRenderer)
                    meshRenderer = cubeObject.GetComponentInChildren<Renderer>();
                
                // Generate random size
                var size = Random.Range(_minSize, _maxSize);
                cubeObject.transform.localScale = Vector3.one * size;
                
                // Generate random color
                var randomColor = new Color(
                    Random.value,
                    Random.value,
                    Random.value
                );
                meshRenderer.material.color = randomColor;
                
                // Generate random velocity
                var speed = Random.Range(_minSpeed, _maxSpeed);
                var direction = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f), // Add Y direction for 3D movement
                    Random.Range(-1f, 1f)
                ).normalized;
                
                _velocities[i] = direction * speed;

                // Position cubes in 3D space using the new boundaries
                cubeObject.transform.position = new Vector3(
                    Random.Range(_boundaries.min.x, _boundaries.max.x),
                    Random.Range(_boundaries.min.y, _boundaries.max.y),
                    Random.Range(_boundaries.min.z, _boundaries.max.z));

                _cubes[i] = cubeObject;
            }
        }
    }
}

