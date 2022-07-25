using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class TabInfoContent : MonoBehaviour
    {
        static TextMeshProUGUI playerName;

        static TextMeshProUGUI destroyed;
        static TextMeshProUGUI givenDamage;
        static TextMeshProUGUI gotMoney;

        static TextMeshProUGUI damageRate;
        static TextMeshProUGUI moneyRate;
        static TextMeshProUGUI movingSpeed;
        static TextMeshProUGUI weaponSpeed;
        static TextMeshProUGUI firingRate;

        private void Awake()
        {
            playerName = GetText(0);
            // spacer (1)
            destroyed = GetText(2);
            givenDamage = GetText(3);
            gotMoney = GetText(4);
            // spacer (5)
            damageRate = GetText(6);
            moneyRate = GetText(7);
            movingSpeed = GetText(8);
            weaponSpeed = GetText(9);
            firingRate = GetText(10);

            // - inner function
            TextMeshProUGUI GetText(int n)
            {
                return gameObject.transform.GetChild(n).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            }
        }

        void Start()
        {
            UpdateContent(null, false);
            SetEvent(1);
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                SV_History.HistoryUpdated += UpdateContent;
            }

            else
            {
                SV_History.HistoryUpdated -= UpdateContent;
            }
        }

        static public void UpdateContent(object obj, bool mute)
        {
            if (playerName == null) { return; }
            if (!TabInfo.Active) { return; }

            playerName.text = "Player1";

            var totalValue = HistoryContentManager.TotalValue(SV_History.HistoryList);

            if (totalValue == null)
            {
                destroyed.text = "0";
                givenDamage.text = "0";
                gotMoney.text = "0";

                var extension = " %";

                damageRate.text = "100" + extension;
                moneyRate.text = "100" + extension;
                movingSpeed.text = "100" + extension;
                weaponSpeed.text = "100" + extension;
                firingRate.text = "100" + extension;
            }

            else
            {
                destroyed.text = ((int)totalValue.valueList[SV_History.HistoryValue.destroyed]).ToString();
                givenDamage.text = ((int)totalValue.valueList[SV_History.HistoryValue.givenDamage]).ToString();
                gotMoney.text = ((int)totalValue.valueList[SV_History.HistoryValue.gotMoney]).ToString();

                var extension = " %";

                damageRate.text = SV_Status.CurrentDamageRate.ToString() + extension;
                moneyRate.text = SV_Status.CurrentMoneyRate.ToString() + extension;
                movingSpeed.text = SV_Status.MovingSpeedRate.ToString() + extension;
                weaponSpeed.text = SV_Status.WeaponSpeedRate.ToString() + extension;
                firingRate.text = SV_Status.FiringSpeedRate.ToString() + extension;
            }
            
        }
    }
}

