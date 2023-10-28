using UnityEngine;
using CharacterController = SidiaGameJam.Controller.CharacterController;
using CookinVaniaController = SidiaGameJam.Controller.Controller;

namespace SidiaGameJam.Effects
{
    [CreateAssetMenu(fileName = "EffectNoTick", menuName = "Effects/Freeze")]
    public class EffectFreeze : EffectBase
    {
        public override void ApplyEffect(GameObject target)
        {
            if (target.GetComponent<CookinVaniaController>())
                target.GetComponent<CookinVaniaController>().DisableInput();

            target.GetComponent<CookinVaniaController>().AnimationController.PauseAnimation();
            Debug.Log(target.name);
            base.ApplyEffect(target);   
        }

        public override void Stop(GameObject target)
        {
            if (target.GetComponent<CookinVaniaController>())
                target.GetComponent<CookinVaniaController>().EnableInput();

            target.GetComponent<CookinVaniaController>().AnimationController.ResumeAnimation();
            base.Stop(target);
        }
    }
}