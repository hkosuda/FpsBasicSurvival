using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class PM_Main : MonoBehaviour
    {
        static readonly List<Controller> controllerList = new List<Controller>()
        {
            new PM_Camera(),
            new PM_InputVector(),

            new PM_Landing(),
            new PM_Jumping(),

            new PM_Crouching(),

            new PM_PlaneVector(),
            new PM_PostProcessor(),
        };

        void Start()
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
                controller.Update(Time.deltaTime);
            }
        }

        static void LateUpdate(object obj, bool mute)
        {
            foreach(var controller in controllerList)
            {
                controller.LateUpdate();
            }
        }

        static void FixedUpdate(object obj, float dt)
        {
            foreach(var controller in controllerList)
            {
                controller.FixedUpdate(dt);
            }
        }
    }
}

