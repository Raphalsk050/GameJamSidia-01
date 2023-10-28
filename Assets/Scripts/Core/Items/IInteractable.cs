using UnityEngine;

namespace SidiaGameJam.Core.Items
{
    public interface IInteractable
    {
        public void Interact(GameObject instigator);
        public void EnableInteraction();
        public void DisableInteraction();
    }
}