using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class Suggest : MonoBehaviour
    {
        static public Dictionary<string, GameObject> SuggestButtons { get; private set; } = new Dictionary<string, GameObject>();
        static public List<GameObject> ActiveButtons { get; private set; } = new List<GameObject>();

        static GameObject _suggestButton;

        static GameObject suggestView;
        static GameObject suggestContent;
        static GameObject outlineGroup;

        static readonly int buttonHeight = 20;
        static readonly int buttonOffset = 10;

        private void Awake()
        {
            InitialSetup(gameObject);
            ShowSuggest(null, "");
        }

        private void Start()
        {
            SetEvent(1);
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        void Update()
        {
            Apply();
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                ConsoleInputField.ValueUpdated += ShowSuggest;
            }

            else
            {
                ConsoleInputField.ValueUpdated -= ShowSuggest;
            }
        }

        static void InitialSetup(GameObject gameObject)
        {
            suggestView = gameObject.transform.GetChild(0).gameObject;

            suggestContent = gameObject.transform.GetChild(0).GetChild(0).gameObject;
            suggestContent.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 0, 0);

            outlineGroup = gameObject.transform.GetChild(1).gameObject;
        }

        static void ShowSuggest(object obj, string content)
        {
            InactivateAllButtons();

            content = CommandReceiver.Grouping(content);
            content = RemoveBeforeOfQuatation(content);

            if (Regex.IsMatch(content, @"\A.* +-\w*\z"))
            {
                ActivateSuggestButtons(new List<string>());
                return;
            }

            var command = GetCommand(content);

            if (command == null)
            {
                var suggestCommands = GetSuggestCommands(content);
                ActivateSuggestButtons(suggestCommands);
            }

            else
            {
                var suggestValues = GetSuggestValues(command, content);
                ActivateSuggestButtons(suggestValues);
            }

            // - inner function
            static string RemoveBeforeOfQuatation(string content)
            {
                if (!Regex.IsMatch(content, @"\A.*\"".*\z"))
                {
                    return content;
                }

                var match = Regex.Match(content, @"\""[^\""]*\z");
                return Regex.Replace(match.Value, @"\""", "");
            }

            // - inner function
            static Command GetCommand(string content)
            {
                content = content.TrimStart();

                foreach (var command in CommandReceiver.CommandList)
                {
                    if (content.StartsWith(command.commandName + " "))
                    {
                        return command;
                    }
                }

                return null;
            }

            // - inner function
            static List<string> GetSuggestValues(Command command, string content)
            {
                var suggestValues = new List<string>();

                var values = CommandReceiver.GetValues(content);
                var availableValues = command.AvailableValues(values);

                if (availableValues == null || availableValues.Count == 0) { return suggestValues; }

                var lastValue = values.Last();

                if (availableValues.Contains(lastValue) && content.EndsWith(" "))
                {
                    values.Add("$");
                    return command.AvailableValues(values);
                }

                if (content.EndsWith(" "))
                {
                    return availableValues;
                }

                foreach (var value in availableValues)
                {
                    if (value.StartsWith(lastValue))
                    {
                        suggestValues.Add(value);
                    }
                }

                return suggestValues;
            }

            // function
            static List<string> GetSuggestCommands(string content)
            {
                content = content.TrimStart();

                var suggestCommands = new List<string>();
                if (content == "") { return suggestCommands; }

                foreach (var command in CommandReceiver.CommandList)
                {
                    if (command.commandName.StartsWith(content))
                    {
                        suggestCommands.Add(command.commandName);
                    }
                }

                return suggestCommands;
            }

            // function
            static void InactivateAllButtons()
            {
                foreach (var button in SuggestButtons.Values)
                {
                    button.SetActive(false);
                }
            }

            // function
            static void ActivateSuggestButtons(List<string> suggestCommands)
            {
                if (_suggestButton == null) { _suggestButton = Resources.Load<GameObject>("UiComponent/SuggestButton"); }
                if (suggestCommands == null) { return; }

                ActiveButtons = new List<GameObject>();

                foreach (var suggestCommand in suggestCommands)
                {
                    if (SuggestButtons.ContainsKey(suggestCommand))
                    {
                        SuggestButtons[suggestCommand].SetActive(true);
                    }

                    else
                    {
                        var suggestButton = Instantiate(_suggestButton);

                        suggestButton.transform.SetParent(suggestContent.transform);
                        suggestButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = suggestCommand;

                        SuggestButtons.Add(suggestCommand, suggestButton);
                    }

                    ActiveButtons.Add(SuggestButtons[suggestCommand]);
                }
            }
        }

        static void Apply()
        {
            if (ActiveButtons == null) { return; }
            if (ActiveButtons.Count == 0) { return; }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                var button = ActiveButtons[0];
                button.GetComponent<Button>().onClick?.Invoke();
            }
        }
    }
}

