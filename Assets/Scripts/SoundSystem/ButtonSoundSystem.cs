using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [RequireComponent(typeof(AudioSource))]
    public class ButtonSoundSystem : MonoBehaviour
    {
        static AudioSource audioSource;
        static AudioClip clip;

        private void Awake()
        {
            clip = Resources.Load<AudioClip>("Sound/System/button_sound");
            audioSource = gameObject.GetComponent<AudioSource>();
        }

        private void Start()
        {
            SetEvent(1);
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                NoisyButton.Clicked += PlayButtonSound;
            }

            else
            {
                NoisyButton.Clicked -= PlayButtonSound;
            }
        }

        static void PlayButtonSound(object obj, bool mute)
        {
            audioSource.volume = 0.5f;
            audioSource.PlayOneShot(clip);

        }
    }
}

