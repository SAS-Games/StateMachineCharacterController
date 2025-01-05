using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class SavePoint : MonoBehaviour
    {
        [SerializeField] private bool m_Save = false;
        public static Vector3 CurretSpawnPoint; // Static to persist between sessions
        public static bool hasSaved = false;
        private const string Tag = "SavePoint";
        public static Vector3 SavedPoint
        {
            get => PlayerPrefsExt.GetVector3(Tag, Vector3.zero);
            private set => PlayerPrefsExt.SetVector3(Tag, value);
        }

        public static bool HasSavedPoint => PlayerPrefsExt.HasVector3(Tag);

        private void SaveLastPoint(Collider other)
        {
            CurretSpawnPoint = transform.position;
            if (m_Save)
                SavedPoint = CurretSpawnPoint;

            Debug.Log("Save point activated at: " + CurretSpawnPoint, Tag);
        }
    }
}
