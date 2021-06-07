using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioController : MonoBehaviour
    {
        private const float CLIP_VOLUME = 1.0f;

        private AudioSource audioPlayer;
        // Start is called before the first frame update
        void Start()
        {
            audioPlayer = GetComponent<AudioSource>();
        }

        public void PlayOneShotSound(AudioClip audioClip)
        {
            audioPlayer.PlayOneShot(audioClip, CLIP_VOLUME);
        }

        public void PlayOneShotSound(AudioClip audioClip, float vol)
        {
            audioPlayer.PlayOneShot(audioClip, vol);
        }
    }
}

