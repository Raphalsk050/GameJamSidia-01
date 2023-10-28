using System;
using UnityEngine.Events;

namespace SidiaGameJam.Events.EventConfig
{
    [Serializable]
    public class EventPropertyNoParamRelated
    {
        public SoEventRelated soEventRelated;
        public UnityEvent eventExecutor;
    }
}