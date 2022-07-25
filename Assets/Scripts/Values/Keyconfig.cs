using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public enum KeyAction
    {
        none,

        jump, autoJump, shot, reload, crouch, check,
        forward, backward, right, left,
        menu, console, information,
        ak, de, m9,
    }

    public class Keyconfig : MonoBehaviour
    {
        static public EventHandler<KeyAction> KeyUpdated { get; set; }

        static readonly Dictionary<KeyAction, Key> defaultKeybindList = new Dictionary<KeyAction, Key>()
        {
            { KeyAction.jump, new Key(KeyCode.Space) },
            { KeyAction.autoJump, new Key(KeyCode.Mouse1) },
            { KeyAction.forward, new Key(KeyCode.W) },
            { KeyAction.backward, new Key(KeyCode.S) },
            { KeyAction.right, new Key(KeyCode.D) },
            { KeyAction.left, new Key(KeyCode.A) },
            { KeyAction.shot, new Key(KeyCode.Mouse0) },
            { KeyAction.reload, new Key(KeyCode.R) },
            { KeyAction.crouch, new Key(KeyCode.LeftShift) },
            { KeyAction.check, new Key(KeyCode.F) },
            { KeyAction.menu, new Key(KeyCode.M) },
            { KeyAction.console, new Key(KeyCode.K) },
            { KeyAction.information, new Key(KeyCode.Tab) },
            { KeyAction.ak, new Key(KeyCode.None, 1) },
            { KeyAction.de, new Key(KeyCode.None, -1) },
            { KeyAction.m9, new Key(KeyCode.Q) },
        };

        static public Dictionary<KeyAction, Key> KeybindList { get; private set; } = new Dictionary<KeyAction, Key>(defaultKeybindList);

        static public void SetKey(KeyAction keyAction, KeyCode keyCode = KeyCode.None, float wheelDelta = 0.0f)
        {
            if (keyCode == KeyCode.None)
            {
                if (wheelDelta == 0.0f)
                {
                    KeybindList[keyAction].keyCode = KeyCode.None;
                    KeybindList[keyAction].wheelDelta = 1.0f;
                }

                else
                {
                    KeybindList[keyAction].keyCode = KeyCode.None;
                    KeybindList[keyAction].wheelDelta = wheelDelta;
                }
            }

            else
            {
                KeybindList[keyAction].keyCode = keyCode;
                KeybindList[keyAction].wheelDelta = 0.0f;
            }

            KeyUpdated?.Invoke(null, keyAction);
        }

        static public bool CheckInput(KeyAction action, bool getKeyDown)
        {
            //if (action != KeyAction.menu && action != KeyAction.console)
            //{
            //    if (TimerManager.CurrentTimer.Name != Timer.TimerName.gameTimer || TimerSystem.Paused) 
            //    {
            //        return false;
            //    }
            //}

            var key = KeybindList[action];

            if (key.keyCode != KeyCode.None)
            {
                if (getKeyDown)
                {
                    if (Input.GetKeyDown(key.keyCode))
                    {
                        return true;
                    }
                }

                else
                {
                    if (Input.GetKey(key.keyCode))
                    {
                        return true;
                    }
                }
            }

            // keybind.keyCode == KeyCode.None
            else
            {
                float wheelDelta = key.wheelDelta;

                if (wheelDelta > 0)
                {
                    if (Input.mouseScrollDelta.y > 0)
                    {
                        return true;
                    }
                }

                else
                {
                    if (Input.mouseScrollDelta.y < 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // for only keycheck command
        //private void Update()
        //{
        //if (!KeycheckCommand.Active) { return; }
        //if (!Input.anyKeyDown && Input.mouseScrollDelta.y == 0.0f) { return; }

        //foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        //{
        //    if (Input.GetKeyDown(keyCode))
        //    {
        //        KeycheckCommand.EchoInputKey(keyCode.ToString());
        //    }
        //}

        //if (Input.mouseScrollDelta.y > 0)
        //{
        //    KeycheckCommand.EchoInputKey("1");
        //}

        //if (Input.mouseScrollDelta.y < 0)
        //{
        //    KeycheckCommand.EchoInputKey("-1");
        //}
        //}

        public class Key
        {
            public KeyCode keyCode;
            public float wheelDelta;

            public Key(KeyCode keyCode, float wheelDelta = 1.0f)
            {
                this.keyCode = keyCode;
                this.wheelDelta = wheelDelta;
            }

            public string GetKeyString()
            {
                if (keyCode == KeyCode.None)
                {
                    if (wheelDelta > 0) { return "+wheel"; }
                    return "-wheel";
                }

                else
                {
                    return keyCode.ToString().ToLower();
                }
            }

            static public Key StringToKey(string str)
            {
                str = str.ToLower();

                if (int.TryParse(str, out var num))
                {
                    if (num > 0)
                    {
                        return new Key(KeyCode.None, 1);
                    }

                    else if (num < 0)
                    {
                        return new Key(KeyCode.None, -1);
                    }

                    else
                    {
                        return null;
                    }
                }

                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (keyCode == KeyCode.None) { continue; }

                    if (str == keyCode.ToString().ToLower())
                    {
                        return new Key(keyCode);
                    }
                }

                return null;
            }
        }
    }
}

