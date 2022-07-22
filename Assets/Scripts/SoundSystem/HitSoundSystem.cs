using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class HitSoundSystem : MonoBehaviour
    {
        static AudioClip clip;
        static AudioSource audioSource;

        private void Awake()
        {
            clip = Resources.Load<AudioClip>("Sound/System/hit_sound");
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
                EnemyMain.EnemyDamageTaken += PlayHitSound;
            }

            else
            {
                EnemyMain.EnemyDamageTaken -= PlayHitSound;
            }
        }

        static void PlayHitSound(object obj, float damage)
        {
            audioSource.volume = 0.5f;
            audioSource.PlayOneShot(clip);
        }
    }
}

