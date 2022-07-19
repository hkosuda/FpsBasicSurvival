using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class TurretSound : MonoBehaviour
    {
        static AudioClip detectedSound;
        static AudioClip shotSound;

        AudioSource source;
        AudioSource engineSource;

        TurretBrain brain;
        TurretShooter shootingSystem;

        private void Awake()
        {
            if (detectedSound == null) { detectedSound = Resources.Load<AudioClip>("Sound/Enemy/detected_alert"); }
            if (shotSound == null) { shotSound = Resources.Load<AudioClip>("Sound/Enemy/turret_shooting"); }

            brain = gameObject.GetComponent<TurretBrain>();
            shootingSystem = gameObject.GetComponent<TurretShooter>();

            source = gameObject.GetComponent<AudioSource>();
            engineSource = gameObject.GetComponent<AudioSource>();
        }

        private void Start()
        {
            SetEvent(1);
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        private void Update()
        {
            if (TimerSystem.Paused)
            {
                engineSource.volume = 0.0f;
            }

            else
            {
                engineSource.volume = Params.volume_turret_engine;
            }
        }

        void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                shootingSystem.Shot += PlayShotSound;
                brain.Detected += PlayDetectedSound;
            }

            else
            {
                shootingSystem.Shot -= PlayShotSound;
                brain.Detected -= PlayDetectedSound;
            }
        }

        void PlayShotSound(object obj, bool mute)
        {
            source.volume = Params.volume_turret_shot;
            source.PlayOneShot(shotSound);
        }

        void PlayDetectedSound(object obj, bool mute)
        {
            source.volume = Params.volume_detection_alert;
            source.PlayOneShot(detectedSound);
        }
    }
}

