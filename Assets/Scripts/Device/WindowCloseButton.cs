using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class WindowCloseButton : MonoBehaviour
    {
        void Start()
        {
            var button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(DestroyMyself);
        }

        void DestroyMyself()
        {
            Destroy(gameObject);
        }
    }
}

