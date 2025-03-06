using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class IsAtClimbableEdge : ICustomCondition
    {
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        [SerializeField] private float _edgeCheckDistance = 1f;
        [SerializeField] private float _clearanceHeight = 2f;
        [SerializeField] private LayerMask _groundLayer;
        private Transform _orientation;
        private Collider  _characterCollider;

        void ICustomCondition.OnInitialize(Actor actor)
        {
            actor.Initialize(this);
            _orientation = _fsmCharacterController.transform;
            _characterCollider = _fsmCharacterController.GetComponent<Collider>();
           
        }

        bool ICustomCondition.Evaluate()
        {            
            // Calculate edge check origin and direction
            Vector3 edgeCheckOrigin = _fsmCharacterController.transform.position + Vector3.up * _characterCollider.bounds.extents.y;
            Vector3 checkDirection = _orientation.forward;
            float checkDistance = _edgeCheckDistance;

            // Check for wall ending
            bool wallEnds = !Physics.Raycast(edgeCheckOrigin, checkDirection, checkDistance, _fsmCharacterController.ClimbableLayer);

            if (!wallEnds)
                return false;

            // Check if there's enough space to stand
            Vector3 checkPosition = _fsmCharacterController.transform.position + checkDirection * checkDistance + Vector3.up * 0.1f;

            Vector3 boxHalfExtents = new Vector3(_characterCollider.bounds.extents.x, _clearanceHeight * 0.5f, _characterCollider.bounds.extents.z);
            bool hasClearance = !Physics.CheckBox(checkPosition + Vector3.up * _clearanceHeight * 0.5f, boxHalfExtents, Quaternion.identity, _groundLayer);

            return hasClearance;
        }



        void ICustomCondition.OnStateEnter()
        {
        }

        void ICustomCondition.OnStateExit()
        {
        }
    }
}