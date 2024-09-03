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
    }
}