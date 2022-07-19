using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class SVUI_KillLog : MonoBehaviour
    {
        TextMeshProUGUI killerText;
        TextMeshProUGUI deadText;
        Image weaponImage;

        float timeRemain;

        private void Awake()
        {
            var content = gameObject.transform.GetChild(5);

            killerText = content.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            deadText = content.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
            weaponImage = content.GetChild(1).gameObject.GetComponent<Image>();

            timeRemain = SVUI_KillLogManager.logExistTime;
        }

        private void Start()
        {
            SetEvent(1);
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                TimerSystem.Updated += UpdateMethod;
            }

            else
            {
                TimerSystem.Updated -= UpdateMethod;
            }
        }

        void UpdateMethod(object obj, float dt)
        {
            timeRemain -= dt;

            if (timeRemain < 0)
            {
                SVUI_KillLogManager.RemoveKillLog();
            }
        }

        public void Setup(string killer, string dead, Sprite weaponSprite)
        {
            killerText.text = killer;
            deadText.text = dead;
            weaponImage.sprite = weaponSprite;

            if (killer == dead)
            {
                for (var n = 1; n < 5; n++)
                {
                    gameObject.transform.GetChild(n).gameObject.SetActive(false);
                }
            }
        }

        //
        // static contents

        static GameObject _killLog;
        static Sprite akm2D;
        static Sprite deagle2D;
        static Sprite bayonet2D;
        static Sprite karambit2D;
        static Sprite explosion2D;

        static public void Initialize()
        {
            _killLog = Load<GameObject>("UiComponent/KillLog");

            var folder = "UiSprite/";
            akm2D = Load<Sprite>(folder + "akm");
            deagle2D = Load<Sprite>(folder + "deagle");
            bayonet2D = Load<Sprite>(folder + "bayonet");
            karambit2D = Load<Sprite>(folder + "karambit");
            explosion2D = Load<Sprite>(folder + "explosion");

            // - inner function
            static T Load<T>(string path) where T : UnityEngine.Object
            {
                return Resources.Load<T>(path);
            }
        }

        static public GameObject InstantiateKillLog(string killer, string dead)//, Weapon usedWeapon)
        {
            var killLog = Instantiate(_killLog);
            //killLog.GetComponent<SVUI_KillLog>().Setup(killer, dead, GetSprite(killer, dead, usedWeapon));

            return killLog;

            //// - inner function
            //static Sprite GetSprite(string killer, string dead, Weapon usedWeapon)
            //{
            //    if (killer == dead) { return explosion2D; }
            //    if (usedWeapon == Weapon.akm) { return akm2D; }
            //    if (usedWeapon == Weapon.deagle) { return deagle2D; }
            //    if (usedWeapon == Weapon.bayonet) { return bayonet2D; }
            //    if (usedWeapon == Weapon.karambit) { return karambit2D; }

            //    return null;
            //}
        }
    }
}

