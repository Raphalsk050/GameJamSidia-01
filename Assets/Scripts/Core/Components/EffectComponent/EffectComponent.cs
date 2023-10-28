using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SidiaGameJam.Effects;
using SidiaGameJam.Enums;
using UnityEngine;

namespace SidiaGameJam.Components
{
    public class EffectComponent : ComponentBase
    {
        public List<EffectBase> effectsSo;
        public ParticleSystem componentParticle;
        public List<EffectBase> RunningEffects { get; private set; }
        private EffectBase _effectToAdd;
        private List<Coroutine> _effectsRoutines;
        
        public override void Initialize()
        {
            base.Initialize();
            RunningEffects = new List<EffectBase>();
            _effectsRoutines = new List<Coroutine>();
        }

        private EffectBase GetEffectFromList(EffectBase effect)
        {
            foreach (var effectSo in effectsSo)
            {
                if (effectSo.effectName == effect.effectName)
                {
                    return effectSo;
                }
            }

            return null;
        }
        
        public void AddEffect(EffectBase effect)
        {
            var newEffect = GetEffectFromList(effect);
            
            if (RunningEffects.Contains(newEffect)) return;
            
            newEffect.Execute(gameObject);

            newEffect.OnEffectStopped += RemoveEffect;

            if (newEffect.EffectHasTick)
            {
                var tickEffect = newEffect;
                _effectsRoutines.Add(StartCoroutine(EffectTimer(tickEffect.effectDuration, tickEffect.tickToExecute,
                    tickEffect)));
            }
            else
            {
                newEffect.ApplyEffect(gameObject);
                _effectsRoutines.Add(StartCoroutine(EffectTimer(newEffect.effectDuration, newEffect)));
            }

            RunningEffects.Add(newEffect);
        }

        public void RemoveEffect(EffectBase effect, GameObject target)
        {
            if (target != gameObject) return;
            
            RunningEffects.Remove(effect);
        }

        public IEnumerator EffectTimer(float effectDuration, EffectBase effect)
        {
            for (float i = 0; i < effectDuration; i += Time.deltaTime) yield return null;

            GetEffect(effect).Stop(gameObject);
        }

        private EffectBase GetEffect(EffectBase effect)
        {
            foreach (var runningEffect in RunningEffects)
            {
                if (runningEffect.effectName == effect.effectName)
                {
                    return runningEffect;
                }
            }

            return null;
        }

        public IEnumerator EffectTimer(float effectDuration, float tickToExecute, EffectBase effect)
        {
            var effectToApply = GetEffectFromList(effect);
            int currentTickValue = 0;
            int previousTickValue = 0;
            
            for (float i = 0; i < effectDuration; i += Time.deltaTime)
            {
                currentTickValue = Mathf.RoundToInt(i % tickToExecute);
                if (currentTickValue == 0 && currentTickValue != previousTickValue)
                {
                    effectToApply.ApplyEffect(gameObject);
                    
                    componentParticle.Play();

                    Debug.Log($"Applied effect {effectToApply.effectName} with tick! into {gameObject.name}");
                }

                previousTickValue = currentTickValue;
                yield return null;
            }

            effectToApply.Stop(gameObject);
        }
    }
}