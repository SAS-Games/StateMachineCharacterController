using UnityEngine;
using SAS.TagSystem;
using SAS.Utilities;

namespace SAS.StateMachineCharacterController
{
    public class CharacterControllerColliderHit : MonoBase
    {
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        public ControllerColliderHit LastHit { get; private set; }
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            LastHit = hit;

            if (!_fsmCharacterController.IsGrounded)
                return;

            Debug.DrawRay(hit.point, hit.normal, Color.blue);

           // _canWallJump = true;
           // _wallJumpNormal = hit.normal;
        }
    }
}
