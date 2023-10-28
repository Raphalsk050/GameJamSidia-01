using System.Collections.Generic;
using UnityEngine;

namespace SidiaGameJam.Data.Sound
{
    [CreateAssetMenu(fileName = "newSoundData", menuName = "Sound/SoundData")]
    public class SoundData: ScriptableObject
    {
        public List<AudioClip> Sound;
        public float Volume;
        public int Priority;
        public float MaxPitch;
        public float MinPitch;
    }
}