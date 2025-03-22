using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    [CreateAssetMenu(menuName = "SAS/State Machine Character Controller/Dash Movement Config")]
    public class DashMovementConfig : ScriptableObject
    {
        public float horizontalSpeed = 20;
        public float verticalSpeed = 20;
        public bool forwardDirection = false;
    }
}
