using UnityEngine.Audio;
using UnityEngine;
namespace Manager.Audio
{
    public enum SoundEnum { Alarm, Theme1 }
    [System.Serializable]
    public class SoundData
    {
        public AudioClip clip;
        public SoundEnum Name;
        [Range(0f,1f)]
        public float volume;
        [Range(-3f, 3f)]
        public float pitch;
        public bool loop;
        public AudioSource Source { get; set; }
    }
}

