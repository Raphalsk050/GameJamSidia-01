using System;
using System.Collections.Generic;
using SidiaGameJam.Data.Sound;
using SidiaGameJam.Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SidiaGameJam.Components.AudioComponent
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioComponent : ComponentBase
    {
        public List<SoundData> soundDatas;
        public bool canPlayAtARandomTime;
        public float maxRandomTimeToPlayAudio;

        private float _currentTime;
        private float _timeToPlayAudio;
        private List<SoundData> _playingSoundData;
        private AudioSource _audioSource;

        protected override void Awake()
        {
            base.Awake();
            _timeToPlayAudio = Random.Range(1f, maxRandomTimeToPlayAudio);
            _audioSource = GetComponent<AudioSource>();
            _playingSoundData = new List<SoundData>();
        }

        public virtual void PlaySound(SoundData soundData)
        {
            if (_playingSoundData.Contains(soundData))
            {
                _audioSource.Stop();
                _playingSoundData.Remove(soundData);
            }
            
            _audioSource.clip = soundData.Sound[Random.Range(0,soundData.Sound.Count)];
            _audioSource.volume = soundData.Volume;
            _audioSource.priority = soundData.Priority;
            _audioSource.pitch = Random.Range(soundData.MinPitch, soundData.MaxPitch);
            _audioSource.Play();
            _playingSoundData.Add(soundData);
        }

        private void FixedUpdate()
        {
            if (canPlayAtARandomTime)
            {
                if (_currentTime >= _timeToPlayAudio)
                {
                    _currentTime = 0;
                    PlaySound(soundDatas[0]);
                    _timeToPlayAudio = Random.Range(1f, maxRandomTimeToPlayAudio);
                }
                else
                {
                    _currentTime += Time.fixedDeltaTime;
                }
            }
        }
    }
}