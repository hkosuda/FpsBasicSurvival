using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class HistoryContentLayout : MonoBehaviour
    {
        static VerticalLayoutGroup verticalLayoutGroup;
        static int counter = 0;

        void Start()
        {
            verticalLayoutGroup = gameObject.GetComponent<VerticalLayoutGroup>();
            counter = 0;
        }

        void Update()
        {
            counter++;
            
            if (counter == 2)
            {
                verticalLayoutGroup.CalculateLayoutInputHorizontal();
                verticalLayoutGroup.CalculateLayoutInputVertical();

                verticalLayoutGroup.SetLayoutHorizontal();
                verticalLayoutGroup.SetLayoutVertical();
            }
        }
    }
}

