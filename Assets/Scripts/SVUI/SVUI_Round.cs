using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class SVUI_Round : MonoBehaviour
    {
        static TextMeshProUGUI roundText;
        static int currentRound;

        private void Awake()
        {
            roundText = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            UpdateRoundText();
        }

        void Update()
        {
            if (currentRound != SV_Round.RoundNumber)
            {
                currentRound = SV_Round.RoundNumber;
                UpdateRoundText();
            }
        }

        static void UpdateRoundText()
        {
            roundText.text = "Round " + SV_Round.RoundNumber.ToString() + " / " + SvParams.GetInt(SvParam.clear_round).ToString();
        }
    }
}

