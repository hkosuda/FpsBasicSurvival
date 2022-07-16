using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SVUI_Money : MonoBehaviour
{
    TextMeshProUGUI moneyText;
    int currentMoney;

    private void Awake()
    {
        moneyText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    void SetEvent(int indicator)
    {
        if (indicator > 0)
        {

        }

        else
        {

        }
    }

    void Update()
    {
        if (SV_StatusAdmin.StatusList == null) { return; }
        if (currentMoney == SV_StatusAdmin.StatusList[SV_Status.money]) { return; }

        currentMoney = SV_StatusAdmin.StatusList[SV_Status.money];

        UpdateText();
    }

    void UpdateText()
    {
        moneyText.text = "$ " + Utility.GetDividedNumberText(currentMoney).ToString();
    }
}
