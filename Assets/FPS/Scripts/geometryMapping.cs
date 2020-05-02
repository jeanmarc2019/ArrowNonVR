using System.Collections.Generic;
using System;
using UnityEngine;

namespace GeometryMapper
{
    class PhysicsHelper
    {
        public static string currentGeometry = ""; // default is Euclidean geometry

        public static void changeGeometry(String geometry)
        {
            currentGeometry = geometry;
        }

        // maps position in terms of the geometry geodesics and returns a tuple of the position and direction
        public static (Vector3 mappedPosition, Vector3 mappedDirection) mapper(Vector3 position, Vector3 direction, float timeSinceRelease)
        {
            Vector3 newPosition; // new position in the update
            Vector3 newDirection; // new direction to point the arrow
            switch(currentGeometry) {
                case "Nil":
                    newDirection = ArrowFromOrigin(
                        BringVelocityToOrigin(
                            direction,
                            position
                        ),
                        timeSinceRelease
                    );
                    newPosition = NilMultiply(
                        position,
                        newDirection
                    );
                    break;
                default: // Euclidean
                    newPosition = position + direction * timeSinceRelease;
                    newDirection = direction;
                    break;
            }
            return (newPosition, newDirection);
        }

        // helper functions for Nil
        //----------------------------------------------------------------
        private static Vector3 NilMultiply(Vector3 left, Vector3 right)
        {
            Vector3 normalsum = new Vector3(
                left.x + right.x,
                left.y + right.y - (left.x * right.z - left.z * right.x),
                left.z + right.z
            );
            return normalsum;
        }

        // ensures path consistency no matter where the arrow is shot from
        // takes advantage of the fact that the group law is a linear transformation
        private static Vector3 BringVelocityToOrigin(Vector3 direction, Vector3 location)
        {
            return NilMultiply(-1*location, location + direction);
        }

        // Note: direction here works because by default it is a vector3 with magnitude 1
        // Would need to generalize this for non-normalized vector3
        private static Vector3 ArrowFromOrigin(Vector3 direction, float t)
        {
            Vector3 r = new Vector3(
                direction.x,
                0,
                direction.z
            );
            float gamma = direction.y;
            float varphi = Mathf.Atan2(direction.x,direction.z);
            float x = (r.magnitude/(2*gamma))*(Mathf.Sin(2*gamma*t + varphi) - Mathf.Sin(varphi));
            float y = ((1 + Mathf.Pow(gamma, 2f))/(2*gamma))*t - ((1 - Mathf.Pow(gamma, 2f))/(4*Mathf.Pow(gamma, 2f)))*Mathf.Sin(2*gamma*t);
            float z = (r.magnitude/(2*gamma))*(Mathf.Cos(varphi) - Mathf.Cos(2*gamma*t + varphi));
            Vector3 mapping = new Vector3(x,y,z);
            return mapping;
        }
    }
}