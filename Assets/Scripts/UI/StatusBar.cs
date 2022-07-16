using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class StatusBar : MonoBehaviour
    {
        static readonly int barWidth = 170;
        static readonly int barHeight = 25;

        static readonly int innerLineWidth = 2;
        static readonly int outerLineWidth = 3;

        RectTransform myRect;
        List<RectTransform> rects;

        float value;

        void Awake()
        {
            value = 1.0f;

            myRect = gameObject.GetComponent<RectTransform>();
            myRect.sizeDelta = new Vector2(barWidth, barHeight);

            rects = new List<RectTransform>()
        {
            GetRect(0), // main
            GetRect(1), // u
            GetRect(2), // b
            GetRect(3), // l
            GetRect(4), // r
        };

            UpdateBarSize();
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
            var mainBarWidth = barWidth - 2 * (outerLineWidth + innerLineWidth);
            var currentBarWidth = mainBarWidth * value;
            var mainBarHeight = barHeight - 2 * (outerLineWidth + innerLineWidth);

            rects[0].sizeDelta = new Vector2(currentBarWidth, mainBarHeight);
        }

        void UpdateBarSize()
        {
            myRect.sizeDelta = new Vector2(barWidth, barHeight);

            SetMainBarSize();

            rects[1].sizeDelta = new Vector2(0, outerLineWidth);
            rects[2].sizeDelta = new Vector2(0, outerLineWidth);
            rects[3].sizeDelta = new Vector2(outerLineWidth, 0);
            rects[4].sizeDelta = new Vector2(outerLineWidth, 0);
        }
    }
}