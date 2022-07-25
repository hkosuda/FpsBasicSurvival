using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class SVUI_Difficulty : MonoBehaviour
    {
        static TextMeshProUGUI text;
        static Difficulty currentDifficulty;

        private void Awake()
        {
            text = gameObject.GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            UpdateText();
        }

        private void Update()
        {
            if (currentDifficulty != SvParams.CurrentDifficulty)
            {
                currentDifficulty = SvParams.CurrentDifficulty;
                UpdateText();
            }
        }

        static void UpdateText()
        {
            text.text = "Difficulty : " + currentDifficulty.ToString().ToUpper();
        }
    }
}

