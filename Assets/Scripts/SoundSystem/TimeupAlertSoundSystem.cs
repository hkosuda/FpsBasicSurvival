using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class TimeupAlertSoundSystem : MonoBehaviour
    {
        static AudioClip alertSound;
        static AudioClip clockSound;

        static AudioSource audioSource;

        static bool alertDone;
        static int prevSec;

        private void Awake()
        {
            alertSound = Resources.Load<AudioClip>("Sound/System/timeup_alert");
            clockSound = Resources.Load<AudioClip>("Sound/System/timeup_clock");

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
                TimerSystem.Updated += UpdateMethod;
            }

            else
            {
                TimerSystem.Updated -= UpdateMethod;
            }
        }

        static void UpdateMethod(object obj, float dt)
        {
            if (SV_Round.RoundNumber > 0 && GameSystem.CurrentHost.HostName == HostName.survival)
            {
                if (SV_Time.TimeRemain > 60.0f)
                {
                    alertDone = false;
                    prevSec = 60;

                    return;
                }

                if (!alertDone)
                {
                    alertDone = true;
                    PlayAlert();
                }

                if (SV_Time.TimeRemain < 30.0f)
                {
                    var sec = (int)SV_Time.TimeRemain;

                    if (sec != prevSec)
                    {
                        prevSec = sec;

                        var volume = 1.1f - (SV_Time.TimeRemain / 30.0f);
                        PlayClockSound(volume);
                    }
                }
            }
        }

        static void PlayAlert()
        {
            audioSource.volume = 0.5f;
            audioSource.PlayOneShot(alertSound);
        }

        static void PlayClockSound(float volume = 0.5f)
        {
            audioSource.volume = volume;
            audioSource.PlayOneShot(clockSound);
        }
    }
}



