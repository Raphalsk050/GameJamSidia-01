using System.Collections;
using SidiaGameJam.Enums;
using SidiaGameJam.Events;
using SidiaGameJam.Components;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SidiaGameJam.Controller
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class CharacterController : Controller
    {
        [SerializeField] private SoEventRelated dodgeEventRelated;
        [SerializeField] private SoEventOneParamRelated<bool> dodgingBoolEvent;
        [SerializeField] private float dodgeDistance;
        [SerializeField] private float dodgeCooldown;
        [SerializeField] private float dodgeDuration;
        
        private bool _canDodge;
        private Vector2 _currentMovementInput;
        private float _dodgeCurrentCooldown;
        private GameObject _inventoryObj;
        private bool _inventoryOpen;
        private UIState _inventoryUIState = UIState.Closed;

        private MainInputAction _playerInput;
        public float WeaponHolderPositionOffset { get; } = 0.143f;
        public float CharacterHeight { get; } = 0.857f;

        protected override void InitializeVariables()
        {
            base.InitializeVariables();
            _playerInput = new MainInputAction();
            ContactFilter = new ContactFilter2D
            {
                layerMask = LayerMask.NameToLayer("Enemy")
            };
        }
        
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.DrawWireSphere(PointToAttack, attackRange);
        }

        //TODO: move the dodge code to be an ability instead of being an internal player controller implementation
        private void ControlDodge()
        {
            if (!_canDodge)
            {
                _dodgeCurrentCooldown += Time.deltaTime;
                if (_dodgeCurrentCooldown >= dodgeCooldown)
                {
                    _canDodge = true;
                    _dodgeCurrentCooldown = 0;
                }
            }
        }

        private void SetYVelocity()
        {
            //theoAnimationController.onYVelocityChange?.Invoke(_rigidbody.velocity.y);
        }

        public void BindInputKeys()
        {
            //player input actions
            _playerInput.Player.Move.started += MovementCallback;
            _playerInput.Player.Move.canceled += MovementCallback;
            _playerInput.Player.Move.performed += MovementCallback;
            _playerInput.Player.DodgeRoll.performed += _ => { DodgeRoll(); };
            _playerInput.Player.Interact.performed += _ => { Interact(); };
            _playerInput.Player.Jump.performed += _ => { Jump(); };
            _playerInput.Player.AbilityOne.performed += _ => { ExecuteAbilityOnSlotOne();};
            _playerInput.Player.Attack.started += _ => { Attack(); };
            _playerInput.Player.ChangeSelectedCat.performed += _ => { CatQueueComponent.ChangeToNextCatIndex(); };

            //ui input actions
            _playerInput.UI.Inventory.performed += _ => { OpenCloseInventory(); };
            //_playerInput.UI.InGameMenu.performed += _ => { };
        }

        public override void EnableInput()
        {
            _playerInput.Player.Enable();
            _playerInput.UI.Enable();
        }

        public override void DisableInput()
        {
            _playerInput.Player.Disable();
            _playerInput.UI.Disable();
        }

        private void MovementCallback(InputAction.CallbackContext context)
        {
            _currentMovementInput = context.ReadValue<Vector2>();
            Moving = _currentMovementInput.x != 0f || _currentMovementInput.y != 0f || !Grounded;
            Move(_currentMovementInput.x);
        }

        private void DodgeRoll()
        {
            if (!_canDodge) return;
            if (!Grounded) return;
            if (Coroutine != null)
                StopCoroutine(Coroutine);
            Coroutine = StartCoroutine(Dodge(dodgeDuration));
            _canDodge = false;
            DisableInput();
        }

        private IEnumerator Dodge(float duration)
        {
            if (!_canDodge) yield break;
            Rigidbody2D.velocity = Vector2.zero;
            Rigidbody2D.simulated = false;
            var playerPosition = transform.position;
            var positionToDodge = playerPosition + GetPlayerCharacterVector() * dodgeDistance;
            var ray = Physics2D.BoxCast(playerPosition, Vector2.one, 0, GetPlayerCharacterVector(), dodgeDistance,
                walkableLayers);

            dodgeEventRelated.InvokeAction(gameObject);
            dodgingBoolEvent.InvokeAction(true, gameObject);

            if (ray)
                positionToDodge =
                    new Vector3(
                        ray.point.x - gameObject.GetComponent<BoxCollider2D>().bounds.size.x *
                        GetPlayerCharacterVector().x, playerPosition.y, 0f);

            var startTime = Time.time;
            var endTime = startTime + duration;
            float t;
            //theoAnimationController.onDodgeChange?.Invoke(true);
            while (Time.time < endTime)
            {
                t = Mathf.InverseLerp(startTime, endTime, Time.time);

                transform.position = Vector2.Lerp(playerPosition, positionToDodge, t);
                yield return null;
            }

            dodgingBoolEvent.InvokeAction(false, gameObject);
            EnableInput();
            Rigidbody2D.simulated = true;
            //theoAnimationController.onDodgeChange?.Invoke(false);
            StopCoroutine(Coroutine);
        }

        public void ExecuteAbilityOnSlotOne()
        {
            var abilityComponent = GetComponent<AbilityComponent>();
            abilityComponent.ExecuteAbility(0);
        }

        #region MonoMethods

        protected override void Awake()
        {
            base.Awake();
            
            
        }

        private void OnEnable()
        {
            InitializeVariables();
            BindInputKeys();
        }

        private void OnDisable()
        {
            DisableInput();
        }

        private void Start()
        {
            //OpenCloseInventory();
        }

        protected override void Update()
        {
            base.Update();
            ControlDodge();
        }

        /*private void OnGUI()
        {
            if (!debug) return;
            GUILayout.TextArea($"Grounded: {Grounded}", guiStyle.style);
            GUILayout.TextArea($"Moving: {Moving}", guiStyle.style);
            GUILayout.TextArea($"Vertical Velocity: {Rigidbody2D.velocity.y}", guiStyle.style);
            GUILayout.TextArea($"Movement input: {_currentMovementInput}", guiStyle.style);
            GUILayout.TextArea($"Horizontal Velocity: {Rigidbody2D.velocity.x}", guiStyle.style);
            GUILayout.TextArea($"Player Position: {transform.position}", guiStyle.style);
            GUILayout.TextArea($"Ground Check Y Size: {GetCharacterGroundCheckSizeWithAGivenVerticalVelocity(Rigidbody2D.velocity.y, 0.5f)}", guiStyle.style);

            if(AbilityComponent.Abilities.Count <= 0) return;  
            foreach (var ability in AbilityComponent.Abilities)
            {
                GUILayout.TextArea($"Ability: {ability.abilityName}, State:{ability.state}", guiStyle.style);
            }
            
            if(AbilityComponent.RunningAbilities.Count <= 0) return; 
            foreach (var runningAbility in AbilityComponent.RunningAbilities)
            {
                GUILayout.TextArea($"Running Ability: {runningAbility.abilityName}", guiStyle.style);
            }
        }*/

        #endregion

        #region UI

        private void SwitchInputToGameAndUI()
        {
            _playerInput.Player.Enable();
            _playerInput.UI.Enable();
            Time.timeScale = 1f;
        }

        private void SwitchInputToUI()
        {
            _playerInput.Player.Disable();
            _playerInput.UI.Enable();
            Time.timeScale = 0f;
        }

        private void OpenCloseInventory()
        {
            switch (_inventoryUIState)
            {
                case UIState.Open:
                    CloseInventory();
                    break;
                case UIState.Closed:
                    OpenInventory();
                    break;
            }
        }

        private void OpenInventory()
        {
            CanvasGroup inventoryRender = _inventoryObj.GetComponent<CanvasGroup>();
            SwitchInputToUI();
            inventoryRender.alpha = 1;
            inventoryRender.blocksRaycasts = true;
            _inventoryUIState = UIState.Open;
        }

        private void CloseInventory()
        {
            CanvasGroup inventoryRender = _inventoryObj.GetComponent<CanvasGroup>();
            SwitchInputToGameAndUI();
            inventoryRender.alpha = 0;
            inventoryRender.blocksRaycasts = false;
            _inventoryUIState = UIState.Closed;
        }

        #endregion
    }
}