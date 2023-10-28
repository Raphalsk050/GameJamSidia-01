using System;
using System.Collections;
using System.Collections.Generic;
using GJS.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GJS.Core
{
    public class Movement : MonoBehaviour
    {

        #region Cat Variables
        [SerializeField] private CatMovementData _movementData;

        private Rigidbody2D _rb2D;
        private Vector3 _forceDirection;
        private bool _isGrounded;
        private const string GroundTag = "Ground";
        #endregion
        
        #region Input System Variables
        [SerializeField] private InputActionAsset _actionAsset;
        private InputActionMap _inputActionMap;
        private InputAction _jumpAction;
        private InputAction _movementAction;
        #endregion

        #region Monobehaviour Events

        private void Awake()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _inputActionMap = _actionAsset.FindActionMap("Cats");
            _movementAction = _inputActionMap.FindAction("Movement");
            _inputActionMap.FindAction("Jump").performed += OnJump;
        }
        
        private void OnEnable()
        {
            _inputActionMap.Enable();
        }

        private void OnDisable()
        {
            _inputActionMap.Disable();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag(GroundTag))
            {
                _isGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(GroundTag))
            {
                _isGrounded = false;
            }
        }

        private void Update()
        {
            _forceDirection += transform.right * (_movementAction.ReadValue<float>() * _movementData.WalkVelocity);
            _rb2D.AddForce(_forceDirection, ForceMode2D.Impulse);
            _forceDirection = Vector3.zero;
            var velocity = _rb2D.velocity;
            velocity.y = 0;
            if (velocity.sqrMagnitude > _movementData.MaxWalkVelocity * _movementData.MaxWalkVelocity )
            {
                _rb2D.velocity = velocity.normalized * _movementData.MaxWalkVelocity  + Vector2.up * _rb2D.velocity.y;
            }
        }

        #endregion
        
        private void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && _isGrounded)
            {
                _rb2D.AddForce(Vector2.up * _movementData.JumpForce , ForceMode2D.Impulse);
            }
        }
    }
}