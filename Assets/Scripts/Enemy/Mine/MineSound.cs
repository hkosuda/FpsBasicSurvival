using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class MineSound : MonoBehaviour
    {
        static AudioClip detectedAlert;
        AudioSource source;

        MineBrain brain;

        private void Awake()
        {
            if (detectedAlert == null) { detectedAlert = Resources.Load<AudioClip>("Sound/Enemy/mine_detected_alert"); }

            brain = gameObject.GetComponent<MineBrain>();
            source = gameObject.GetComponent<AudioSource>();
        }

        private void Start()
        {
            SetEvent(1);
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                brain.Detected += PlayDetectedSound;
            }

            else
            {
                brain.Detected -= PlayDetectedSound;
            }
        }

        void PlayDetectedSound(object obj, bool mute)
        {
            source.volume = Params.volume_detection_alert;
            source.PlayOneShot(detectedAlert);
        }
    }
}

