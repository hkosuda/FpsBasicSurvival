using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSound : MonoBehaviour
{
    static AudioClip detectedAlert;

    AudioSource source;
    AudioSource engineSource;

    MineBrain brain;

    private void Awake()
    {
        if (detectedAlert == null) { detectedAlert = Resources.Load<AudioClip>("Audio/enemy/detected_alert"); }

        brain = gameObject.GetComponent<MineBrain>();

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
            engineSource.volume = Floats.Get(Floats.Item.volume_mine_moving);
        }
    }

    void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            brain.PlayerDetected += PlayDetectedSound;
        }

        else
        {
            if (brain != null) { brain.PlayerDetected -= PlayDetectedSound; }
        }
    }

    void PlayDetectedSound(object obj, bool mute)
    {
        source.volume = Floats.Get(Floats.Item.volume_detected_alert);
        source.PlayOneShot(detectedAlert);
    }
}
