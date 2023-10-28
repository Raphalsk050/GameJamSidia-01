using System;
using UnityEngine.Events;

namespace SidiaGameJam.Events.EventConfig
{
    [Serializable]
    public class EventPropertyOneParamRelated<T>
    {
        public SoEventOneParamRelated<T> soEvent;
        public UnityEvent<T> eventExecutor;
    }
}