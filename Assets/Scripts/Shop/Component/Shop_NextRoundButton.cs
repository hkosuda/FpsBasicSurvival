using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class Shop_NextRoundButton : MonoBehaviour
    {
        void Start()
        {
            var button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(CancelAfterConfirmation);
        }

        void CancelAfterConfirmation()
        {
            Confirmation.BeginConfirmation("次のラウンドに進みますか？", SvHost.BeginNext, null);
        }
    }
}

