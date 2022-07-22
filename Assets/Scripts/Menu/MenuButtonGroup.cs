using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class MenuButtonGroup : MonoBehaviour
    {
        static GameObject _howToPlayWindow;
        static GameObject _commandWindow;
        static GameObject _settingWindow;

        private void Awake()
        {
            if (_howToPlayWindow == null) { _howToPlayWindow = Resources.Load<GameObject>("UI/HowToPlay"); }
            if (_commandWindow == null) { _commandWindow = Resources.Load<GameObject>("UI/AboutCommand"); }
            if (_settingWindow == null) { _settingWindow = Resources.Load<GameObject>("UI/SettingWindow"); }
        }

        void Start()
        {
            var howToPlayButton = GetButton(0);
            howToPlayButton.onClick.AddListener(OpenHowToPlayWindow);

            var settingButton = GetButton(1);
            settingButton.onClick.AddListener(OpenSettingWindow);

            var commandButton = GetButton(2);
            commandButton.onClick.AddListener(OpenCommandWindow);

            var restartButton = GetButton(3);
            restartButton.onClick.AddListener(Restart);

            var finishGameButton = GetButton(4);
            finishGameButton.onClick.AddListener(FinishTheGame);
        }

        // - inner function
        Button GetButton(int n)
        {
            return gameObject.transform.GetChild(n).gameObject.GetComponent<Button>();
        }

        static void OpenHowToPlayWindow()
        {
            Instantiate(_howToPlayWindow);
        }

        static void OpenSettingWindow()
        {
            Instantiate(_settingWindow);
        }

        static void OpenCommandWindow()
        {
            Instantiate(_commandWindow);
        }

        static void Restart()
        {
            Confirmation.BeginConfirmation("最初のラウンドからやり直しますか？", RequestRestart, null);

            // - inner function
            static void RequestRestart()
            {
                GameSystem.SwitchHost(HostName.survival);
            }
        }

        static void FinishTheGame()
        {
            Confirmation.BeginConfirmation("ゲームを終了しますか？", FinishTheGameMethod, null);

            // - inner function
            static void FinishTheGameMethod()
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif

                Application.Quit();
            }
        }

        
    }
}
