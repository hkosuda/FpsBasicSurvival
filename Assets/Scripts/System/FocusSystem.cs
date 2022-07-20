using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class FocusSystem : MonoBehaviour
    {
        static readonly float searchDistance = 5.0f;
        static readonly int itemLayer = 1 << Const.itemLayer;

        static public EventHandler<GameObject> Focused { get; set; }
        static public EventHandler<GameObject> Defocused { get; set; }
        static public EventHandler<GameObject> Touched { get; set; }

        static GameObject currentFocusedObject;

        private void Start()
        {
            SetEvent(1);
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        static void SetEvent(int indicator)
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

        static void UpdateMethod(object obj, float dt)
        {
            var ray = GetRay();

            if (Physics.Raycast(ray, out var hit, searchDistance, itemLayer))
            {
                var focusedObject = hit.collider.gameObject;
                
                if (currentFocusedObject != focusedObject)
                {
                    Defocused?.Invoke(null, currentFocusedObject);
                    Focused?.Invoke(null, focusedObject);

                    currentFocusedObject = focusedObject;
                }
            }

            else
            {
                if (currentFocusedObject != null)
                {
                    Defocused?.Invoke(null, currentFocusedObject);
                }

                currentFocusedObject = null;
            }

            if (Keyconfig.CheckInput(KeyAction.check, true))
            {
                if (currentFocusedObject != null)
                {
                    Touched?.Invoke(null, currentFocusedObject);
                }
            }

            // - inner function
            static Ray GetRay()
            {
                var origin = Player.Camera.transform.position;
                var rot = Player.Camera.transform.eulerAngles;

                var rotY = rot.y * Mathf.Deg2Rad;
                var rotX = -rot.x * Mathf.Deg2Rad;

                var x = Mathf.Cos(rotX) * Mathf.Sin(rotY);
                var y = Mathf.Sin(rotX);
                var z = Mathf.Cos(rotX) * Mathf.Cos(rotY);

                var direction = new Vector3(x, y, z);

                return new Ray(origin: origin, direction: direction);
            }
        }
    }
}

