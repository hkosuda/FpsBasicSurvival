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
            { KeyAction.jump, "�W�����v" },
            { KeyAction.autoJump, "�I�[�g�W�����v" },
            { KeyAction.shot, "�ˌ�" },
            { KeyAction.reload , "�����[�h" },
            { KeyAction.crouch, "���Ⴊ��" },
            { KeyAction.check, "���ׂ�" },
            { KeyAction.forward, "�O�i" },
            { KeyAction.backward, "���" },
            { KeyAction.right, "�E�Ɉړ�" },
            { KeyAction.left, "���Ɉړ�" },
            { KeyAction.menu, "���j���[���J��" },
            { KeyAction.console, "�R���\�[�����J��" },
            { KeyAction.ak, "���C���E�F�|��" },
            { KeyAction.de, "�T�u�E�F�|��" },
            { KeyAction.m9, "�ߐڕ���" },
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

