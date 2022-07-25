using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class EnemyEngineSound : MonoBehaviour
    {
        AudioSource source;

        void Start()
        {
            source = gameObject.GetComponent<AudioSource>();
        }

        void Update()
        {
            if (TimerSystem.Paused)
            {
                source.volume = 0.0f;
            }

            else
            {
                source.volume = Params.volume_mine_engine;
            }
        }
    }
}

