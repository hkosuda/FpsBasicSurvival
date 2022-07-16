using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    static public class Calcf
    {
        static readonly int interpolationSize = 0;

        static public float Clip(float min, float max, float value)
        {
            if (value < min) { return min; }
            if (value > max) { return max; }
            return value;
        }

        static public float SafetyDiv(float a, float b, float alt)
        {
            if (b == 0.0f)
            {
                return alt;
            }

            return a / b;
        }

        static public float[] QuoRem(float a, float b)
        {
            if (b == 0.0f) { return new float[2] { 0.0f, 0.0f }; }

            var quo = (int)(a / b);
            var rem = a - b * quo;

            return new float[2] { quo, rem };
        }

        static public float AngleDelta(float degRot1, float degRot2)
        {
            degRot1 = QuoRem(degRot1, 360.0f)[1];
            degRot2 = QuoRem(degRot2, 360.0f)[1];

            degRot1 = CorrectAngle(degRot1);
            degRot2 = CorrectAngle(degRot2);

            var delta = Mathf.Abs(degRot1 - degRot2);
            if (delta > 180.0f) { delta = 360.0f - delta; }

            return delta;

            // - inner function
            static float CorrectAngle(float deg)
            {
                if (deg < 0.0f)
                {
                    return 360.0f + deg;
                }

                return deg;
            }
        }
    }
}

