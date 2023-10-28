using System.Collections;
using SidiaGameJam.Enums;
using UnityEngine;

namespace SidiaGameJam.Abilities
{
    public abstract class AbilityBase : ScriptableObject
    {
        public string abilityName;
        public float timeToExecute;
        public float timeToStop;
        public float cooldown;
        private GameObject _target;
        public AbilityState State { get; protected set; } = AbilityState.Awaiting;

        public virtual void Execute(GameObject target)
        {
            State = AbilityState.Running;
            _target = target;
            Debug.Log("Ability Execute Triggered!");
        }

        public IEnumerator CountCooldown()
        {
            for (float i = 0; i < cooldown; i += Time.deltaTime) yield return null;

            State = AbilityState.Awaiting;
        }

        public virtual void Stop()
        {
            Debug.Log("Ability Stop Triggered!");
        }
    }
}