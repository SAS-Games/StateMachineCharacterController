using UnityEngine;

namespace SAS.StateMachineCharacterController
{
	[CreateAssetMenu(fileName = "AerialMovement", menuName = "SAS/State Machine Character Controller/Aerial Movement")]
	public class AerialMovementConfig : ScriptableObject
	{
		public float speed = 7;
		public float acceleration = 30;
		public float airResistance = 5f;
	}
}