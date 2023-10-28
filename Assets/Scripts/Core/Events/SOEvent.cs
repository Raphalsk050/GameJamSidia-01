using System.Collections.Generic;
using UnityEngine;

namespace SidiaGameJam.Events
{
    [CreateAssetMenu(fileName = "EventSO", menuName = "Events/NoAttributes")]
    public class SoEvent : ScriptableObject
    {
        private readonly List<EventListener> _listeners = new();

        public virtual void Register(EventListener listener)
        {
            _listeners.Add(listener);
        }

        public virtual void UnRegister(EventListener listener)
        {
            _listeners.Remove(listener);
        }

        public void InvokeAction()
        {
            var i = 0;
            foreach (var listener in _listeners)
            {
                foreach (var listenerEvent in listener.events)
                    if (listenerEvent.soEvent == this)
                        listenerEvent.eventExecutor?.Invoke();

                i++;
            }
        }
    }
}