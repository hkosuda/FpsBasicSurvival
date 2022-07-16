using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        detectedSound = Resources.Load<AudioClip>("Audio/enemy/enemy_detect_player");
        shotSound = Resources.Load<AudioClip>("Audio/enemy/turret_shooting");

        brain = gameObject.GetComponent<TurretBrain>();
        shootingSystem = gameObject.GetComponent<TurretShooter>();

        SetEvent(1);

        source = gameObject.GetComponent<AudioSource>();
        engineSource = gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    private void Update()
    {
        if (TimerSystem.Pause)
        {
            engineSource.volume = 0.0f;
        }

        else
        {
            engineSource.volume = Floats.Get(Floats.Item.volume_turret_moving);
        }
    }

    void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            shootingSystem.Shot += PlayShotSound;
            brain.FindStriker += PlayDetectedSound;
        }

        else
        {
            if (brain != null) { brain.FindStriker -= PlayDetectedSound; }
            if (shootingSystem != null) { shootingSystem.Shot -= PlayShotSound; }
        }
    }

    void PlayShotSound(object obj, bool mute)
    {
        source.volume = Floats.Get(Floats.Item.volume_turret_shooting);
        source.PlayOneShot(shotSound);
    }

    void PlayDetectedSound(object obj, bool mute)
    {
        source.volume = Floats.Get(Floats.Item.volume_detected_alert);
        source.PlayOneShot(detectedSound);
    }
}
