using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class SVUI_Key : MonoBehaviour
    {
        static int currentKey;
        static int currentRequireKey;

        static HostName currentHost;

        static TextMeshProUGUI text;

        void Start()
        {
            text = gameObject.GetComponent<TextMeshProUGUI>();
            UpdateText();
        }

        void Update()
        {
            if (SV_Round.RoundNumber == 0)
            {
                if (currentKey != 0 || currentRequireKey != 0)
                {
                    currentKey = 0;
                    currentRequireKey = 0;

                    UpdateText();
                }
            }

            if (currentKey != SV_Round.CurrentKey || currentRequireKey != SvParams.GetInt(SvParam.require_keys))
            {
                currentKey = SV_Round.CurrentKey;
                currentRequireKey = SvParams.GetInt(SvParam.require_keys);

                UpdateText();
            }

            if (currentHost != GameSystem.CurrentHost.HostName)
            {
                currentHost = GameSystem.CurrentHost.HostName;
                UpdateText();
            }
        }

        static void UpdateText()
        {
            if (currentHost != HostName.survival)
            {
                text.text = "";
            }

            if (SV_Round.RoundNumber == 0)
            {
                text.text = TxtUtil.C("Key : 0 / 0", Clr.lime);
                return;
            }

            var info = "Key : " + currentKey.ToString() + " / " + currentRequireKey.ToString();

            if (currentKey < currentRequireKey)
            {
                text.text = TxtUtil.C(info, Clr.red);
            }

            else
            {
                text.text = TxtUtil.C(info, Clr.lime);
            }
            
        }
    }
}

