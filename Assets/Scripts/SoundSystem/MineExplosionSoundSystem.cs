using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class MineExplosionSoundSystem : MonoBehaviour
    {
        static AudioClip clip;
        static AudioSource audioSource;

        private void Awake()
        {
            clip = Resources.Load<AudioClip>("Sound/Enemy/mine_explosion");
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
                MineMain.MineExplosion += PlayItemSound;
            }

            else
            {
                MineMain.MineExplosion -= PlayItemSound;
            }
        }

        static void PlayItemSound(object obj, MineMain mine)
        {
            audioSource.volume = 0.5f;
            audioSource.PlayOneShot(clip);
        }
    }
}

