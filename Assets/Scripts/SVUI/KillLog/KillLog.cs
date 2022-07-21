using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class KillLog : MonoBehaviour
    {
        public enum HowKilled
        {
            ak, de, explosion, none,
        }

        TextMeshProUGUI killerText;
        TextMeshProUGUI deadText;
        Image weaponImage;

        private void Awake()
        {
            var content = gameObject.transform.GetChild(2);

            killerText = content.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            weaponImage = content.GetChild(1).gameObject.GetComponent<Image>();
            deadText = content.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        }

        public void Setup(string killer, string dead, Sprite weaponSprite)
        {
            killerText.text = killer;
            deadText.text = dead;

            weaponImage.sprite = weaponSprite;

            if (killer == "Player1")
            {
                SetLineColor(new Color(1.0f, 0.0f, 0.0f));
            }

            else
            {
                SetLineColor(new Color(1.0f, 1.0f, 1.0f));
            }

            // - inner function
            void SetLineColor(Color color)
            {
                for (var n = 0; n < 4; n++)
                {
                    gameObject.transform.GetChild(1).GetChild(n).gameObject.GetComponent<Image>().color = color;
                }
            }
        }

        //
        // static contents

        static GameObject _killLog;
        static Sprite ak2D;
        static Sprite de2D;
        static Sprite explosion2D;

        static public void Initialize()
        {
            _killLog = Load<GameObject>("UiComponent/KillLog");

            var folder = "UiSprite/";
            ak2D = Load<Sprite>(folder + "ak");
            de2D = Load<Sprite>(folder + "de");
            explosion2D = Load<Sprite>(folder + "explosion");

            // - inner function
            static T Load<T>(string path) where T : Object
            {
                return Resources.Load<T>(path);
            }
        }

        static public GameObject InstantiateLog(string killer, string dead, HowKilled how)
        {
            var killLog = Instantiate(_killLog);
            killLog.GetComponent<KillLog>().Setup(killer, dead, GetSprite(how));

            return killLog;

            // - inner function
            static Sprite GetSprite(HowKilled how)
            {
                if (how == HowKilled.ak) { return ak2D; }
                if (how == HowKilled.de) { return de2D; }

                return explosion2D;
            }
        }
    }
}

