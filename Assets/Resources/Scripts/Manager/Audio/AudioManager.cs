using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager.Audio
{
    public class AudioManager : MonoBehaviour
    {
        AudioHolder audioHolder;
        private void Awake()
        {
            if(SceneManager.sceneCount > 1)
                audioHolder = SceneManager.GetSceneByBuildIndex(0).GetRootGameObjects()[0].GetComponent<AudioHolder>();
        }
        protected void PlaySound(SoundEnum name)
        {
            if (audioHolder != null)
            {
                SoundData sound = System.Array.Find(audioHolder.sounds, SoundData => SoundData.Name == name);
                sound.Source.Play();
            }
        }
        protected void StopSound(SoundEnum name)
        {
            if (audioHolder != null)
            {
                SoundData sound = System.Array.Find(audioHolder.sounds, SoundData => SoundData.Name == name);
                sound.Source.Stop();
            }
        }
    }

}