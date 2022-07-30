using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class ChatMessageLayout : MonoBehaviour
    {
        static public EventHandler<bool> LayoutEnd { get; set; }

        static readonly int frameBuffer = 6;

        int frameBufferRemain;

        VerticalLayoutGroup vLayout;
        ContentSizeFitter sizeFilter;

        private void Awake()
        {
            sizeFilter = gameObject.GetComponent<ContentSizeFitter>();
            vLayout = gameObject.GetComponent<VerticalLayoutGroup>();
        }

        void Start()
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
                ChatMessageManager.ChatUpdated += BeginLayout1;

                TimerSystem.TimerPaused += BeginLayout2;
                TimerSystem.TimerPaused += BeginLayout3;
            }

            else
            {
                ChatMessageManager.ChatUpdated -= BeginLayout1;

                TimerSystem.TimerPaused -= BeginLayout2;
                TimerSystem.TimerPaused -= BeginLayout3;
            }
        }

        void BeginLayout1(object obj, bool mute)
        {
            BeginLayout();
        }

        void BeginLayout2(object obj, bool mute)
        {
            BeginLayout();
        }

        void BeginLayout3(object obj, bool mute)
        {
            BeginLayout();
        }

        void BeginLayout()
        {
            frameBufferRemain = frameBuffer;
        }

        void Update()
        {
            frameBufferRemain--;
            if (frameBufferRemain < 0) { frameBufferRemain = -1; }

            if (frameBufferRemain == 2)
            {
                sizeFilter.SetLayoutHorizontal();
                sizeFilter.SetLayoutVertical();
            }

            if (frameBufferRemain == 4)
            {
                vLayout.CalculateLayoutInputHorizontal();
                vLayout.CalculateLayoutInputVertical();

                vLayout.SetLayoutHorizontal();
                vLayout.SetLayoutVertical();
            }

            if (frameBufferRemain == 0)
            {
                LayoutEnd?.Invoke(null, false);
            }
        }
    }
}

