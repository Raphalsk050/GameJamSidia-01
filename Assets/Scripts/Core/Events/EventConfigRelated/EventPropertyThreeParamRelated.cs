using System;
using UnityEngine.Events;

namespace SidiaGameJam.Events
{
    [Serializable]
    public class EventPropertyThreeParamRelated<T, TU, TV>
    {
        public SoEventThreeParamRelated<T, TU, TV> soEvent;
        public UnityEvent<T, TU, TV> eventExecutor;
    }
}