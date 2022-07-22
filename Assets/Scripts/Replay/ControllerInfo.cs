using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class ControllerInfo : MonoBehaviour
    {
        static TextMeshProUGUI x;
        static TextMeshProUGUI y;
        static TextMeshProUGUI z;
        static TextMeshProUGUI vx;
        static TextMeshProUGUI vy;
        static TextMeshProUGUI vz;
        static TextMeshProUGUI vxy;

        private void Awake()
        {
            x = GetText(0);
            y = GetText(1);
            z = GetText(2);
            vx = GetText(3);
            vy = GetText(4);
            vz = GetText(5);
            vxy = GetText(6);

            TextMeshProUGUI GetText(int n)
            {
                return gameObject.transform.GetChild(n).gameObject.GetComponent<TextMeshProUGUI>();
            }
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
                ReplaySystem.Updated += UpdateInfo;
            }

            else
            {
                ReplaySystem.Updated -= UpdateInfo;
            }
        }

        static void UpdateInfo(object ojb, float[] data)
        {
            x.text = Text("X", data[1]);
            y.text = Text("Y", data[2] - Player.centerY);
            z.text = Text("Z", data[3]);
            vx.text = Text("Vx", data[7]);
            vy.text = Text("Vy", data[8]);
            vz.text = Text("Vz", data[9]);
            vxy.text = Text("|Vxz|", new Vector2(data[7], data[9]).magnitude);

            static string Text(string head, float value)
            {
                return head + " : " + value.ToString("F2");
            }
        }
    }
}

