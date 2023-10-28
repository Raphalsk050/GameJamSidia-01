using SidiaGameJam.Components;
using UnityEngine;

namespace SidiaGameJam.Effects
{
    [CreateAssetMenu(fileName = "EffectWithTick", menuName = "Effects/Heal")]
    public class EffectHeal : EffectBase
    {
        private LifeComponentBase _lifeComponent;

        public override void ApplyEffect(GameObject target)
        {
            base.ApplyEffect(target);
            _lifeComponent = target.GetComponent<LifeComponentBase>();
            _lifeComponent.Heal(effectValueToBeApplied);
        }
    }
}