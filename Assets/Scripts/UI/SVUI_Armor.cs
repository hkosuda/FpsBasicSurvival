using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SVUI_Armor : MonoBehaviour
{
    TextMeshProUGUI armorText;
    StatusBar statusBar;

    int currentArmor;
    int currentMaxArmor;

    void Start()
    {
        armorText = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        statusBar = gameObject.transform.GetChild(1).gameObject.GetComponent<StatusBar>();
    }

    void Update()
    {
        if (SV_StatusAdmin.StatusList != null)
        {
            var _currentArmor = SV_StatusAdmin.StatusList[SV_Status.armor];
            var _currentMaxArmor = SV_StatusAdmin.CurrentMaxArmor;

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
        armorText.text = "Armor : " + currentArmor.ToString() + " / " + currentMaxArmor.ToString();

        var value = Utility.SafetyDivision(currentArmor, currentMaxArmor, 0.0f);
        statusBar.SetValue(value);
    }
}
