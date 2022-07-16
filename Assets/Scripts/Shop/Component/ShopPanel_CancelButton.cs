using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel_CancelButton : MonoBehaviour
{
    void Start()
    {
        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(CancelAfterConfirmation);
    }

    void CancelAfterConfirmation()
    {
        Confirmation.BeginConfirmation("変更をすべて破棄しますか？", SV_ShopAdmin.Initialize, null);
    }
}
