using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public static class Util
    {
        public static Vector3 SetX(this Vector3 vec, float x)
        {
            return new Vector3(x, vec.y, vec.z);
        }

        public static Vector3 SetY(this Vector3 vec, float y)
        {
            return new Vector3(vec.x, y, vec.z);
        }

        public static Vector3 SetZ(this Vector3 vec, float z)
        {
            return new Vector3(vec.x, vec.y, z);
        }

        public static Vector3 Multiply(this Vector3 vec, float x, float y, float z)
        {
            return new Vector3(vec.x * x, vec.y * y, vec.z * z);
        }

        public static Vector3 Multiply(this Vector3 vec, Vector3 other)
        {
            return Multiply(vec, other.x, other.y, other.z);
        }

        public static Vector3 Clamp(this Vector3 vec, Vector3 min, Vector3 max)
        {
            vec.x = Mathf.Clamp(vec.x, min.x, max.x);
            vec.y = Mathf.Clamp(vec.y, min.y, max.y);
            vec.z = Mathf.Clamp(vec.z, min.z, max.z);

            return vec;
        }

        public static float Remap(this float f, float fromMin, float fromMax, float toMin, float toMax)
        {
            float t = (f - fromMin) / (fromMax - fromMin);
            return Mathf.LerpUnclamped(toMin, toMax, t);
        }

        public static void SetXLocalPosition(this Transform transform, float xValue)
        {
            Vector3 position = transform.localPosition;
            position.x = xValue;
            transform.localPosition = position;
        }

        // Set the Y value of the position
        public static void SetYLocalPosition(this Transform transform, float yValue)
        {
            Vector3 position = transform.localPosition;
            position.y = yValue;
            transform.localPosition = position;
        }

        // Set the Z value of the position
        public static void SetZLocalPosition(this Transform transform, float zValue)
        {
            Vector3 position = transform.localPosition;
            position.z = zValue;
            transform.localPosition = position;
        }
    }

    public static class PlayerPrefsExt
    {
        // Save a Vector3 to PlayerPrefs
        public static void SetVector3(string key, Vector3 value)
        {
            PlayerPrefs.SetFloat(key + "_x", value.x);
            PlayerPrefs.SetFloat(key + "_y", value.y);
            PlayerPrefs.SetFloat(key + "_z", value.z);
            PlayerPrefs.Save();
        }

        // Retrieve a Vector3 from PlayerPrefs
        public static Vector3 GetVector3(string key, Vector3 defaultValue = default(Vector3))
        {
            if (PlayerPrefs.HasKey(key + "_x") &&
                PlayerPrefs.HasKey(key + "_y") &&
                PlayerPrefs.HasKey(key + "_z"))
            {
                float x = PlayerPrefs.GetFloat(key + "_x");
                float y = PlayerPrefs.GetFloat(key + "_y");
                float z = PlayerPrefs.GetFloat(key + "_z");
                return new Vector3(x, y, z);
            }

            return defaultValue; // Return default value if keys do not exist
        }

        public static bool HasVector3(string key)
        {
            return PlayerPrefs.HasKey(key + "_x") &&
                   PlayerPrefs.HasKey(key + "_y") &&
                   PlayerPrefs.HasKey(key + "_z");
        }

        public static void DeleteVector3(string key)
        {
            PlayerPrefs.DeleteKey(key + "_x");
            PlayerPrefs.DeleteKey(key + "_y");
            PlayerPrefs.DeleteKey(key + "_z");
            PlayerPrefs.Save();
        }


        public static void SetQuaternion(string key, Quaternion value)
        {
            PlayerPrefs.SetFloat(key + "_x", value.x);
            PlayerPrefs.SetFloat(key + "_y", value.y);
            PlayerPrefs.SetFloat(key + "_z", value.z);
            PlayerPrefs.SetFloat(key + "_w", value.w);
            PlayerPrefs.Save();
        }

        // Retrieve a Vector3 from PlayerPrefs
        public static Quaternion GetQuaternion(string key, Quaternion defaultValue = default(Quaternion))
        {
            if (PlayerPrefs.HasKey(key + "_x") &&
                PlayerPrefs.HasKey(key + "_y") &&
                PlayerPrefs.HasKey(key + "_z") &&
                    PlayerPrefs.HasKey(key + "_w"))
            {
                float x = PlayerPrefs.GetFloat(key + "_x");
                float y = PlayerPrefs.GetFloat(key + "_y");
                float z = PlayerPrefs.GetFloat(key + "_z");
                float w = PlayerPrefs.GetFloat(key + "_w");
                return new Quaternion(x, y, z, w);
            }

            return defaultValue; // Return default value if keys do not exist
        }

        public static bool HasQuaternion(string key)
        {
            return PlayerPrefs.HasKey(key + "_x") &&
                   PlayerPrefs.HasKey(key + "_y") &&
                   PlayerPrefs.HasKey(key + "_z") &&
                   PlayerPrefs.HasKey(key + "_w");
        }

        public static void DeleteQuaternion(string key)
        {
            PlayerPrefs.DeleteKey(key + "_x");
            PlayerPrefs.DeleteKey(key + "_y");
            PlayerPrefs.DeleteKey(key + "_z");
            PlayerPrefs.DeleteKey(key + "_w");
            PlayerPrefs.Save();
        }
    }
}
