using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ChatSoundSystem : MonoBehaviour
    {
        static AudioClip clip;
        static AudioSource audioSource;

        private void Awake()
        {
            clip = Resources.Load<AudioClip>("Sound/System/chat_sound");
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
                ChatMessageManager.ChatUpdated += PlayChatSound;
            }

            else
            {
                ChatMessageManager.ChatUpdated -= PlayChatSound;
            }
        }

        static void PlayChatSound(object obj, bool mute)
        {
            audioSource.volume = 0.5f;
            audioSource.PlayOneShot(clip);
        }
    }
}
