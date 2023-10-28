using UnityEngine;

namespace SidiaGameJam.Data.Character
{
    [CreateAssetMenu(menuName = "Character/Ally", fileName = "New Ally Character")]
    public class CharacterData : ScriptableObject
    {
        public string CharacterName;
        public string Decription;
        public Sprite CharacterImage;
        public float MaxLife;
        public float WalkSpeed;
    }
}