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
            if (currentTime == SV_TimeAdmin.ActiveTime && currentRound == SV_RoundAdmin.RoundNumber) { return; }

            currentTime = SV_TimeAdmin.ActiveTime;
            currentRound = SV_RoundAdmin.RoundNumber;

            UpdateText();
        }

        void UpdateText()
        {
            roundText.text = "Round " + SV_RoundAdmin.RoundNumber.ToString() + " / " + Params.sv_clear_round.ToString();
            timerText.text = TxtUtil.Time(SV_TimeAdmin.ActiveTime, true);
        }
    }
}

