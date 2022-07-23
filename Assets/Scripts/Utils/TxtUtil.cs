using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public enum None
    {
        none,
    }

    public enum Clr
    {
        white,
        red,
        cyan,
        lime,
        orange,
    }

    static public class TxtUtil
    {
        static public string C(string text, Clr color)
        {
            if (color == Clr.cyan)
            {
                return "<color=#00FFFFFF>" + text + "</color>";
            }

            if (color == Clr.lime)
            {
                return "<color=#00FF00FF>" + text + "</color>";
            }

            return "<color=" + color.ToString().ToLower() + ">" + text + "</color>";
        }

        static public string L<T>(T e) where T : Enum
        {
            return e.ToString().ToLower();
        }

        static public string Command<T>(List<T> ts) where T : Enum
        {
            var command = "";

            foreach(T t in ts)
            {
                command += t + " ";
            }

            return command;
        }

        static public List<string> GetValues(string code)
        {
            var splitted = SplitBySpace(code);

            var values = new List<string>();

            foreach (var s in splitted)
            {
                if (s.StartsWith("-")) { continue; }

                values.Add(s);
            }

            return values;
        }

        static List<string> SplitBySpace(string sentence)
        {
            var splitted = sentence.Split(new char[] { ' ', 'Å@' }, System.StringSplitOptions.RemoveEmptyEntries);

            return new List<string>(splitted);
        }

        static public string PaddingZero2(int n)
        {
            if (n < 10)
            {
                return "0" + n.ToString();
            }

            return n.ToString();
        }

        static public string PaddingZero3(int n)
        {
            if (n < 10)
            {
                return "00" + n.ToString();
            }

            else if (n < 100)
            {
                return "0" + n.ToString();
            }

            return n.ToString();
        }

        static public string Time(float time, bool msec, string separator = ":")
        {
            var min = (int)(time / 60.0f);
            var sec = (int)(time - min * 60.0f);
            
            if (msec)
            {
                var ms = (int)((time - min * 60.0f - sec) * 100.0f);

                return PaddingZero2(min) + separator + PaddingZero2(sec) + separator + PaddingZero2(ms);
            }

            else
            {
                return PaddingZero2(min) + separator + PaddingZero2(sec);
            }
        }

        static public string SecMSec(float time, string separator = ":")
        {
            var min = (int)(time / 60.0f);
            var sec = (int)(time - min * 60.0f);
            var ms = (int)((time - min * 60.0f - sec) * 100.0f);

            return PaddingZero2(sec) + separator + PaddingZero2(ms);
        }
    }
}

