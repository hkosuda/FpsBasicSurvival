using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel_OkButton : MonoBehaviour
{
    void Start()
    {
        var button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(CancelAfterConfirmation);
    }

    void CancelAfterConfirmation()
    {
        Confirmation.BeginConfirmation("�A�b�v�O���[�g���s���C���̃��E���h�ɐi�݂܂����H", ShopPanel.DestroyShopPanel, null);
    }
}
