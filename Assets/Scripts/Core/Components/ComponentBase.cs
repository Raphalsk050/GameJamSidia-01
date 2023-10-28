using UnityEngine;

namespace SidiaGameJam.Components
{
    public abstract class ComponentBase : MonoBehaviour
    {
        protected GameObject Owner;

        protected virtual void Awake()
        {
            Initialize();
            Owner = gameObject;
        }

        public virtual void Initialize()
        {
        }

        protected GameObject GetOwner()
        {
            return Owner;
        }
    }
}