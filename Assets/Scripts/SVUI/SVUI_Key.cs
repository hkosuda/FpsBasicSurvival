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

