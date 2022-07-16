using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShopPanelLayoutManager : MonoBehaviour
    {
        static RectTransform mainPanel;
        static RectTransform itemWindowHead;
        static RectTransform itemWindow;
        static RectTransform messageWindow;
        static RectTransform remainMoneyWindow;
        static RectTransform cancelButton;
        static RectTransform okButton;

        static float gap = 10.0f;

        static float PanelHeight { get; set; }
        static float PanelWidth { get; set; }

        // component size
        static float ItemWindowWidth { get; } = 780;
        static float ItemWindowHeight { get; } = 280;
        static float TitleHeight { get; } = 40;
        static float ComponentHeight { get; } = 30;
        static float ButtonWidth { get; } = 160;

        static float panelHeight;
        static float panelWidth;

        private void Awake()
        {
            mainPanel = gameObject.GetComponent<RectTransform>();

            itemWindowHead = GetRect(1);
            itemWindow = GetRect(2);
            messageWindow = GetRect(3);
            remainMoneyWindow = GetRect(4);
            cancelButton = GetRect(5);
            okButton = GetRect(6);

            // - inner function
            RectTransform GetRect(int n)
            {
                return gameObject.transform.GetChild(n).gameObject.GetComponent<RectTransform>();
            }
        }

        void Update()
        {
            SizeCheck();
        }

        static void SizeCheck()
        {
            PanelHeight = TitleHeight + 4 * gap + 4 * ComponentHeight + ItemWindowHeight;
            PanelWidth = 2 * gap + ItemWindowWidth;

            var _panelWidth = (float)Screen.width;
            var _panelHeight = (float)Screen.height;

            if (_panelWidth > PanelWidth) { _panelWidth = PanelWidth; }
            if (_panelHeight > PanelHeight) { _panelHeight = PanelHeight; }

            if (panelWidth != _panelWidth || panelHeight != _panelHeight)
            {
                panelHeight = _panelHeight;
                panelWidth = _panelWidth;

                UpdateLayout();
            }
        }

        static void UpdateLayout()
        {
            // set panel
            mainPanel.sizeDelta = new Vector2(panelWidth, panelHeight);

            // setting view
            var itemWindowHeight = Calcf.Clip(ComponentHeight, ItemWindowHeight, panelHeight - (4 * gap + 3 * ComponentHeight + TitleHeight));

            // item window head
            var left = gap;
            var bottom = panelHeight - (TitleHeight + ComponentHeight);
            var right = -gap;
            var top = -TitleHeight;

            itemWindowHead.offsetMin = new Vector2(left, bottom);
            itemWindowHead.offsetMax = new Vector2(right, top);

            // item window
            bottom = panelHeight - (TitleHeight + ComponentHeight + itemWindowHeight);
            top = -(TitleHeight + ComponentHeight);

            itemWindow.offsetMin = new Vector2(left, bottom);
            itemWindow.offsetMax = new Vector2(right, top);

            // message window 
            var buttonWidth = (int)((panelWidth - 3 * gap) * (160.0f / (605.0f + 160.0f)));

            left = gap;
            bottom = gap;
            right = -(2 * gap + buttonWidth);
            top = -(TitleHeight + ComponentHeight + itemWindowHeight + gap);

            messageWindow.offsetMin = new Vector2(left, bottom);
            messageWindow.offsetMax = new Vector2(right, top);

            // remain money window
            left = panelWidth - (gap + buttonWidth);
            bottom = 3 * gap + 2 * ComponentHeight;
            right = -gap;

            remainMoneyWindow.offsetMin = new Vector2(left, bottom);
            remainMoneyWindow.offsetMax = new Vector2(right, top);

            // cancel button
            bottom -= gap + ComponentHeight;
            top -= gap + ComponentHeight;

            cancelButton.offsetMin = new Vector2(left, bottom);
            cancelButton.offsetMax = new Vector2(right, top);

            // ok button
            bottom -= gap + ComponentHeight;
            top -= gap + ComponentHeight;

            okButton.offsetMin = new Vector2(left, bottom);
            okButton.offsetMax = new Vector2(right, top);
        }
    }
}

