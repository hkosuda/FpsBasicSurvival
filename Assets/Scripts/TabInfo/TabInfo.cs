using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class TabInfo : MonoBehaviour
    {
        static public bool Active { get; private set; }
        static GameObject canvas;

        private void Awake()
        {
            canvas = gameObject.transform.GetChild(0).gameObject;
        }

        void Start()
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
            if (InputSystem.CheckInput(Keyconfig.KeybindList[KeyAction.information], false))
            {
                var prev = Active;

                Active = true;
                canvas.SetActive(true);

                // must be after of 'set_active true' for waiting 'awake' of tab_info_content_manager
                if (prev == false)
                {
                    TabInfoContent.UpdateContent(null, false);
                }
            }

            else
            {
                Active = false;
                canvas.SetActive(false);
            }
        }
    }
}

