using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class DE_Main : MonoBehaviour
    {
        static List<Controller> controllers;
        static public bool Suspend { get; set; }

        private void Awake()
        {
            controllers = new List<Controller>()
            {
                new DE_Availability(),
                new DE_Shooter(),
                new DE_Potensial(),
                new DE_Recoil(),
            };

            foreach (var controller in controllers)
            {
                controller.Initialize();
            }
        }

        void Start()
        {
            SetEvent(1);
        }

        private void OnDestroy()
        {
            foreach (var controller in controllers)
            {
                controller.Shutdown();
            }

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
            foreach (var controller in controllers)
            {
                controller.Update(dt);
            }
        }
    }
}

