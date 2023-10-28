using System.Collections.Generic;
using SidiaGameJam.Events.EventConfig;
using UnityEngine;

namespace SidiaGameJam.Events
{
    public class EventListenerOneParamRelated<T> : MonoBehaviour
    {
        public List<EventPropertyOneParamRelated<T>> events;

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