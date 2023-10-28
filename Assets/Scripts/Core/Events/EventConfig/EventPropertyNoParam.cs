using System;
using UnityEngine.Events;

namespace SidiaGameJam.Events.EventConfig
{
    [Serializable]
    public class EventPropertyNoParam
    {
        public SoEvent soEvent;
        public UnityEvent eventExecutor;
    }
}