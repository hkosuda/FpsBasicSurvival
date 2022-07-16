using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItemThemeManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image image;

    void Awake()
    {
        image = gameObject.GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(0.2f, 0.2f, 0.2f, 1.0f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = new Color(1, 1, 1, 0);
    }
}
