using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class SVUI_RoundTime : MonoBehaviour
    {
        TextMeshProUGUI roundText;
        TextMeshProUGUI timerText;

        float currentTime;
        int currentRound;

        private void Awake()
        {
            roundText = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            timerText = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            UpdateText();
        }

        void Update()
        {
            if (currentTime == SV_Time.ActiveTime && currentRound == SV_Round.RoundNumber) { return; }

            currentTime = SV_Time.ActiveTime;
            currentRound = SV_Round.RoundNumber;

            UpdateText();
        }

        void UpdateText()
        {
            roundText.text = "Round " + SV_Round.RoundNumber.ToString() + " / " + Params.sv_clear_round.ToString();
            timerText.text = TxtUtil.Time(SV_Time.ActiveTime, true);
        }
    }
}

