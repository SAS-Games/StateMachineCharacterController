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
            get => FlexPrefs.Get<Vector3>(PositionTag, Vector3.zero);
            private set => FlexPrefs.Set<Vector3>(PositionTag, value);
        }

        public static Quaternion Rotation
        {
            get => FlexPrefs.Get<Quaternion>(RotationTag, Quaternion.identity);
            private set => FlexPrefs.Set<Quaternion>(RotationTag, value);
        }

        public static bool HasSavedPoint => FlexPrefs.HasKey(PositionTag);

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
