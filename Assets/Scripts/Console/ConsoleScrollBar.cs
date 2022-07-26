using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class ConsoleScrollBar : MonoBehaviour
    {
        static Scrollbar scrBar;

        static readonly int frameBuffer = 4;
        static int frameBufferRemain;

        private void Awake()
        {
            scrBar = gameObject.GetComponent<Scrollbar>();
        }

        void Start()
        {
            frameBufferRemain = frameBuffer;
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
                Console.Opened += ReserveUpdate1;
                ConsoleLogLayout.LayoutEnd += ReserveUpdate3;
            }

            else
            {
                Console.Opened -= ReserveUpdate1;
                ConsoleLogLayout.LayoutEnd -= ReserveUpdate3;
            }
        }

        private void Update()
        {
            if (frameBufferRemain > 0)
            {
                frameBufferRemain--;

                if (frameBufferRemain == 0)
                {
                    SetBottom();
                }
            }
        }

        static void ReserveUpdate1(object obj, bool mute)
        {
            frameBufferRemain = frameBuffer;
        }

        static void ReserveUpdate2(object obj, bool mute)
        {
            frameBufferRemain = frameBuffer;
        }

        static void ReserveUpdate3(object obj, bool mute)
        {
            frameBufferRemain = frameBuffer;
        }

        static void SetBottom()
        {
            scrBar.value = 0.0f;
        }
    }
}

