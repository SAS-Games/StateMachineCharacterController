using UnityEngine;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class CharacterControllerColliderHit : MonoBase
    {
        [HideInInspector, FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        public ControllerColliderHit LastHit { get; private set; }
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            LastHit = hit;

            if (!_fsmCharacterController.IsGrounded || LastHit == null)
                return;

            Debug.DrawRay(hit.point, hit.normal, Color.blue);

           // _canWallJump = true;
           // _wallJumpNormal = hit.normal;
        }
    }
}
