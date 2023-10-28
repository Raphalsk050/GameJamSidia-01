using System.Collections.Generic;
using UnityEngine;

namespace SidiaGameJam.Events
{
    public class EventListenerFourParamRelated<T, TU, TV, TW> : MonoBehaviour
    {
        public List<EventPropertyFourParamRelated<T, TU, TV, TW>> events;

        private void OnEnable()
        {
            foreach (var eventProperty in events) eventProperty.soEvent.Register(this, transform.root.gameObject);
        }

        private void OnDisable()
        {
            foreach (var eventProperty in events) eventProperty.soEvent.UnRegister(this, transform.root.gameObject);
        }
    }
}