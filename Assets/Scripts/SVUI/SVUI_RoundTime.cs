using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class SVUI_RoundTime : MonoBehaviour
    {
        static TextMeshProUGUI roundText;
        static TextMeshProUGUI timerText;

        static float currentTime;
        static int currentRound;

        private void Awake()
        {
            roundText = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            timerText = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            UpdateRoundText();
            UpdateTimeText();
        }

        void Update()
        {
            if (currentRound != SV_Round.RoundNumber)
            {
                currentRound = SV_Round.RoundNumber;
                UpdateRoundText();
            }

            if (currentTime != SV_Time.ActiveTime)
            {
                currentTime = SV_Time.ActiveTime;
                UpdateTimeText();
            }
        }

        static void UpdateRoundText()
        {
            roundText.text = "Round " + SV_Round.RoundNumber.ToString() + " / " + SvParams.GetInt(SvParam.clear_round).ToString();
        }

        static void UpdateTimeText()
        {
            timerText.text = TxtUtil.Time(SV_Time.ActiveTime, true);
        }
    }
}

