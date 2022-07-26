using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class ConsoleLogLayout : MonoBehaviour
    {
        static public EventHandler<bool> LayoutEnd { get; set; }

        static readonly int vLayoutFrameBuffer = 2;
        static readonly int sizeFilterFrameBuffer = 2;

        static VerticalLayoutGroup vLayout;
        static ContentSizeFitter sizeFilter;

        static int vLayoutFrameBufferRemain;
        static int sizeFilterFrameBufferRemain;

        void Awake()
        {
            vLayout = gameObject.GetComponent<VerticalLayoutGroup>();
            sizeFilter = gameObject.GetComponent<ContentSizeFitter>();
        }

        void Start()
        {
            SetEvent(1);
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                ConsoleLogManager.Updated += BeginLayout;
            }

            else
            {
                ConsoleLogManager.Updated -= BeginLayout;
            }
        }

        static void BeginLayout(object obj, bool mute)
        {
            vLayoutFrameBufferRemain = vLayoutFrameBuffer;
        }

        void Update()
        {
            sizeFilterFrameBufferRemain--;
            if (sizeFilterFrameBufferRemain < 0) { sizeFilterFrameBufferRemain = -1; }

            if (sizeFilterFrameBufferRemain == 0)
            {
                sizeFilter.SetLayoutHorizontal();
                sizeFilter.SetLayoutVertical();
            }

            vLayoutFrameBufferRemain--;
            if (vLayoutFrameBufferRemain < 0) { vLayoutFrameBufferRemain = -1; }

            if (vLayoutFrameBufferRemain == 0)
            {
                vLayout.CalculateLayoutInputHorizontal();
                vLayout.CalculateLayoutInputVertical();

                vLayout.SetLayoutHorizontal();
                vLayout.SetLayoutVertical();

                sizeFilterFrameBufferRemain = sizeFilterFrameBuffer;

                LayoutEnd?.Invoke(null, false);
            }
        }

        private void FixedUpdate()
        {
            vLayout.CalculateLayoutInputVertical();
            vLayout.SetLayoutVertical();

            sizeFilter.SetLayoutVertical();
        }
    }
}

