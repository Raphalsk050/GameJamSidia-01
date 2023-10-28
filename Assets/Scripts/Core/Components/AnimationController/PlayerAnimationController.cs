using SidiaGameJam.Events;
using UnityEngine;

namespace SidiaGameJam.Controller
{
    [RequireComponent(
        typeof(EventListenerRelated),
        typeof(EventListenerOneParamRelated<float>),
        typeof(EventListenerOneParamRelated<float>))
    ]
    public class PlayerAnimationController : AnimationController
    {
        public EventListenerRelated eventListenerRelated;
        public EventListenerOneParamRelated<float> eventListenerOneFloatParamRelated;
        public EventListenerOneParamRelated<bool> eventListenerOneBoolParamRelated;

        protected override void Awake()
        {
            base.Awake();
            eventListenerRelated = GetComponent<EventListenerRelated>();
            eventListenerOneFloatParamRelated = GetComponent<EventListenerOneParamRelated<float>>();
            eventListenerOneBoolParamRelated = GetComponent<EventListenerOneParamRelated<bool>>();
        }

        protected override void BindEventsToFunctions()
        {
        }

        public void PauseAnimation()
        {
            Animator.enabled = false;
        }

        public void ResumeAnimation()
        {
            Animator.enabled = true;
        }

        public override void Attack()
        {
            Animator.SetTrigger("Attack");
        }

        public override void Move(float velocity)
        {
            Animator.SetFloat("Velocity", velocity);
        }

        public void Protect()
        {
            Animator.SetTrigger("Protect");
        }

        public void Unprotect()
        {
            Animator.SetTrigger("Unprotect");
        }

        public void Jump()
        {
            Animator.SetTrigger("Jump");
        }
        
        public void TakeHit()
        {
            Animator.SetTrigger("TakeHit");
        }

        public void Grounded(bool value)
        {
            Animator.SetBool("Grounded", value);
        }

        public void Falling(bool value)
        {
            Animator.SetBool("Falling", value);
        }
        
        public void Dead(bool value)
        {
            Animator.SetBool("Dead", value);
        }

        public void DodgeChanged(bool value)
        {
            if (value) Animator.SetTrigger("Dodge");
            Animator.SetBool("Dodging", value);
        }
    }
}