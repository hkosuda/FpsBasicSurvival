using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class Menu : MonoBehaviour
    {
        static GameObject canvas;
        static bool active;

        void Awake()
        {
            canvas = gameObject.transform.GetChild(0).gameObject;

            var closeButton = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Button>();
            closeButton.onClick.AddListener(CloseMenu);

            canvas.SetActive(false);
            active = false;
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
                TimerSystem.Updated += UpdateMethod;
            }

            else
            {
                TimerSystem.Updated -= UpdateMethod;
            }
        }

        static void UpdateMethod(object obj, float dt)
        {
            if (Keyconfig.CheckInput(KeyAction.menu, true))
            {
                OpenMenu();
            }
        }

        private void Update()
        {
            if (active) { TimerSystem.Pause(); }
        }

        static void OpenMenu()
        {
            canvas.SetActive(true);
            TimerSystem.Pause();

            active = true;
        }

        static void CloseMenu()
        {
            canvas.SetActive(false);
            TimerSystem.Resume();

            active = false;
        }
    }
}

