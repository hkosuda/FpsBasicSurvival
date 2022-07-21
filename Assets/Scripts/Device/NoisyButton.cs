using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class NoisyButton : MonoBehaviour
    {
        static public EventHandler<bool> Clicked { get; set; }

        private void Start()
        {
            var button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(InvokeClicked);
        }

        static void InvokeClicked()
        {
            Clicked?.Invoke(null, false);
        }
    }
}

