using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class SVUI_HP : MonoBehaviour
    {
        static TextMeshProUGUI hpText;
        static StatusBar statusBar;

        static int currentHP;
        static int currentMaxHP;

        void Start()
        {
            hpText = gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            statusBar = gameObject.transform.GetChild(1).gameObject.GetComponent<StatusBar>();
        }

        void Update()
        {
            var _currentHP = SV_Status.CurrentHP;
            var _currentMaxHP = SV_Status.CurrentMaxHP;

            if (currentHP != _currentHP || currentMaxHP != _currentMaxHP)
            {
                currentHP = _currentHP;
                currentMaxHP = _currentMaxHP;

                UpdateUI();
            }
        }

        void UpdateUI()
        {
            hpText.text = currentHP.ToString() + " / " + currentMaxHP.ToString();

            var value = Calcf.SafetyDiv(currentHP, currentMaxHP, 0.0f);
            value = Calcf.Clip(0.0f, 1.0f, value);

            statusBar.SetValue(value);
        }
    }
}

