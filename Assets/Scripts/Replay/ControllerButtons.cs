using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class ControllerButtons : MonoBehaviour
    {
        static TextMeshProUGUI playStopText;

        private void Awake()
        {
            var prevButton = GetButton(0);
            prevButton.onClick.AddListener(ToTheStart);

            var backwardButton = GetButton(1);
            backwardButton.onClick.AddListener(Backward);

            var playStopButton = GetButton(2);
            playStopButton.onClick.AddListener(PlayStop);

            var forwardButton = GetButton(3);
            forwardButton.onClick.AddListener(Forward);

            var nextButton = GetButton(4);
            nextButton.onClick.AddListener(ToTheEnd);

            playStopText = playStopButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            // - inner function
            Button GetButton(int n)
            {
                return gameObject.transform.GetChild(n).gameObject.GetComponent<Button>();
            }
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
                ReplayTimer.TimerPaused += UpdateText1;
                ReplayTimer.TimerResumed += UpdateText2;
            }

            else
            {
                ReplayTimer.TimerPaused -= UpdateText1;
                ReplayTimer.TimerResumed -= UpdateText2;
            }
        }

        static void ToTheStart()
        {
            ReplaySystem.ToTheStart();
        }

        static void ToTheEnd()
        {
            ReplaySystem.ToTheEnd();
        }

        static void Backward()
        {
            ReplaySystem.Backward();
        }

        static void Forward()
        {
            ReplaySystem.Forward();
        }

        static void PlayStop()
        {
            if (ReplayTimer.Paused)
            {
                ReplayTimer.Resume();
            }

            else
            {
                ReplayTimer.Pause();
            }
        }

        static void UpdateText1(object obj, bool mute)
        {
            UpdateText();
        }

        static void UpdateText2(object obj, bool mute)
        {
            UpdateText();
        }

        static void UpdateText()
        {
            if (ReplayTimer.Paused)
            {
                playStopText.text = ">";
            }

            else
            {
                playStopText.text = "||";
            }
        }
    }
}

