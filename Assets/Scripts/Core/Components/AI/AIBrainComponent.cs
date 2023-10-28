using System;
using UnityEngine;
using PawnController = SidiaGameJam.Controller.Controller;

namespace SidiaGameJam.Components
{
    [RequireComponent(typeof(Controller.Controller))]
    public class AIBrainComponent: ComponentBase
    {
        public Transform target;
        public bool canAttack = true;
        public float distanceTolerance = 1f;
        private Controller.Controller _pawnController;
        private Vector3 _targetPosition;
        private Vector3 _originalTargetPosition;
        private Vector2 _directionMovement;
        
        public override void Initialize()
        {
            base.Initialize();
            FindTarget();
            _pawnController = GetComponent<Controller.Controller>();
        }

        public void Attack()
        {
            if (!canAttack) return;
            if (Vector3.Distance(_targetPosition, transform.position) > distanceTolerance)return;
            
            _pawnController.Attack();
        }
        
        private void FixedUpdate()
        {
            AIMovement();
            Attack();
            VerifyJump();
        }

        public void AIMovement()
        {
            if (target == null)
            {
                FindTarget();
                return;
            }
            
            GetDestination(target);
            if (Vector3.Distance(_targetPosition, transform.position) > distanceTolerance)
                Follow();
            else
            {
                _directionMovement = Vector2.zero;
                Follow();
            }
        }

        public void FindTarget()
        {
            if (!GameObject.FindWithTag("Player")) return;
            
            target = GameObject.FindWithTag("Player").transform;
        }

        public void VerifyJump()
        {
            var deltaHeight = _originalTargetPosition.y - transform.position.y;
            if (Mathf.Abs(_pawnController.CurrentHorizontalVelocity) < 0.05f && deltaHeight > 1.2f)
            {
                _pawnController.Jump();
            }
        }

        public void GetDestination(Transform target)
        {
            _originalTargetPosition = target.position;
            _targetPosition = new Vector3(_originalTargetPosition.x, transform.position.y,transform.position.z);
            var movementDirection = _targetPosition - transform.position;
            _directionMovement = movementDirection.normalized;
        }
        
        public void Follow()
        {
            _pawnController.Move(_directionMovement.x);
        }
    }
}