using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class SuggestDescription : MonoBehaviour
    {
        static TextMeshProUGUI descriptionText;

        private void Awake()
        {
            descriptionText = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
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
                ConsoleInputField.ValueUpdated += ShowDescription;
            }

            else
            {
                ConsoleInputField.ValueUpdated += ShowDescription;
            }
        }

        static void ShowDescription(object obj, string value)
        {
            var command = GetCommand(value);
            if (command == null) { return; }

            descriptionText.text = GetDescription(command, value);

            // - inner function
            static Command GetCommand(string value)
            {
                value = value.TrimStart();

                foreach (var command in CommandReceiver.CommandList)
                {
                    if (value.StartsWith(command.commandName + " "))
                    {
                        return command;
                    }
                }

                return null;
            }

            // - inner function
            static string GetDescription(Command command, string value)
            {
                var description = "";
                var values = CommandReceiver.GetValues(value);

                description = AddLine(description, "[" + command.commandName + "]", true);
                description = AddLine(description, "---------------------------------------------------------", false);


                if (command.description != "")
                {
                    description = AddLine(description, "äTóv", true);
                    description = AddLine(description, command.description);
                }

                if (command.detail != "")
                {
                    description = AddLine(description, "è⁄ç◊", true);
                    description = AddLine(description, command.detail);
                }

                return description;

                static string AddLine(string description, string content, bool color = false)
                {
                    if (color) { return description + TxtUtil.C(content, Clr.lime) + "\n"; }
                    return description + content + "\n";
                }
            }
        }
    }
}

