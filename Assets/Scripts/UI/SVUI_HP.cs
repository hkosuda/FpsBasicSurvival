using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class SVUI_HP : MonoBehaviour
    {
        TextMeshProUGUI hpText;
        StatusBar statusBar;

        int currentHP;
        int currentMaxHP;

        void Start()
        {
            hpText = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            statusBar = gameObject.transform.GetChild(1).gameObject.GetComponent<StatusBar>();
        }

        void Update()
        {
            if (SV_StatusAdmin.StatusList != null)
            {
                var _currentHP = SV_StatusAdmin.StatusList[SV_Status.hp];
                var _currentMaxHP = SV_StatusAdmin.CurrentMaxHP;

                if (_currentHP != currentMaxHP || _currentMaxHP != currentMaxHP)
                {
                    currentHP = _currentHP;
                    currentMaxHP = _currentMaxHP;

                    UpdateUI();
                }
            }
        }

        void UpdateUI()
        {
            hpText.text = "HP : " + currentHP.ToString() + " / " + currentMaxHP.ToString();

            var value = Calcf.SafetyDiv(currentHP, currentMaxHP, 0.0f);
            statusBar.SetValue(value);
        }
    }
}

