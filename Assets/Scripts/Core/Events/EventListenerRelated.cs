using System.Collections.Generic;
using SidiaGameJam.Events.EventConfig;
using UnityEngine;

namespace SidiaGameJam.Events
{
    public class EventListenerRelated : MonoBehaviour
    {
        public List<EventPropertyNoParamRelated> events;

        protected virtual void OnEnable()
        {
            foreach (var value in events) value.soEventRelated.Register(this, transform.root.gameObject);
        }

        protected virtual void OnDisable()
        {
            foreach (var value in events) value.soEventRelated.UnRegister(this, transform.root.gameObject);
        }
    }
}