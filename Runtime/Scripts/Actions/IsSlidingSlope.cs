using SAS.StateMachineGraph;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class IsSlidingSlope : ICustomCondition
    {
        private CharacterController _characterController;
        private FSMCharacterController _fsmCharacterController;
        private Actor _actor;
        private Transform _transform;

        void ICustomCondition.OnInitialize(Actor actor)
        {
            _actor = actor;
            actor.TryGetComponent(out _fsmCharacterController);
            actor.TryGetComponent(out _characterController);
            _transform = _characterController.transform;
        }

        void ICustomCondition.OnStateEnter()
        {
        }

        void ICustomCondition.OnStateExit()
        {
        }

        bool ICustomCondition.Evaluate()
        {
            int gridResolution = 3; // Number of rays in each direction (3x3 grid)
            float gridSpacing = 0.15f; // Distance between rays in the grid
            float rayLength = _characterController.skinWidth;

            Vector3 origin = _transform.position;
            Vector3 downwardDirection = Vector3.down;

            Vector3 averageNormal = Vector3.zero;
            int validHitCount = 0;

            // Loop through the grid
            for (int x = -gridResolution / 2; x <= gridResolution / 2; x++)
            {
                for (int z = -gridResolution / 2; z <= gridResolution / 2; z++)
                {
                    Vector3 offset = new Vector3(x * gridSpacing, 0, z * gridSpacing);
                    Vector3 rayOrigin = origin + offset;

                    if (Physics.Raycast(rayOrigin, downwardDirection, out RaycastHit hit, rayLength))
                    {
                        float angle = Vector3.Angle(Vector3.up, hit.normal);

                        // Ignore steep normals (e.g., risers)
                        if (angle < 85.0f) // Example threshold for vertical surfaces
                        {
                            averageNormal += hit.normal;
                            validHitCount++;

                            // Debug: Draw individual rays
                            Debug.DrawRay(rayOrigin, downwardDirection * rayLength, Color.yellow);
                        }
                        else
                        {
                            // Debug: Indicate ignored hits
                            Debug.DrawRay(rayOrigin, downwardDirection * rayLength, Color.red);
                        }
                    }
                }
            }

            // Calculate the average normal
            if (validHitCount > 0)
            {
                averageNormal.Normalize(); // Normalize to get the final averaged normal
                float slopeAngle = Vector3.Angle(Vector3.up, averageNormal);

                // Debug: Draw the averaged normal
                Debug.DrawRay(origin, averageNormal, slopeAngle >= _characterController.slopeLimit ? Color.green : Color.blue);

                // Return true if the slope is too steep
                return slopeAngle >= _characterController.slopeLimit;
            }

            // No valid hits detected
            return false;
        }
    }
}
