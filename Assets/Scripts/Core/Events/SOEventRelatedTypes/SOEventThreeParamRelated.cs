using System.Collections.Generic;
using UnityEngine;

namespace SidiaGameJam.Events
{
    public class SoEventThreeParamRelated<T, TU, TV> : ScriptableObject
    {
        protected List<EventListenerThreeParamRelated<T, TU, TV>> Listeners = new();
        protected List<GameObject> Targets = new();

        public void Register(EventListenerThreeParamRelated<T, TU, TV> listener, GameObject target)
        {
            Listeners.Add(listener);
            Targets.Add(target);
        }

        public void UnRegister(EventListenerThreeParamRelated<T, TU, TV> listener, GameObject target)
        {
            Listeners.Remove(listener);
            Targets.Remove(target);
        }

        public void InvokeAction(T value, TU valueU, TV valueV, GameObject target)
        {
            var i = 0;
            foreach (var listener in Listeners)
            {
                foreach (var listenerEvent in listener.events)
                    if (listenerEvent.soEvent == this && target == Targets[i])
                        listenerEvent.eventExecutor?.Invoke(value, valueU, valueV);

                i++;
            }
        }
    }
}