using System.Collections.Generic;
using SidiaGameJam.Events.EventConfig;
using UnityEngine;

namespace SidiaGameJam.Events
{
    public class EventListener : MonoBehaviour
    {
        public List<EventPropertyNoParam> events;

        protected virtual void OnEnable()
        {
            foreach (var value in events) value.soEvent.Register(this);
        }

        protected virtual void OnDisable()
        {
            foreach (var value in events) value.soEvent.UnRegister(this);
        }
    }
}