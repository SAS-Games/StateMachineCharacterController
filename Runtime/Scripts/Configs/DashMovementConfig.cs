using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    [CreateAssetMenu(menuName = "SAS/State Machine Character Controller/Dash Movement Config")]
    public class DashMovementConfig : ScriptableObject
    {
        public float horizontalSpeed = 6;
        public float verticalSpeed = 6;
        public float gravityMultiplier = 5;
        public float gravityComebackMultiplier = 0.03f;
        public float gravityDivider = 0.6f;
    }
}
