using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class CursorSystem : MonoBehaviour
    {
        private void Update()
        {
            if (TimerSystem.Paused)
            {
                Cursor.lockState = CursorLockMode.None;
            }

            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}

