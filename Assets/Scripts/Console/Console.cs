using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Console : MonoBehaviour
    {
        static public EventHandler<bool> Opened { get; set; }
        static public bool Active { get; private set; }

        static public GameObject ConsoleLogContent { get; private set; }
        static GameObject canvas;
        
        private void Awake()
        {
            canvas = gameObject.transform.GetChild(0).gameObject;
            ConsoleLogManager.Initialize();

            ConsoleLogContent = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        }

        private void Start()
        {
            CloseConsole();
        }

        void Update()
        {
            if (Keyconfig.CheckInput(KeyAction.console, true))
            {
                OpenConsole();
            }
        }

        static public void OpenConsole()
        {
            Active = true;

            TimerSystem.Pause();
            canvas.SetActive(true);

            ConsoleInputField.Activate();

            Opened?.Invoke(null, false);
        }

        static public void CloseConsole()
        {
            Active = false;

            TimerSystem.Resume();
            canvas.SetActive(false);
        }
    }
}

