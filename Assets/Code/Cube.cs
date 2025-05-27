using UnityEngine;

namespace Code
{
    /// <summary>
    /// Minimalist component that stores only the velocity data for a cube.
    /// </summary>
    public class Cube : MonoBehaviour
    {
        public Vector3 Velocity;
        public float HalfSize { get; private set; }
        public Transform Transform { get; private set; }

        private void Awake()
        {
            HalfSize = transform.localScale.x * 0.5f;
            Transform = transform;
        }
    }
}
