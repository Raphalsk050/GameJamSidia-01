using System.Collections;
using System.Collections.Generic;
using SidiaGameJam.Core.Items;
using SidiaGameJam.Enums;
using UnityEngine;
using UnityEngine.Events;

public class InteractableBase : MonoBehaviour, IInteractable
{
    public UnityEvent activationComplete;

    private bool _canInteract;
    private EnableState _state;
    
    public virtual void Interact(GameObject instigator)
    {
        if (_state == EnableState.Disabled) return;
    }

    public virtual void EnableInteraction()
    {
        _state = EnableState.Enabled;
    }

    public virtual void DisableInteraction()
    {
        _state = EnableState.Disabled;
    }
}
