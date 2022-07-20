using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class PlayerSound : MonoBehaviour
    {
        static readonly int landingSoundFrameBuffer = 8;
        static readonly float footstepInterval = 0.32f;

        static float landingSoundFrameBufferRemain;
        static float footstepIntervalRemain;

        static AudioSource audioSource;
        static AudioClip landingSound;
        static AudioClip footstepSound;

        static float prevVy;

        private void Awake()
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            landingSound = Resources.Load<AudioClip>("Player/landing");
            footstepSound = Resources.Load<AudioClip>("Player/footstep");
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
                PM_Landing.Landed += PlayLandingSound;

                TimerSystem.Updated += UpdateMethod;
                TimerSystem.LateUpdated += LateUpdateMethod;
            }

            else
            {
                PM_Landing.Landed -= PlayLandingSound;

                TimerSystem.Updated -= UpdateMethod;
                TimerSystem.LateUpdated -= LateUpdateMethod;
            }
        }

        static void PlayLandingSound(object obj, RaycastHit hit)
        {
            if (landingSoundFrameBufferRemain > 0) { return; }

            audioSource.volume = Params.volume_landing;

            audioSource.PlayOneShot(landingSound);
            landingSoundFrameBufferRemain = landingSoundFrameBuffer;
        }

        static void UpdateMethod(object obj, float dt)
        {
            landingSoundFrameBufferRemain--;
            if (landingSoundFrameBufferRemain < 0) { landingSoundFrameBufferRemain = 0; }

            if (PM_Landing.LandingIndicator <= 0)
            {
                footstepIntervalRemain = footstepInterval;
                return;
            }

            var speed = Params.pm_max_speed_on_ground;

            if (Player.Rb.velocity.magnitude < speed * 0.6f)
            {
                footstepIntervalRemain = footstepInterval;
                return;
            }

            footstepIntervalRemain -= dt;

            if (footstepIntervalRemain < 0.0f)
            {
                audioSource.volume = Params.volume_footstep;
                footstepIntervalRemain = footstepInterval;
                audioSource.PlayOneShot(footstepSound);
            }
        }

        static void LateUpdateMethod(object obj, bool mute)
        {
            prevVy = Player.Rb.velocity.y;
        }
    }
}

