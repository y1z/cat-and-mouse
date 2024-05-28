using System;
using UnityEngine;

namespace Managers
{
    public class SoundFXManager : MonoBehaviour
    {
        public static SoundFXManager instance;

        [SerializeField] private AudioSource soundFXObject;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume = 0.5f)
        {
            AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

            audioSource.clip = audioClip;

            audioSource.volume = volume;
            
            audioSource.Play();

            Destroy(audioSource.gameObject, audioClip.length);
        }
    }
}