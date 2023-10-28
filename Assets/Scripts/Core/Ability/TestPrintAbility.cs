using System.Collections;
using SidiaGameJam.Enums;
using UnityEngine;

namespace SidiaGameJam.Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Abilities/PrintTest")]
    public class TestPrintAbility : AbilityBase
    {
        public override void Execute(GameObject target)
        {
            base.Execute(target);
            Debug.Log($"{target.name}");
        }
    }
}