using System;
using System.Collections;
using System.Collections.Generic;
using GJS.Helper;
using UnityEngine;

namespace GJS.Core
{
    public class PressurePlate : MonoBehaviour
    {

        [SerializeField] private string _catTag;
        [SerializeField] private float _animationTime;
        [SerializeField] private float _maxModifier;
        [SerializeField] private Transform _plateTransform;

        private Coroutine _pressAnimation;
        private Coroutine _releaseAnimation;
        private Vector3 _originalScale;
        private float _minVerticalSize;

        private void Awake()
        {
            _originalScale = _plateTransform.localScale;
            _minVerticalSize = _originalScale.y * _maxModifier;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag(_catTag)) return;

            if (_releaseAnimation != null)
            {
                StopCoroutine(_releaseAnimation);
            }
            var currentT = Mathf.InverseLerp(_minVerticalSize, _originalScale.y, _plateTransform.localScale.y);
            var currentSize = _originalScale.y * currentT;
            var timeToAnimation = _animationTime * currentT;
            _pressAnimation = StartCoroutine(LerpHelp.Lerp(currentSize, _minVerticalSize, timeToAnimation, UpdatePlateSize));
        }
        
        private IEnumerator OnTriggerExit2D(Collider2D other)
        {
            if (_pressAnimation != null)
            {
                StopCoroutine(_pressAnimation);
            }
            yield return new WaitForSeconds(2.5f);
            var localScale = _plateTransform.localScale;
            var currentT = Mathf.InverseLerp(_minVerticalSize, _originalScale.y, localScale.y);
            var timeToAnimation = _animationTime - (_animationTime * currentT);
            _releaseAnimation = StartCoroutine(LerpHelp.Lerp(localScale.y, _originalScale.y, timeToAnimation, UpdatePlateSize));
        }
        
        private void UpdatePlateSize(float verticalSize)
        {
            var newScale = new Vector3(_originalScale.x, verticalSize, _originalScale.z);
            var delta = _plateTransform.localScale.y - verticalSize;
            var newPos = _plateTransform.localPosition;
            newPos.y -= delta * 0.5f;

            _plateTransform.localPosition = newPos;
            _plateTransform.localScale = newScale;
        }

    }
}
