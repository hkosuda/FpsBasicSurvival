using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class HistoryHeader : MonoBehaviour
    {
        static readonly Dictionary<SV_History.Condition, string> conditionNames = new Dictionary<SV_History.Condition, string>()
        {
            { SV_History.Condition.clear, "Game Clear" },
            { SV_History.Condition.dead, "Dead" },
            { SV_History.Condition.timeup, "Time Up" }
        };

        void Start()
        {
            var condition = SV_History.CurrentCondition;

            var text = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            text.text = conditionNames[condition];

            var image = gameObject.GetComponent<Image>();
            
            if (condition == SV_History.Condition.clear)
            {
                image.color = new Color(0.0f, 1.0f, 0.0f, WindowHeader.alpha);
            }

            else
            {
                image.color = new Color(1.0f, 0.0f, 0.0f, WindowHeader.alpha);
            }
        }
    }
}

