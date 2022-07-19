using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class SVUI_Armor : MonoBehaviour
    {
        static TextMeshProUGUI armorText;
        static StatusBar statusBar;

        static int currentArmor;
        static int currentMaxArmor;

        void Start()
        {
            armorText = gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            statusBar = gameObject.transform.GetChild(1).gameObject.GetComponent<StatusBar>();
        }

        void Update()
        {
            if (SV_Status.StatusList != null)
            {
                var _currentArmor = SV_Status.StatusList[Status.armor];
                var _currentMaxArmor = SV_Status.CurrentMaxArmor;

                if (_currentArmor != currentMaxArmor || _currentMaxArmor != currentMaxArmor)
                {
                    currentArmor = _currentArmor;
                    currentMaxArmor = _currentMaxArmor;

                    UpdateUI();
                }
            }
        }

        void UpdateUI()
        {
            armorText.text = currentArmor.ToString() + " / " + currentMaxArmor.ToString();

            var value = Calcf.SafetyDiv(currentArmor, currentMaxArmor, 0.0f);
            value = Calcf.Clip(0.0f, 1.0f, value);

            statusBar.SetValue(value);
        }
    }
}

