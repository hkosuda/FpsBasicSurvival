using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class ConsoleInputField : MonoBehaviour
    {
        static public EventHandler<string> ValueUpdated { get; set; }

        static TMP_InputField inputField;

        private void Awake()
        {
            inputField = gameObject.GetComponent<TMP_InputField>();
            inputField.onValueChanged.AddListener(OnValueUpdatedMethod);
        }

        static void OnValueUpdatedMethod(string value)
        {
            ValueUpdated?.Invoke(null, value);
        }

        static public void RequestCommand()
        {
            var value = inputField.text;
            if (value.Trim() == "") { return; }

            CommandReceiver.RequestCommand(value, null);
            inputField.text = "";
        }

        void Start()
        {
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
                CommandReceiver.RequestEnd += UpdateInputFieldOnRequestEnd;
            }

            else
            {
                CommandReceiver.RequestEnd -= UpdateInputFieldOnRequestEnd;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                RequestCommand();
            }

            inputField.selectionAnchorPosition = inputField.selectionFocusPosition;
        }

        static void UpdateInputFieldOnUnknownCommand(object obj, string sentence)
        {
            UpdateInputField(false);
        }

        static void UpdateInputFieldOnRequestEnd(object obj, Tracer tracer)
        {
            UpdateInputField(tracer.NoError);
        }

        static void UpdateInputField(bool noError)
        {
            if (noError)
            {
                inputField.text = "";
            }

            Activate();
        }

        static public void Activate()
        {
            if (inputField == null) { return; }
            inputField.ActivateInputField();
        }

        static public void AddValue(string value)
        {
            var currentValue = inputField.text;

            var values = CommandReceiver.GetValues(currentValue);
            var options = CommandReceiver.GetOptions(currentValue);

            if (values == null || values.Count == 0) { inputField.text = ""; return; }

            string corrected;

            // pre processing
            if (currentValue.EndsWith(" "))
            {
                corrected = CorrectValues(values);
            }

            else
            {
                corrected = CorrectValues(values, true);
            }

            // add
            if (values.Last().StartsWith("\""))
            {
                corrected += "\"" + value;
            }

            else
            {
                corrected += value;
            }

            corrected = AddOptions(corrected, options);
            inputField.text = corrected.TrimStart();
            inputField.caretPosition = inputField.text.Length;

            // - inner function
            static string CorrectValues(List<string> values, bool offset = false)
            {
                if (values == null || values.Count == 0) { return ""; }

                var text = "";

                if (offset)
                {
                    for (var n = 0; n < values.Count - 1; n++)
                    {
                        text += values[n] + " ";
                    }
                }

                else
                {
                    for (var n = 0; n < values.Count; n++)
                    {
                        text += values[n] + " ";
                    }
                }

                return text.TrimEnd() + " ";
            }

            static string AddOptions(string corrected, List<string> options)
            {
                corrected = corrected.TrimEnd() + " ";

                if (options == null || options.Count == 0)
                {
                    return corrected;
                }

                else
                {
                    return corrected + options.Last() + " ";
                }
            }
        }
    }
}

