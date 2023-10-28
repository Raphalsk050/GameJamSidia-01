using SidiaGameJam.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace SidiaGameJam.Effects
{
    public class EffectBase : ScriptableObject
    {
        public ParticleSystem ParticleSystem;
        public string effectName;
        public bool EffectHasTick = false;

#if UNITY_EDITOR
        [ConditionalHide("EffectHasTick", true)]
        public float tickToExecute;

        [ConditionalHide("EffectHasTick", true)]
        public float effectValueToBeApplied;
#else
        public float tickToExecute;
        public float effectValueToBeApplied;
#endif

        public float effectDuration = 2f;

        public UnityAction<EffectBase, GameObject> OnEffectStopped;
        
        public virtual void Execute(GameObject target)
        {
        }

        public virtual void ApplyEffect(GameObject target)
        {
        }

        public virtual void Stop(GameObject target)
        {
            OnEffectStopped?.Invoke(this, target);
        }
    }
}