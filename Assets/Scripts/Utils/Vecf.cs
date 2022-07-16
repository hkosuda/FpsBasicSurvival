using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    static public class Vecf
    {
        static public float Magnitude(float[] vec)
        {
            return Mathf.Sqrt(Mathf.Pow(vec[0], 2.0f) + Mathf.Pow(vec[1], 2.0f));
        }
    }
}

