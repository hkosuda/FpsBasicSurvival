using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class SVUI_Help : MonoBehaviour
    {
        static TextMeshProUGUI keyActionText;
        static TextMeshProUGUI keyStringText;

        static bool active = true;

        private void Awake()
        {
            keyActionText = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            keyStringText = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();

            active = true;
        }

        void Start()
        {
            UpdateText(null, KeyAction.shot);
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
                Keyconfig.KeyUpdated += UpdateText;
            }

            else
            {
                Keyconfig.KeyUpdated -= UpdateText;
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.H))
            {
                active = !active;
                UpdateText(null, KeyAction.shot);
            }
        }

        static void UpdateText(object obj, KeyAction keyAction)
        {
            if (active)
            {
                var keyActionInfo = "";
                var keyStringInfo = "";

                foreach(var textPair in KeySettingItem.keyText)
                {
                    var keyString = KeySettingItem.CorrectKeyString(Keyconfig.KeybindList[textPair.Key].GetKeyString());

                    keyActionInfo += TxtUtil.C(textPair.Value, Clr.orange) + "\n";
                    keyStringInfo += TxtUtil.C(keyString, Clr.orange) + "\n";
                }

                keyActionInfo += TxtUtil.C("ƒwƒ‹ƒv‚Ì”ñ•\Ž¦", Clr.cyan);
                keyStringInfo += TxtUtil.C("left-alt + h", Clr.cyan);

                keyActionText.text = keyActionInfo;
                keyStringText.text = keyStringInfo;
            }

            else
            {
                keyActionText.text = "";
                keyStringText.text = "";

            }
        }
    }
}

