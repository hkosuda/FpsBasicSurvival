using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Shop : MonoBehaviour
    {
        private void OnDestroy()
        {
            TimerSystem.Resume();
        }

        void Update()
        {
            TimerSystem.Pause();
        }
    }
}

