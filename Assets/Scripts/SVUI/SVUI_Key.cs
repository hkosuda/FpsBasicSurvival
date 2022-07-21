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

        static TextMeshProUGUI text;

        void Start()
        {
            text = gameObject.GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            if (currentKey != SV_Round.CurrentKey || currentRequireKey != SvParams.GetInt(SvParam.require_keys))
            {
                currentKey = SV_Round.CurrentKey;
                currentRequireKey = SvParams.GetInt(SvParam.require_keys);

                UpdateText();
            }
        }

        static void UpdateText()
        {
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

