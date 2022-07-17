using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
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
            if (SV_Status.StatusList == null) { return; }
            if (currentMoney == SV_Status.StatusList[Status.money]) { return; }

            currentMoney = SV_Status.StatusList[Status.money];

            UpdateText();
        }

        void UpdateText()
        {
            moneyText.text = "$ " + currentMoney.ToString("#,0");
        }
    }
}

