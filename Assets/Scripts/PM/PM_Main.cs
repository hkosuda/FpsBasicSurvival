using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class PM_Main : MonoBehaviour
    {
        static public bool Interrupt { get; set; }

        static readonly List<Controller> controllerList = new List<Controller>()
        {
            new PM_Camera(),
            new PM_InputVector(),
            new PM_Observer(),

            new PM_Landing(),
            new PM_Jumping(),

            new PM_Crouching(),

            new PM_PlaneVector(),
            new PM_PostProcessor(),
        };

        void Awake()
        {
            foreach(var controller in controllerList)
            {
                controller.Initialize();
            }

            SetEvent(1);
        }

        private void OnDestroy()
        {
            foreach(var controller in controllerList)
            {
                controller.Shutdown();
            }

            SetEvent(-1);
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                TimerSystem.Updated += Update;
                TimerSystem.LateUpdated += LateUpdate;
                TimerSystem.FixedUpdated += FixedUpdate;
            }

            else
            {
                TimerSystem.Updated -= Update;
                TimerSystem.LateUpdated -= LateUpdate;
                TimerSystem.FixedUpdated -= FixedUpdate;
            }
        }

        static void Update(object obj, float dt)
        {
            foreach (var controller in controllerList)
            {
                if (Interrupt) { break; }
                controller.Update(Time.deltaTime);
            }

            Interrupt = false;
        }

        static void LateUpdate(object obj, bool mute)
        {
            foreach(var controller in controllerList)
            {
                if (Interrupt) { break; }
                controller.LateUpdate();
            }

            Interrupt = false;
        }

        static void FixedUpdate(object obj, float dt)
        {
            foreach(var controller in controllerList)
            {
                if (Interrupt) { break; }
                controller.FixedUpdate(dt);
            }

            Interrupt = false;
        }
    }
}

