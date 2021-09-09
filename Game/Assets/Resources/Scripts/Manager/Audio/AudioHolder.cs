using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager.Audio;

public class AudioHolder : MonoBehaviour
{
    public SoundData[] sounds;
    private void Awake()
    {
        foreach (SoundData soundData in sounds)
        {
            soundData.Source = gameObject.AddComponent<AudioSource>();
            soundData.Source.clip = soundData.clip;
            soundData.Source.volume = soundData.volume;
            soundData.Source.pitch = soundData.pitch;
            soundData.Source.loop = soundData.loop;
        }
    }
}
