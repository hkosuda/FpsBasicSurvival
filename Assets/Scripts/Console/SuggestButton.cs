using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class SuggestButton : MonoBehaviour
    {
        void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(OnClickMethod);
        }

        void OnClickMethod()
        {
            var value = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
            ConsoleInputField.AddValue(value + " ");
        }
    }
}

