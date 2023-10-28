using System.Collections.Generic;
using UnityEngine;

namespace SidiaGameJam.Events
{
    [CreateAssetMenu(fileName = "EventSO", menuName = "EventsRelated/NoAttributes")]
    public class SoEventRelated : ScriptableObject
    {
        private readonly List<EventListenerRelated> _listeners = new();
        private readonly List<GameObject> _targets = new();

        public virtual void Register(EventListenerRelated listenerRelated, GameObject target)
        {
            _listeners.Add(listenerRelated);
            _targets.Add(target);
        }

        public virtual void UnRegister(EventListenerRelated listenerRelated, GameObject target)
        {
            _listeners.Remove(listenerRelated);
            _targets.Remove(target);
        }

        public void InvokeAction(GameObject target)
        {
            var i = 0;
            foreach (var listener in _listeners)
            {
                foreach (var listenerEvent in listener.events)
                    if (listenerEvent.soEventRelated == this && target == _targets[i])
                        listenerEvent.eventExecutor?.Invoke();

                i++;
            }
        }
    }
}