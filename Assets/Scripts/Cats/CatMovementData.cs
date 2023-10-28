using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJS.Data
{
    [CreateAssetMenu(menuName = "Create Cat Movement Data")]
    public class CatMovementData : ScriptableObject
    {
        [SerializeField] private float _walkVelocity;
        [SerializeField] private float _maxWalkVelocity;
        [SerializeField] private float _jumpForce;

        public float WalkVelocity => _walkVelocity;
        public float MaxWalkVelocity => _maxWalkVelocity;
        public float JumpForce => _jumpForce;
    }
}