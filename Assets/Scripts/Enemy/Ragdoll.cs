using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Ragdoll : MonoBehaviour
    {
        static float ragdollExistTime = 0.6f;

        float ragdollExistTimeRemain;

        private void Awake()
        {
            ragdollExistTimeRemain = ragdollExistTime;
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
                TimerSystem.Updated += UpdateMethod;
            }

            else
            {
                TimerSystem.Updated -= UpdateMethod;
            }
        }

        void UpdateMethod(object obj, float dt)
        {
            ragdollExistTimeRemain -= dt;

            if (ragdollExistTimeRemain < 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}

