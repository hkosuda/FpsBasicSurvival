using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class KillLogManagerLayout : MonoBehaviour
    {
        static VerticalLayoutGroup layoutGroup;
        static int counter;

        private void Awake()
        {
            layoutGroup = gameObject.GetComponent<VerticalLayoutGroup>();
        }

        private void Start()
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
                SVUI_KillLogManager.LogUpdated += BeginLayout;
            }

            else
            {
                SVUI_KillLogManager.LogUpdated -= BeginLayout;
            }
        }

        static void BeginLayout(object obj, bool mute)
        {
            counter = 0;
        }

        private void Update()
        {
            counter++;

            if (counter == 4)
            {
                layoutGroup.CalculateLayoutInputHorizontal();
                layoutGroup.CalculateLayoutInputVertical();

                layoutGroup.SetLayoutHorizontal();
                layoutGroup.SetLayoutVertical();
            }
        }
    }
}

