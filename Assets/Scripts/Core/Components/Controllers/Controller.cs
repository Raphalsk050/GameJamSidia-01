using System.Collections;
using System.Collections.Generic;
using SidiaGameJam.Enums;
using SidiaGameJam.Events;
using SidiaGameJam.Components;
using SidiaGameJam.Core.Items;
using SidiaGameJam.DebugFunctions;
using UnityEngine;

namespace SidiaGameJam.Controller
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class Controller : ComponentBase
    {
        [SerializeField] protected float distanceOfBoxCast;
        [SerializeField] protected LayerMask walkableLayers;
        [SerializeField] protected Color debugColor;
        [SerializeField] protected Vector2 groundCheckBoxSize;
        [SerializeField] protected float maxCheckGroundFallSize = 0.5f;
        [SerializeField] protected Vector3 groundCheckerOffset;
        [SerializeField] protected bool debug = true;
        [SerializeField] protected float attackRange;
        [SerializeField] protected SoEventRelated attackEventRelated;
        [SerializeField] protected float jumpForce = 50f;
        [SerializeField] protected SoEventRelated jumpEventRelated;
        [SerializeField] protected SoEventOneParamRelated<bool> fallingBoolEvent;
        [SerializeField] protected SoEventOneParamRelated<bool> groundedBoolEvent;
        [SerializeField] protected SoEventOneParamRelated<float> movementFloatEvent;
        [SerializeField] protected LayerMask interactionLayer;
        [SerializeField] protected float interactionRadius = 0.6f;
        [SerializeField] protected float interactionDistance = 1.2f;

        
        protected ContactFilter2D ContactFilter;
        protected AbilityComponent AbilityComponent;
        protected float InputVelocity;
        protected List<Collider2D> Colliders;
        protected float MovementInputVelocity;
        protected Coroutine Coroutine;
        protected Vector2 PointToAttack;
        protected InputState InputState;
        protected float AttackCooldown = 0.5f;
        protected float CurrentAttackCooldown = 0;
        protected bool CanAttack = true;


        protected DebugDrawGizmos DebugDraw;
        protected bool Falling;
        protected Vector2 FloorNormalVector;
        protected Vector2 GroundCheckBoxSizeWithVelocity;
        protected bool Grounded;
        protected bool Jumping;
        protected bool LastGroundState;
        protected bool Moving;
        protected Vector3 PositionToCheckGround;
        protected Rigidbody2D Rigidbody2D;

        protected float Speed;
        protected SpriteRenderer SpriteRenderer;
        protected float SurfaceAngle;
        private float _currentHorizontalVelocity;

        public float CurrentHorizontalVelocity => _currentHorizontalVelocity;
        
        public float MovementSpeed => Speed;

        public PlayerAnimationController AnimationController => GetComponentInChildren<PlayerAnimationController>();

        protected override void Awake()
        {
            base.Awake();
            InitializeVariables();
            ConfigureRigidBodyComponent();
            if (!GetOwner().GetComponent<CharacterBase>())
                return;
            Speed = GetOwner().GetComponent<CharacterBase>().characterData.WalkSpeed;
        }
        public virtual void EnableInput()
        {
            Speed = GetOwner().GetComponent<CharacterBase>().characterData.WalkSpeed;
            InputState = InputState.Enabled;
        }

        public virtual void DisableInput()
        {
            Speed = 0;
            InputState = InputState.Disabled;
        }
        
        protected virtual void Update()
        {
            CheckIfCharacterIsGrounded();
            FallCheck();
            DetermineInputVelocity();
            _currentHorizontalVelocity = Rigidbody2D.velocity.x;
        }

        private void DetermineInputVelocity()
        {
            InputVelocity = MovementInputVelocity * Speed;
        }
        
        protected virtual void FixedUpdate()
        {
            SetCharacterRigidbodyVelocity(InputVelocity);
        }
        
        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = debugColor;

            if (!debug || !DebugDraw) return;
            
            DebugDraw.Position = PositionToCheckGround;
            DebugDraw.Shape = EDebugShape.Cube;
            DebugDraw.DebugColor = debugColor;
            DebugDraw.Size = GroundCheckBoxSizeWithVelocity;

            DebugDraw.Position = transform.position + Vector3.up * 0.001f * interactionDistance;
            DebugDraw.Shape = EDebugShape.Sphere;
            DebugDraw.DebugColor = new Color(0.97f, 0f, 1f, 0.38f);
            DebugDraw.Size = Vector3.one * interactionRadius;
            DebugDraw.DrawGizmos();
        }

        protected virtual void SetCharacterRigidbodyVelocity(float value)
        {
            Rigidbody2D.velocity = new Vector2(value, Rigidbody2D.velocity.y);
        }

        protected virtual void FallCheck()
        {
            if (Rigidbody2D.velocity.y < -0.2f && !Falling && !Grounded)
            {
                fallingBoolEvent.InvokeAction(true, gameObject);
                Falling = true;
            }
            else if (Rigidbody2D.velocity.y >= -0.1f || Grounded)
            {
                fallingBoolEvent.InvokeAction(false, gameObject);
                Falling = false;
            }
        }

        protected virtual void ConfigureRigidBodyComponent()
        {
            if (Rigidbody2D)
            {
                Rigidbody2D.simulated = true;
                Rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                Rigidbody2D.interpolation = RigidbodyInterpolation2D.Extrapolate;
                Rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;
            }
        }

        protected virtual void InitializeVariables()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            Colliders = new List<Collider2D>();
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            AbilityComponent = GetComponent<AbilityComponent>();
            DebugDraw = GetComponent<DebugDrawGizmos>();
        }

        public virtual Vector3 GetPlayerCharacterVector()
        {
            var lookDirection = SpriteRenderer.flipX ? -1 : 1;
            var forwardVector = transform.right * lookDirection;

            return forwardVector;
        }

        public virtual void Attack()
        {
            /*if (_weaponHolder.WeaponsAmount() <= 0) return;
            if(!_weaponHolder.GetSelectedWeapon().CanAttack) return;*/
            if (InputState != InputState.Enabled) return;
            if (!CanAttack) return;

            StartCoroutine(AttackTimer(AttackCooldown));
            
            var playerPosition = transform.position;
            var lookDirection = SpriteRenderer.flipX ? -1 : 1;
            var forwardVector = transform.right * lookDirection;
            PointToAttack = playerPosition + forwardVector*1.7f;
            
            Physics2D.OverlapCircle(PointToAttack, attackRange,ContactFilter,Colliders);
            
            //_weaponHolder.GetSelectedWeapon().onAttack?.Invoke();
            
            attackEventRelated.InvokeAction(gameObject);
            
            if(Colliders.Count == 0)return;
            
            foreach (var col in Colliders)
            {
                if (col.GetComponent<LifeComponentBase>())
                {
                    var enemy = col.gameObject;
                    var lifeComponent = enemy.GetComponent<LifeComponentBase>();
                    lifeComponent.ReceiveDamage(10f);
                }
            }
        }
        
        protected void Interact()
        {
            var rayCastHit =
                Physics2D.CircleCastAll(transform.position, interactionRadius, Vector2.up * 0.001f, interactionDistance, interactionLayer);
            if (rayCastHit.Length == 0)
                return;

            foreach (var ray in rayCastHit)
                if (ray.transform.GetComponent<IInteractable>() != null)
                {
                    ray.transform.GetComponent<IInteractable>().Interact(gameObject);
                }
        }

        private IEnumerator AttackTimer(float timeToWait)
        {
            CanAttack = false;
            yield return new WaitForSeconds(timeToWait);
            CanAttack = true;
        }
        
        public virtual void Jump()
        {
            if (InputState != InputState.Enabled) return;
            
            if (Grounded)
            {
                Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, 0f);
                Rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jumpEventRelated.InvokeAction(transform.root.gameObject);
            }
        }

        protected float GetCharacterGroundCheckSizeWithAGivenVerticalVelocity(float verticalVelocity,
            float maxValueToInterpolate)
        {
            if (verticalVelocity < -0.2f)
                return Mathf.Abs(Mathf.Lerp(0, maxValueToInterpolate, Mathf.Abs(verticalVelocity)));

            return 0f;
        }

        protected virtual void CheckIfCharacterIsGrounded()
        {
            var groundCheckNewOffset = new Vector3(groundCheckerOffset.x,
                -GetCharacterGroundCheckSizeWithAGivenVerticalVelocity(Rigidbody2D.velocity.y, maxCheckGroundFallSize) +
                groundCheckerOffset.y, 0f);

            PositionToCheckGround = transform.position + Vector3.down * 0.5f + groundCheckNewOffset *
                (0.5f + (maxCheckGroundFallSize + groundCheckBoxSize.y / 2) * 4);

            GroundCheckBoxSizeWithVelocity = groundCheckBoxSize + new Vector2(groundCheckBoxSize.x,
                groundCheckBoxSize.y +
                GetCharacterGroundCheckSizeWithAGivenVerticalVelocity(Rigidbody2D.velocity.y, 0.5f));

            Grounded = Physics2D.BoxCast(PositionToCheckGround, GroundCheckBoxSizeWithVelocity, 0f, Vector2.down,
                distanceOfBoxCast, walkableLayers);

            if (Grounded != LastGroundState)
            {
                groundedBoolEvent.InvokeAction(Grounded, gameObject);
                LastGroundState = Grounded;
            }
        }

        public virtual void Move(float value)
        {
            if (InputState != InputState.Enabled) return;
            
            movementFloatEvent.InvokeAction(Mathf.Abs(value), gameObject);
            MovementInputVelocity = value;
            if (Mathf.Abs(value) > 0.05f) SpriteRenderer.flipX = !(value > 0f);
        }
    }
}