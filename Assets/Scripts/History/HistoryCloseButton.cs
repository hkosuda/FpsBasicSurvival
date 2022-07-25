using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class HistoryCloseButton : MonoBehaviour
    {
        private void Start()
        {
            var button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(BeginConfirmation);
        }

        static void BeginConfirmation()
        {
            Confirmation.BeginConfirmation("�I�����܂����H", ResetGame, null);
        }

        static void ResetGame()
        {
            GameSystem.SwitchHost(HostName.survival);
        }
    }
}

