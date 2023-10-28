#if UNITY_EDITOR

using UnityEngine;

namespace SidiaGameJam.DebugFunctions
{
    public class DebugDrawGizmos : MonoBehaviour, IDebugGizmos
    {
        private Vector3 _size = Vector3.one;

        public Vector2 Position { get; set; }

        public void OnDrawGizmos()
        {
            Gizmos.color = DebugColor;

            switch (Shape)
            {
                case EDebugShape.Cube:
                    Gizmos.DrawCube(Position, _size);
                    Gizmos.DrawWireCube(Position, _size);
                    break;

                case EDebugShape.Sphere:
                    Gizmos.DrawSphere(Position, _size.x);
                    Gizmos.DrawWireSphere(Position, _size.x);
                    break;
            }
        }

        public Color DebugColor { get; set; }

        public EDebugShape Shape { get; set; }

        public Vector3 Size
        {
            get => _size;
            set => _size = value;
        }
    }
}
#endif