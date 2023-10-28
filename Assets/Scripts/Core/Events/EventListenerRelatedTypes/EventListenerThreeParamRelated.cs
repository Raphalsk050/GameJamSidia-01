using System.Collections.Generic;
using UnityEngine;

namespace SidiaGameJam.Events
{
    public class EventListenerThreeParamRelated<T, TU, TV> : MonoBehaviour
    {
        public List<EventPropertyThreeParamRelated<T, TU, TV>> events;

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