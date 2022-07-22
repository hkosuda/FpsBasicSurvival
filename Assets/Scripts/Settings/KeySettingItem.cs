using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class KeySettingItem : MonoBehaviour
    {
        static readonly Dictionary<KeyAction, string> keyText = new Dictionary<KeyAction, string>()
        {
            { KeyAction.jump, "ジャンプ" },
            { KeyAction.autoJump, "オートジャンプ" },
            { KeyAction.shot, "射撃" },
            { KeyAction.reload , "リロード" },
            { KeyAction.crouch, "しゃがむ" },
            { KeyAction.check, "調べる" },
            { KeyAction.forward, "前進" },
            { KeyAction.backward, "後退" },
            { KeyAction.right, "右に移動" },
            { KeyAction.left, "左に移動" },
            { KeyAction.menu, "メニューを開く" },
            { KeyAction.console, "コンソールを開く" },
            { KeyAction.ak, "メインウェポン" },
            { KeyAction.de, "サブウェポン" },
            { KeyAction.m9, "近接武器" },
        };

        static public EventHandler<KeyAction> SettingBegin { get; set; }
        static public EventHandler<KeyAction> SettingEnd { get; set; }

        KeyAction keyAction;

        TextMeshProUGUI titleText;
        TextMeshProUGUI buttonText;

        Button button;

        private void Awake()
        {
            titleText = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            button = gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();
            buttonText = button.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            button.onClick.AddListener(BeginKeySetting);
        }

        void Start()
        {
            UpdateContent();
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
                SettingBegin += InactivateButton;
                SettingEnd += ActivateButton;
            }

            else
            {
                SettingBegin -= InactivateButton;
                SettingEnd -= ActivateButton;
            }
        }

        void BeginKeySetting()
        {
            SettingBegin?.Invoke(null, keyAction);
        }

        void InactivateButton(object obj, KeyAction keyAction)
        {
            button.interactable = false;
        }

        void ActivateButton(object obj, KeyAction keyAction)
        {
            button.interactable = true;

            UpdateContent();
        }

        void UpdateContent()
        {
            var key = Keyconfig.KeybindList[keyAction];

            titleText.text = keyText[keyAction];
            buttonText.text = key.GetKeyString();
        }

        public void Initialize(KeyAction keyAction)
        {
            this.keyAction = keyAction;

            UpdateContent();
        }
    }
}

