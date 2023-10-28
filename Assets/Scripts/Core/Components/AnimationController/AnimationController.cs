using UnityEngine;

namespace SidiaGameJam.Controller
{
    public abstract class AnimationController : MonoBehaviour
    {
        protected Animator Animator;

        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        protected virtual void Start()
        {
        }

        protected abstract void BindEventsToFunctions();

        public abstract void Attack();

        public abstract void Move(float velocity);
    }
}