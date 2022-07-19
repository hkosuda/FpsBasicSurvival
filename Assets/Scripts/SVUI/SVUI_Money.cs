using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class SVUI_Money : MonoBehaviour
    {
        static readonly float addMoneyExistTime = 2.0f;

        static TextMeshProUGUI currentMoneyText;
        static TextMeshProUGUI addMoneyText;

        static int currentMoney;

        static int addMoney;
        static float addMoneyExistTimeRemain;

        private void Awake()
        {
            currentMoneyText = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            addMoneyText = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
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
                TimerSystem.Updated += UpdateMethod;
                SV_Status.PlayerGotMoney += AddMoney;
            }

            else
            {
                TimerSystem.Updated -= UpdateMethod;
                SV_Status.PlayerGotMoney -= AddMoney;
            }
        }

        static void AddMoney(object obj, int add)
        {
            addMoneyText.gameObject.SetActive(true);

            addMoney += add;
            addMoneyText.text = "+ $ " + addMoney.ToString("#,0");

            addMoneyExistTimeRemain = addMoneyExistTime;
        }

        static void UpdateMethod(object obj, float dt)
        {
            addMoneyExistTimeRemain -= dt;

            if (addMoneyExistTimeRemain < 0)
            {
                addMoneyText.gameObject.SetActive(false);
                addMoney = 0;

                if (SV_Status.StatusList != null)
                {
                    if (currentMoney != SV_Status.StatusList[Status.money])
                    {
                        currentMoney = SV_Status.StatusList[Status.money];

                        UpdateText();
                    }
                }
            }
        }

        static void UpdateText()
        {
            currentMoneyText.text = "$ " + currentMoney.ToString("#,0");
        }
    }
}

