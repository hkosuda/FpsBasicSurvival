using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    RectTransform myRect;
    List<RectTransform> rects;

    float value;

    void Awake()
    {
        value = 1.0f;

        myRect = gameObject.GetComponent<RectTransform>();
        myRect.sizeDelta = new Vector2(Ints.Get(Ints.Item.sv_ui_bar_width), Ints.Get(Ints.Item.sv_ui_bar_height));

        rects = new List<RectTransform>()
        {
            GetRect(0), // main
            GetRect(1), // u
            GetRect(2), // b
            GetRect(3), // l
            GetRect(4), // r
        };

        UpdateBarSize(null, Ints.Item.sv_ui_bar_width);
    }

    private void Start()
    {
        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            IgSetting.Updated += UpdateBarSize;
        }

        else
        {
            IgSetting.Updated -= UpdateBarSize;
        }
    }

    RectTransform GetRect(int n)
    {
        return gameObject.transform.GetChild(n).gameObject.GetComponent<RectTransform>();
    }

    public void SetColor(Color color)
    {
        foreach (var rect in rects)
        {
            rect.gameObject.GetComponent<Image>().color = color;
        }
    }

    public void SetValue(float value)
    {
        this.value = value;

        if (this.value < 0)
        {
            this.value = 0;
        }

        if (this.value > 1)
        {
            this.value = 1;
        }

        SetMainBarSize();
    }

    void SetMainBarSize()
    {
        var width = Ints.Get(Ints.Item.sv_ui_bar_width);
        var height = Ints.Get(Ints.Item.sv_ui_bar_height);
        var innerLineWidth = Ints.Get(Ints.Item.sv_ui_bar_inner_line_width);
        var outerLineWidth = Ints.Get(Ints.Item.sv_ui_bar_outer_line_width);

        var mainBarWidth = width - 2 * (outerLineWidth + innerLineWidth);
        var currentBarWidth = mainBarWidth * value;
        var mainBarHeight = height - 2 * (outerLineWidth + innerLineWidth);

        rects[0].sizeDelta = new Vector2(currentBarWidth, mainBarHeight);
    }

    void UpdateBarSize(object obj, Ints.Item item)
    {
        var width = Ints.Get(Ints.Item.sv_ui_bar_width);
        var height = Ints.Get(Ints.Item.sv_ui_bar_height);
        var outerLineWidth = Ints.Get(Ints.Item.sv_ui_bar_outer_line_width);

        myRect.sizeDelta = new Vector2(width, height);

        SetMainBarSize();

        rects[1].sizeDelta = new Vector2(0, outerLineWidth);
        rects[2].sizeDelta = new Vector2(0, outerLineWidth);
        rects[3].sizeDelta = new Vector2(outerLineWidth, 0);
        rects[4].sizeDelta = new Vector2(outerLineWidth, 0);
    }
}
