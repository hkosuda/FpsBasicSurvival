using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class TurretSound : MonoBehaviour
    {
        static AudioClip detectedSound;
        static AudioClip shotSound;

        AudioSource alertSource;
        AudioSource shootingSource;

        TurretBrain brain;
        TurretShooter shootingSystem;

        private void Awake()
        {
            if (detectedSound == null) { detectedSound = Resources.Load<AudioClip>("Sound/Enemy/turret_detected_alert"); }
            if (shotSound == null) { shotSound = Resources.Load<AudioClip>("Sound/Enemy/turret_shooting"); }

            brain = gameObject.GetComponent<TurretBrain>();
            shootingSystem = gameObject.GetComponent<TurretShooter>();

            alertSource = gameObject.GetComponent<AudioSource>();
            shootingSource = gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
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
            shootingSource.volume = Params.volume_turret_shot;
            shootingSource.PlayOneShot(shotSound);
        }

        void PlayDetectedSound(object obj, bool mute)
        {
            alertSource.volume = Params.volume_detection_alert;
            alertSource.PlayOneShot(detectedSound);
        }
    }
}

