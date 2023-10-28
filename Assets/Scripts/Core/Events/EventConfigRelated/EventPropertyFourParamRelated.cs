using System;
using UnityEngine.Events;

namespace SidiaGameJam.Events
{
    [Serializable]
    public class EventPropertyFourParamRelated<T, TU, TV, TW>
    {
        public SoEventFourParam<T, TU, TV, TW> soEvent;
        public UnityEvent<T, TU, TV, TW> eventExecutor;
    }
}