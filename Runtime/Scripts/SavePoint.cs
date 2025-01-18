using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class SavePoint : MonoBehaviour
    {
        [SerializeField] private bool m_Save = false;
        public static bool hasSaved = false;
        private const string PositionTag = "SavePosition";
        private const string RotationTag = "SaveRotation";
        public static Vector3 Position
        {
            get => PlayerPrefsExt.GetVector3(PositionTag, Vector3.zero);
            private set => PlayerPrefsExt.SetVector3(PositionTag, value);
        }

        public static Quaternion Rotation
        {
            get => PlayerPrefsExt.GetQuaternion(RotationTag, Quaternion.identity);
            private set => PlayerPrefsExt.SetQuaternion(RotationTag, value);
        }

        public static bool HasSavedPoint => PlayerPrefsExt.HasVector3(PositionTag);

        private void SaveLastPoint(Collider other)
        {
            if (m_Save)
            {
                Position = transform.position;
                Rotation = transform.rotation;
            }

            Debug.Log("Save point activated at: " + transform, PositionTag);
        }
    }
}
