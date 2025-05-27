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
        /// Handle collisions with 3D boundaries and update velocity
        /// </summary>
        public static Vector3 HandleBoundaryCollisions(Vector3 position, float halfSize, Bounds boundaries, ref Vector3 velocity)
        {
            // Handle X boundaries
            if (position.x - halfSize < boundaries.min.x)
            {
                position.x = boundaries.min.x + halfSize;
                velocity.x = -velocity.x;
            }
            else if (position.x + halfSize > boundaries.max.x)
            {
                position.x = boundaries.max.x - halfSize;
                velocity.x = -velocity.x;
            }
            
            // Handle Y boundaries
            if (position.y - halfSize < boundaries.min.y)
            {
                position.y = boundaries.min.y + halfSize;
                velocity.y = -velocity.y;
            }
            else if (position.y + halfSize > boundaries.max.y)
            {
                position.y = boundaries.max.y - halfSize;
                velocity.y = -velocity.y;
            }
            
            // Handle Z boundaries
            if (position.z - halfSize < boundaries.min.z)
            {
                position.z = boundaries.min.z + halfSize;
                velocity.z = -velocity.z;
            }
            else if (position.z + halfSize > boundaries.max.z)
            {
                position.z = boundaries.max.z - halfSize;
                velocity.z = -velocity.z;
            }
            
            return position;
        }
        
        /// <summary>
        /// Legacy method for 2D boundaries (Rect) to maintain backward compatibility
        /// </summary>
        public static Vector3 HandleBoundaryCollisions(Vector3 position, float halfSize, Rect boundaries, ref Vector3 velocity)
        {
            // Create a 3D bounds from the 2D rect (with Y bounds from -10 to 10)
            var bounds = new Bounds(
                new Vector3((boundaries.xMin + boundaries.xMax) * 0.5f, 0, (boundaries.yMin + boundaries.yMax) * 0.5f),
                new Vector3(boundaries.width, 20, boundaries.height)
            );
            
            return HandleBoundaryCollisions(position, halfSize, bounds, ref velocity);
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
            
            // Calculate distance between cube centers
            var distance = Vector3.Distance(position1, position2);
            
            var minDistance = halfSize1 + halfSize2;
            
            // No collision if distance is greater than or equal to minimum distance
            if (distance >= minDistance)
            {
                return false;
            }
            
            // Calculate normal vector (direction from other cube to this cube)
            normal = (position1 - position2).normalized;
                
            // Adjust position to prevent overlap
            resolvedPosition = position2 + normal * minDistance;
                
            return true;
        }
        
        /// <summary>
        /// Reflect a velocity vector based on a collision normal
        /// </summary>
        public static Vector3 ReflectVelocity(Vector3 velocity, Vector3 normal)
        {
            return Vector3.Reflect(velocity, normal);
        }
    }
}
