using UnityEngine;

namespace Core.Utils.Extended
{
    public static class Vector3Extension
    {
        public static Vector3 SetX(this Vector3 value, float x)
        {
            return new Vector3(x, value.y, value.z);
        }
        
        public static Vector3 SetY(this Vector3 value, float y)
        {
            return new Vector3(value.x, y, value.z);
        }
        
        public static Vector3 SetZ(this Vector3 value, float z)
        {
            return new Vector3(value.x, value.y, z);
        }
        
        public static Vector3 Divide(this Vector3 value, Vector3 divider)
        {
            return new Vector3(value.x / divider.x, value.y / divider.y, value.z / divider.z);
        }


        public static Vector3 ScaleProportionallyTo(this Vector3 value, Vector3 relative, float baseValue = 1)
        {
            var invBaseValue = 1f / baseValue;
            return new Vector3(
                value.x * relative.x * invBaseValue,
                value.y * relative.y * invBaseValue,
                value.z * relative.z * invBaseValue);
        }
    }
}