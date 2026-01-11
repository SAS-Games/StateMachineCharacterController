using SAS.Core;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    [CreateAssetMenu(menuName = "SAS/State Machine Character Controller/Downward Movement Config")]
    public class DownwardMovementConfig : ScriptableObject
    {
        public float gravityMultiplier = 5;
        public floatRange fallSpeedRange  = new floatRange(-50, 100);
    }
}
