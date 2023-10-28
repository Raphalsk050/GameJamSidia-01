using System.Collections.Generic;
using UnityEngine;

namespace SidiaGameJam.Events
{
    public class SoEventOneParamRelated<T> : ScriptableObject
    {
        protected List<EventListenerOneParamRelated<T>> Listeners = new();
        protected List<GameObject> Targets = new();

        public void Register(EventListenerOneParamRelated<T> listener, GameObject target)
        {
            Listeners.Add(listener);
            Targets.Add(target);
        }

        public void UnRegister(EventListenerOneParamRelated<T> listener, GameObject target)
        {
            Listeners.Remove(listener);
            Targets.Remove(target);
        }

        public void InvokeAction(T value, GameObject target)
        {
            var i = 0;
            foreach (var listener in Listeners)
            {
                foreach (var listenerEvent in listener.events)
                    if (listenerEvent.soEvent == this && target == Targets[i])
                        listenerEvent.eventExecutor?.Invoke(value);

                i++;
            }
        }
    }
}