using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class ConsoleButtonGroup : MonoBehaviour
    {
        void Start()
        {
            var closeButton = gameObject.transform.GetChild(0).gameObject.GetComponent<Button>();
            closeButton.onClick.AddListener(Console.CloseConsole);

            var returnButton = gameObject.transform.GetChild(1).gameObject.GetComponent<Button>();
            returnButton.onClick.AddListener(ConsoleInputField.RequestCommand);
        }
    }
}

