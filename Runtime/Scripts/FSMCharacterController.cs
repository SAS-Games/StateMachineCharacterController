using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using System;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    [RequireComponent(typeof(Actor)), DisallowMultipleComponent]
    public class FSMCharacterController : MonoBehaviour
    {
        [FieldRequiresSelf] private CharacterController _characterController;
        /* [NonSerialized]*/
        public Vector3 movementVector;
        /* [NonSerialized]*/
        public Vector3 movementInput;
        public float NormalizedMoveInput => movementInput.magnitude;

        private Actor _actor;
        private int NormalizedMoveInputHash = Animator.StringToHash("NormalizedMoveInput");
        public Vector3 VerticalVelocity => _characterController.velocity.Multiply(0.0f, 1.0f, 0.0f);

        private void Awake()
        {
            this.Initialize();
        }
        public Actor Actor
        {
            get
            {
                if (_actor == null)
                    _actor = GetComponent<Actor>();
                return _actor;
            }
        }

        private bool _isGrounded = true;
        public bool IsGrounded
        {
            get { return _isGrounded; }
            internal set
            {
                _isGrounded = value;
                Actor.SetBool("IsGrounded", _isGrounded);
            }
        }

        public void OnMove(float normalizedMoveInput)
        {
            Actor.SetFloat(NormalizedMoveInputHash, (float)Math.Round(normalizedMoveInput, 2));
        }

        public void OnJumpInitiated()
        {
            Actor.SetBool("Jump", true);
        }

        public void OnJumpCanceled()
        {
            Actor.SetBool("Jump", false);
        }
    }
}
