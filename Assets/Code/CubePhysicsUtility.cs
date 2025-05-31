using UnityEngine;

namespace Code
{
    /// <summary>
    /// Static utility class containing common cube physics operations used across different implementations
    /// </summary>
    public static class CubePhysicsUtility
    {
        /// <summary>
        /// Apply movement to a position based on velocity and delta time
        /// </summary>
        public static Vector3 ApplyMovement(Vector3 position, Vector3 velocity, float deltaTime)
        {
            return position + velocity * deltaTime;
        }
        
        /// <summary>
        /// Helper method to handle collision along a single axis
        /// </summary>
        private static float HandleAxisBoundary(float position, float halfSize, float minBoundary, float maxBoundary, ref float velocity)
        {
            var min = minBoundary + halfSize;
            var max = maxBoundary - halfSize;
            
            if (position < min)
            {
                velocity = Mathf.Abs(velocity);
                return min;
            }

            if (position > max)
            {
                velocity = -Mathf.Abs(velocity);
                return max;
            }

            return position;
        }
        
        /// <summary>
        /// Handle collisions with 3D boundaries and update velocity
        /// </summary>
        public static Vector3 HandleBoundaryCollisions(Vector3 position, float halfSize, Bounds boundaries, ref Vector3 velocity)
        {
            var result = position;
            
            result.x = HandleAxisBoundary(position.x, halfSize, boundaries.min.x, boundaries.max.x, ref velocity.x);
            result.y = HandleAxisBoundary(position.y, halfSize, boundaries.min.y, boundaries.max.y, ref velocity.y);
            result.z = HandleAxisBoundary(position.z, halfSize, boundaries.min.z, boundaries.max.z, ref velocity.z);
            
            return result;
        }
        
        /// <summary>
        /// Check for collision between two cubes
        /// </summary>
        public static bool CheckCubeCollision(
            Vector3 position1, Vector3 position2, 
            float halfSize1, float halfSize2,
            out Vector3 resolvedPosition, out Vector3 normal)
        {
            resolvedPosition = position1;
            normal = Vector3.zero;
            
            // Check for axis-aligned bounding box (AABB) collision
            var collisionX = Mathf.Abs(position1.x - position2.x) <= (halfSize1 + halfSize2);
            var collisionY = Mathf.Abs(position1.y - position2.y) <= (halfSize1 + halfSize2);
            var collisionZ = Mathf.Abs(position1.z - position2.z) <= (halfSize1 + halfSize2);
            
            // Collision only occurs when there's overlap on all axes
            if (!(collisionX && collisionY && collisionZ))
            {
                return false;
            }
            
            // Calculate the overlap on each axis
            var overlapX = halfSize1 + halfSize2 - Mathf.Abs(position1.x - position2.x);
            var overlapY = halfSize1 + halfSize2 - Mathf.Abs(position1.y - position2.y);
            var overlapZ = halfSize1 + halfSize2 - Mathf.Abs(position1.z - position2.z);
            
            // Find the minimum overlap axis to determine collision normal
            if (overlapX <= overlapY && overlapX <= overlapZ)
            {
                normal = new Vector3(position1.x > position2.x ? 1 : -1, 0, 0);
                resolvedPosition = new Vector3(
                    position2.x + (halfSize1 + halfSize2) * (position1.x > position2.x ? 1 : -1),
                    position1.y,
                    position1.z);
            }
            else if (overlapY <= overlapZ)
            {
                normal = new Vector3(0, position1.y > position2.y ? 1 : -1, 0);
                resolvedPosition = new Vector3(
                    position1.x,
                    position2.y + (halfSize1 + halfSize2) * (position1.y > position2.y ? 1 : -1),
                    position1.z);
            }
            else
            {
                normal = new Vector3(0, 0, position1.z > position2.z ? 1 : -1);
                resolvedPosition = new Vector3(
                    position1.x,
                    position1.y,
                    position2.z + (halfSize1 + halfSize2) * (position1.z > position2.z ? 1 : -1));
            }
            
            return true;
        }
        
        /// <summary>
        /// Reflect a velocity vector based on a collision normal
        /// </summary>
        public static Vector3 ReflectVelocity(Vector3 velocity, Vector3 normal)
        {
            return Vector3.Reflect(velocity, normal);
        }
        
        /// <summary>
        /// Draw boundaries as gizmos in the scene view
        /// </summary>
        public static void DrawBoundariesGizmo(Bounds bounds, Color color)
        {
            var prevColor = Gizmos.color;
            Gizmos.color = color;

            // Draw the wireframe cube for the boundaries
            Gizmos.DrawWireCube(bounds.center, bounds.size);
            
            Gizmos.color = prevColor;
        }
    }
}
