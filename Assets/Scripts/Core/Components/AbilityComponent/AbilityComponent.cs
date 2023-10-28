using System.Collections;
using System.Collections.Generic;
using SidiaGameJam.Abilities;
using SidiaGameJam.Enums;
using UnityEngine;

namespace SidiaGameJam.Components
{
    public class AbilityComponent : MonoBehaviour
    {
        public List<AbilityBase> Abilities { get; private set; }

        public List<AbilityBase> RunningAbilities { get; private set; }
        private GameObject _owner;

        private void Awake()
        {
            Abilities = new List<AbilityBase>();
            _owner = gameObject;
            RunningAbilities = new List<AbilityBase>();
        }

        public void ExecuteAbility(AbilityBase ability)
        {
            if (ability.State != AbilityState.Awaiting) return;
            RunningAbilities.Add(ability);
            StartCoroutine(ExecuteAbilityRoutine(ability));
        }

        public void ExecuteAbility(int index)
        {
            if (index >= Abilities.Count || RunningAbilities.Contains(Abilities[index]) || !Abilities[index])
                return;
            if (Abilities[index].State != AbilityState.Awaiting) return;
            RunningAbilities.Add(Abilities[index]);
            StartCoroutine(ExecuteAbilityRoutine(Abilities[index]));
        }

        public void AddAbility(AbilityBase ability)
        {
            if (Abilities.Contains(ability)) return;
            Abilities.Add(ability);
        }

        public void Remove(AbilityBase ability)
        {
            if (!Abilities.Contains(ability)) return;
            if (RunningAbilities.Contains(ability))
            {
                StopAbility(ability);
                RunningAbilities.Remove(ability);
            }

            Abilities.Remove(ability);
        }

        private IEnumerator ExecuteAbilityRoutine(AbilityBase ability)
        {
            yield return new WaitForSeconds(ability.timeToExecute);
            ability.Execute(_owner);
            StartCoroutine(StopAbilityRoutine(ability));
        }

        private IEnumerator StopAbilityRoutine(AbilityBase ability)
        {
            if (!RunningAbilities.Contains(ability)) yield break;
            yield return new WaitForSeconds(ability.timeToStop);
            StopAbility(ability);
        }

        private void StopAbility(AbilityBase ability)
        {
            if (!RunningAbilities.Contains(ability) || !Abilities.Contains(ability)) return;
            if (ability.State != AbilityState.Running) return;
            RunningAbilities[RunningAbilities.IndexOf(ability)].Stop();
            RunningAbilities.Remove(ability);
            StartCoroutine(ability.CountCooldown());
        }
    }
}