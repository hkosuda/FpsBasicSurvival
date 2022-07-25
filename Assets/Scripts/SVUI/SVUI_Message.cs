using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class SVUI_Message : MonoBehaviour
    {
        static readonly float alertExistTime = 3.0f;

        static TextMeshProUGUI message;
        static TextMeshProUGUI alert;

        static float alertExistTimeRemain;

        private void Awake()
        {
            message = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            alert = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();

            message.text = "";
            alert.text = "";
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
            alertExistTimeRemain -= dt;
            if (-99.0f < alertExistTimeRemain && alertExistTimeRemain < 0.0f) { alertExistTimeRemain = -100.0f; alert.text = ""; }
        }

        static public void ShowMessage(string _message)
        {
            message.text = _message;
        }

        static public void ShowAlert(string _alert)
        {
            alert.text = _alert;
            alertExistTimeRemain = alertExistTime;
        }
    }
}

