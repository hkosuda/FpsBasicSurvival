using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class BHop_BackZero : HostComponent
    {
        public override void Initialize()
        {
            SetEvent(1);
        }

        public override void Shutdown()
        {
            SetEvent(-1);
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                InvalidArea.CourseOut += BackZero;
            }

            else
            {
                InvalidArea.CourseOut -= BackZero;
            }
        }

        static void BackZero(object obj, Vector3 position)
        {
            MapSystem.CurrentMap.Back(0);
        }
    }
}

