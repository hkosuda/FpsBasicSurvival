using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class HistoryContentManager : MonoBehaviour
    {
        static readonly string separator = "----------";

        static readonly Dictionary<SV_History.HistoryValue, string> titleList = new Dictionary<SV_History.HistoryValue, string>()
        {
            { SV_History.HistoryValue.movingDistance, "移動距離" },
            { SV_History.HistoryValue.shotAmmo, "消費弾数" },
            { SV_History.HistoryValue.takenDamage, "被ダメージ" },
        };

        static readonly List<SV_History.HistoryValue> floatValues = new List<SV_History.HistoryValue>() 
        {
            SV_History.HistoryValue.movingDistance 
        };

        static GameObject _credit;
        static GameObject _info;

        static Transform myself;

        private void Awake()
        {
            if (_credit == null) { _credit = Resources.Load<GameObject>("UiComponent/Credit"); }
            if (_info == null) { _info = Resources.Load<GameObject>("UiComponent/HistoryInfo"); }

            myself = gameObject.transform;
        }

        void Start()
        {
            ShowInfo();
        }

        static void ShowInfo()
        {
            var historyList = SV_History.HistoryList;

            var total = TotalValue(historyList);

            if (SV_History.CurrentCondition != SV_History.Condition.clear)
            {
                WriteInfo("クリアしたラウンド数", 0);
                WriteInfo((SV_Round.RoundNumber - 1).ToString(), 1);
            }

            WriteInfo(separator + " 累計情報 " + separator, 0, Clr.lime);
            WriteAllInfo(total, 1, true, true);
            
            for(var n = 0; n < historyList.Count; n++)
            {
                WriteInfo(separator + " Round " + n.ToString() + " " + separator, 0, Clr.lime);

                if (n == 0)
                {
                    WriteAllInfo(historyList[n], 1, false, true);
                }

                else if (n == historyList.Count - 1)
                {
                    WriteAllInfo(historyList[n], 1, true, false);
                }

                else
                {
                    WriteAllInfo(historyList[n], 1, true, true);
                }
            }
        }

        static SV_History.History TotalValue(List<SV_History.History> historyList)
        {
            var totalValue = new SV_History.History();

            if (historyList != null)
            {
                foreach(var history in historyList)
                {
                    foreach(var v in history.valueList)
                    {
                        totalValue.valueList[v.Key] += v.Value;
                    }

                    foreach(var buy in history.buyList)
                    {
                        totalValue.buyList[buy.Key] += buy.Value;
                    }
                }
            }

            return totalValue;
        }

        static void WriteAllInfo(SV_History.History history, int indent, bool writeValues, bool writeBuy)
        {
            if (writeValues)
            {
                foreach (var title in titleList)
                {
                    WriteInfo(title.Value, indent);

                    if (floatValues.Contains(title.Key))
                    {
                        WriteInfo(history.valueList[title.Key].ToString("F2"), indent + 1);
                    }

                    else
                    {
                        WriteInfo(history.valueList[title.Key].ToString("#,0"), indent + 1);
                    }
                }
            }
            
            if (writeBuy)
            {
                WriteInfo("購入情報", indent);

                var buyInfo = "";

                foreach(var buy in history.buyList)
                {
                    if (buy.Value > 0)
                    {
                        buyInfo += ShopItemButton.itemNames[buy.Key] + "\t : " + buy.ToString() + "\n";
                    }
                }

                if (buyInfo == "")
                {
                    WriteInfo("なし", indent + 1);
                }

                else
                {
                    WriteInfo(buyInfo.TrimEnd(new char[] { '\n' }), indent + 1);
                }
            }
        }

        static void WriteInfo(string content, int indent, Clr c = Clr.white, bool center = true)
        {
            var info = Instantiate(_info);
            info.transform.SetParent(myself);

            var text = info.GetComponent<TextMeshProUGUI>();
            text.text = TxtUtil.C(content, c);

            if (center)
            {
                text.alignment = TextAlignmentOptions.Center;
            }

            else
            {
                text.margin = new Vector4(indent * 10 + 3, 0, 0, 0);
            }
        }
    }
}

