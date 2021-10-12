using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public abstract class CustomRaycast : ScriptableObject
    {
        [SerializeField] protected LayerMask m_GroundLayer;
        [SerializeField] protected Vector3 m_Direction;
        [SerializeField] protected float m_HitDistanceOffset = 0.05f;

        public abstract bool Raycast(Vector3 origin, Vector3 direction, float hitDistance);
        public abstract bool Raycast(Vector3 origin, float hitDistance);
    }
}
