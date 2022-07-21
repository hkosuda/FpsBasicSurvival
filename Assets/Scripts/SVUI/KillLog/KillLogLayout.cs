using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class KillLogLayout : MonoBehaviour
    {
        static readonly int height = 18;

        static GameObject contentGroup;

        ContentSizeFitter filter1;
        ContentSizeFitter filter2;

        HorizontalLayoutGroup layoutGroup;

        int counter;

        private void Start()
        {
            contentGroup = gameObject.transform.GetChild(2).gameObject;

            layoutGroup = contentGroup.GetComponent<HorizontalLayoutGroup>();

            filter1 = contentGroup.transform.GetChild(0).gameObject.GetComponent<ContentSizeFitter>();
            filter2 = contentGroup.transform.GetChild(2).gameObject.GetComponent<ContentSizeFitter>();

            counter = 0;
        }

        private void Update()
        {
            counter++;

            if (counter == 1)
            {
                filter1.SetLayoutVertical();
                filter1.SetLayoutHorizontal();

                filter2.SetLayoutVertical();
                filter2.SetLayoutHorizontal();
            }

            if (counter == 2)
            {
                layoutGroup.CalculateLayoutInputHorizontal();
                layoutGroup.CalculateLayoutInputVertical();

                layoutGroup.SetLayoutHorizontal();
                layoutGroup.SetLayoutVertical();
            }

            if (counter == 3)
            {
                var rect = gameObject.GetComponent<RectTransform>();
                var contentRect = contentGroup.GetComponent<RectTransform>();
                Debug.Log(contentRect.sizeDelta.ToString());
                rect.sizeDelta = new Vector2(contentRect.sizeDelta.x, height);
            }
        }
    }
}

