using System.Collections.Generic;
using UnityEngine;

namespace SidiaGameJam.Events
{
    public class SoEventFourParam<T, TU, TV, TW> : ScriptableObject
    {
        protected List<EventListenerFourParamRelated<T, TU, TV, TW>> Listeners = new();
        protected List<GameObject> Targets = new();

        public void Register(EventListenerFourParamRelated<T, TU, TV, TW> listener, GameObject target)
        {
            Listeners.Add(listener);
            Targets.Add(target);
        }

        public void UnRegister(EventListenerFourParamRelated<T, TU, TV, TW> listener, GameObject target)
        {
            Listeners.Remove(listener);
            Targets.Remove(target);
        }

        public virtual void InvokeAction(T valueT, TU valueU, TV valueV, TW valueW, GameObject target)
        {
            var i = 0;
            foreach (var listener in Listeners)
            {
                foreach (var listenerEvent in listener.events)
                    if (listenerEvent.soEvent == this && Targets[i] == target)
                        listenerEvent.eventExecutor?.Invoke(valueT, valueU, valueV, valueW);

                i++;
            }
        }
    }
}