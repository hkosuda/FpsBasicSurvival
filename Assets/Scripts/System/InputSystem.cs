using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class InputSystem : MonoBehaviour
    {
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
            //if (indicator > 0)
            //{
            //    TimerSystem.TimerPaused += InactivateOnTimerPaused;
            //    TimerSystem.TimerResumed += ActivateOnTimerResumed;
            //}

            //else
            //{
            //    TimerSystem.TimerPaused -= InactivateOnTimerPaused;
            //    TimerSystem.TimerResumed -= ActivateOnTimerResumed;
            //}
        }

        //private void Update()
        //{
        //    if (InGameTimer.Paused)
        //    {
        //        Active = false;
        //    }
        //}

        //static void InactivateOnTimerPaused(object obj, bool mute)
        //{
        //    Inactivate();
        //}

        //static void ActivateOnTimerResumed(object obj, bool mute)
        //{
        //    Activate();
        //}


        static public bool CheckInput(Keyconfig.Key key, bool getKeyDown)
        {
            if (key.keyCode != KeyCode.None)
            {
                if (getKeyDown)
                {
                    if (Input.GetKeyDown(key.keyCode))
                    {
                        return true;
                    }
                }

                else
                {
                    if (Input.GetKey(key.keyCode))
                    {
                        return true;
                    }
                }
            }

            // keybind.keyCode == KeyCode.None
            else
            {
                float wheelDelta = key.wheelDelta;

                if (wheelDelta > 0)
                {
                    if (Input.mouseScrollDelta.y > 0)
                    {
                        return true;
                    }
                }

                else
                {
                    if (Input.mouseScrollDelta.y < 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

