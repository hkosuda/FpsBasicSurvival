using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ItemSoundSystem : MonoBehaviour
    {
        static AudioClip clip;
        static AudioSource audioSource;

        private void Awake()
        {
            clip = Resources.Load<AudioClip>("Sound/System/item_sound");
            audioSource = gameObject.GetComponent<AudioSource>();
        }

        void Start()
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
                FieldItem.PlayerGotItem += PlayItemSound;
            }

            else
            {
                FieldItem.PlayerGotItem -= PlayItemSound;
            }
        }

        static void PlayItemSound(object obj, Item item)
        {
            audioSource.volume = 0.5f;
            audioSource.PlayOneShot(clip);
        }
    }
}
