using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ControllerKeyinfo : MonoBehaviour
    {
        static ControllerKey forward;
        static ControllerKey backward;
        static ControllerKey right;
        static ControllerKey left;
        static ControllerKey crouch;
        static ControllerKey jump;

        private void Awake()
        {
            forward = GetKey(0);
            backward = GetKey(1);
            right = GetKey(2);
            left = GetKey(3);
            crouch = GetKey(4);
            jump = GetKey(5);

            // - inner function
            ControllerKey GetKey(int n)
            {
                return gameObject.transform.GetChild(n).gameObject.GetComponent<ControllerKey>();
            }
        }

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
                ReplaySystem.Updated += UpdateInfo;
            }

            else
            {
                ReplaySystem.Updated -= UpdateInfo;
            }
        }

        static void UpdateInfo(object obj, float[] data)
        {
            forward.SetValue(B(data[10]));
            backward.SetValue(B(data[11]));
            right.SetValue(B(data[12]));
            left.SetValue(B(data[13]));
            crouch.SetValue(B(data[14]));
            jump.SetValue(B(data[15]));

            // - inner function
            static bool B(float value)
            {
                return value > 0.5f;
            }
        }
    }
}

