using SidiaGameJam.Components;
using SidiaGameJam.Data.Character;
using SidiaGameJam.Components;
using SidiaGameJam.Data.Character;
using UnityEngine;

[RequireComponent(typeof(LifeComponentBase))]
public class CharacterBase : MonoBehaviour
{
    public CharacterData characterData;
    public LifeComponentBase LifeComponent { get; protected set; }

    protected virtual void Awake()
    {
        LifeComponent = GetComponent<LifeComponentBase>();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    protected virtual void OnEnable()
    {
        LifeComponent.SetMaxLife(characterData.MaxLife);
    }
}