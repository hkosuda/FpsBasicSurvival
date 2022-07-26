using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class WindowDelayedLayout : MonoBehaviour
    {
        VerticalLayoutGroup vLayout;
        int frameBuffer;

        private void Awake()
        {
            vLayout = gameObject.GetComponent<VerticalLayoutGroup>();
        }

        private void Start()
        {
            frameBuffer = 3;
        }

        private void Update()
        {
            frameBuffer--;

            if (frameBuffer == 0)
            {
                vLayout.CalculateLayoutInputHorizontal();
                vLayout.CalculateLayoutInputVertical();

                vLayout.SetLayoutHorizontal();
                vLayout.SetLayoutVertical();
            }
        }
    }
}

