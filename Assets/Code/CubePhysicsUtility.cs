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

            var collisionX = Mathf.Abs(position1.x - position2.x) <= (halfSize1 + halfSize2);
            var collisionY = Mathf.Abs(position1.y - position2.y) <= (halfSize1 + halfSize2);
            var collisionZ = Mathf.Abs(position1.z - position2.z) <= (halfSize1 + halfSize2);

            if (!(collisionX && collisionY && collisionZ))
            {
                return false;
            }

            var overlapX = halfSize1 + halfSize2 - Mathf.Abs(position1.x - position2.x);
            var overlapY = halfSize1 + halfSize2 - Mathf.Abs(position1.y - position2.y);
            var overlapZ = halfSize1 + halfSize2 - Mathf.Abs(position1.z - position2.z);

            resolvedPosition = position1; 

            if (overlapX <= overlapY && overlapX <= overlapZ)
            {
                ResolveAxisCollision(position1.x, position2.x, halfSize1, halfSize2, ref normal, ref resolvedPosition, Vector3.right, 0);
            }
            else if (overlapY <= overlapZ)
            {
                ResolveAxisCollision(position1.y, position2.y, halfSize1, halfSize2, ref normal, ref resolvedPosition, Vector3.up, 1);
            }
            else
            {
                ResolveAxisCollision(position1.z, position2.z, halfSize1, halfSize2, ref normal, ref resolvedPosition, Vector3.forward, 2);
            }

            return true;
        }

        /// <summary>
        /// Helper method to handle collision resolution for a single axis.
        /// </summary>
        private static void ResolveAxisCollision(
            float p1, float p2, float hs1, float hs2,
            ref Vector3 currentNormal, 
            ref Vector3 currentResolvedPos,
            Vector3 axisNormalDirection, 
            int axisIndex)
        {
            currentNormal = axisNormalDirection * (p1 > p2 ? 1 : -1);
            
            // Create a temporary resolved position to modify only the relevant axis
            var tempResolvedPos = currentResolvedPos;

            if (axisIndex == 0) // X-axis
            {
                tempResolvedPos.x = p2 + (hs1 + hs2) * currentNormal.x;
            }
            else if (axisIndex == 1) // Y-axis
            {
                tempResolvedPos.y = p2 + (hs1 + hs2) * currentNormal.y;
            }
            else // Z-axis
            {
                tempResolvedPos.z = p2 + (hs1 + hs2) * currentNormal.z;
            }
            currentResolvedPos = tempResolvedPos;
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
