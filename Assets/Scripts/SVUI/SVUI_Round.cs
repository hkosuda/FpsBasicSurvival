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
        static int currentMaxRound;

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
            if (currentRound != SV_Round.RoundNumber || currentMaxRound != SvParams.GetInt(SvParam.clear_round))
            {
                currentRound = SV_Round.RoundNumber;
                currentMaxRound = SvParams.GetInt(SvParam.clear_round);

                UpdateRoundText();
            }
        }

        static void UpdateRoundText()
        {
            roundText.text = "Round " + SV_Round.RoundNumber.ToString() + " / " + currentMaxRound.ToString();
        }
    }
}

