using SAS.StateMachineGraph;
using System;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    [RequireComponent(typeof(Actor)), DisallowMultipleComponent]
    public class FSMCharacterController : MonoBehaviour
    {
       /* [NonSerialized]*/ public Vector3 movementVector;
       /* [NonSerialized]*/ public Vector3 movementInput;
        public float NormalizedMoveInput => movementInput.magnitude;
        
        private Actor _actor;

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
            Actor.SetFloat("NormalizedMoveInput", (float)Math.Round(normalizedMoveInput, 2));
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
