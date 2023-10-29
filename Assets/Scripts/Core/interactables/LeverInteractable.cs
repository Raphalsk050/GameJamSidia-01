using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverInteractable : InteractableBase
{
    public Transform leverRoot;
    public float animationDuration;
    
    private const float ACTIVATED_ROTATION = -60f;
    private const float DEACTIVATED_ROTATION = 60f;
    private float _animationPercent;
    private int _targetPercent = 1;
    private Coroutine _animationRoutine;
    private bool _activated;

    private void Start()
    {
        EnableInteraction();
    }

    public override void Interact(GameObject instigator)
    {
        if (_activated) return;
        
        base.Interact(instigator);
        _targetPercent = -_targetPercent;
        _animationRoutine = StartCoroutine(ActivationRoutine(DEACTIVATED_ROTATION, ACTIVATED_ROTATION));
    }

    private IEnumerator ActivationRoutine(float initialAngle, float finalAngle)
    {
        float timer = 0f;
        while (_animationPercent <= 1f)
        {
            timer += Time.deltaTime;
            _animationPercent = timer / animationDuration;

            var newRotation = Mathf.LerpAngle(initialAngle, finalAngle, _animationPercent);

            leverRoot.rotation = Quaternion.Euler(0,0,newRotation);

            yield return null;
        }
        
        _activated = true;
        activationComplete?.Invoke();
    }
}
