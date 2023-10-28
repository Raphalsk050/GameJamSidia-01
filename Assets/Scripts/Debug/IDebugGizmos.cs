using UnityEngine;

namespace SidiaGameJam.DebugFunctions
{
    public interface IDebugGizmos
    {
        public Color DebugColor { get; set; }
        public EDebugShape Shape { get; set; }
        public Vector3 Size { get; set; }
    }
}