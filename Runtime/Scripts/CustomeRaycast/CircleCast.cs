using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    [CreateAssetMenu(menuName = "SAS/State Machine Character Controller/CustomRaycast")]
    public class CircleCast : CustomRaycast
    {
        [SerializeField] private float m_Radius = 0.25f;
        [SerializeField] private int m_MaxRayCount = 5;

        public sealed override bool Raycast(Vector3 origin, Vector3 direction, float hitDistance)
        {
#if UNITY_EDITOR
            for (int i = 0; i < m_MaxRayCount; ++i)
            {
                var position = PointOnTheCircle(m_Radius, m_MaxRayCount, origin, i);
                bool result = Physics.Raycast(position, direction, out var hit, hitDistance + m_HitDistanceOffset, m_GroundLayer);
                Debug.DrawRay(position, direction * (hitDistance + m_HitDistanceOffset), hit.collider != null ? Color.green : Color.red);
            }
#endif
            for (int i = 0; i < m_MaxRayCount; ++i)
            {
                var position = PointOnTheCircle(m_Radius, m_MaxRayCount, origin, i);
                if (Physics.Raycast(position, direction, out var hit, hitDistance + m_HitDistanceOffset, m_GroundLayer))
                    return true;
            }

            return false;
        }

        public sealed override bool Raycast(Vector3 origin, float hitDistance)
        {
            return Raycast(origin, m_Direction, hitDistance);
        }

        private Vector3 PointOnTheCircle(float radius, int numberOfPoints, Vector3 centre, int currentointIndex)
        {
            if (numberOfPoints == 1)
                return centre;

            float angle = currentointIndex * (6.28318f / numberOfPoints);
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            return new Vector3(centre.x + x, centre.y, centre.z + z);
        }
    }
}
