using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJS.Helper
{
    public static class LerpHelp
    {
        
        private static WaitForEndOfFrame _waitEndOfFrame = new WaitForEndOfFrame();
    
        public static IEnumerator Lerp(float lerpTime, Action<float> callback)
        {
            var timeElapsed = 0.0f;
            callback?.Invoke(0f);

            while (timeElapsed < lerpTime)
            {
                timeElapsed += Time.deltaTime;
                var t = timeElapsed / lerpTime;
                callback?.Invoke(t);
                yield return _waitEndOfFrame;
            }
        
            callback?.Invoke(1.0f);
        }

        public static IEnumerator Lerp(float startValue, float endValue, float lerpTime, Action<float> callback, Action finished = null)
        {
            var timeElapsed = 0.0f;
            callback?.Invoke(startValue);

            while (timeElapsed < lerpTime)
            {
                timeElapsed += Time.deltaTime;
                var t = timeElapsed / lerpTime;
                var value = Mathf.Lerp(startValue, endValue, t);
                callback?.Invoke(value);
                yield return _waitEndOfFrame;
            }
        
            callback?.Invoke(endValue);
            finished?.Invoke();
        }


    }

}