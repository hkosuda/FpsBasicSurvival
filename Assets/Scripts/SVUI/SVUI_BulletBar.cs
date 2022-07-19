using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class SVUI_BulletBar : MonoBehaviour
    {
        static List<GameObject> cellList;
        static int currentBullets;

        private void Start()
        {
            cellList = new List<GameObject>();
            var nChild = gameObject.transform.childCount;

            for(var n = 0; n < nChild; n++)
            {
                cellList.Add(gameObject.transform.GetChild(n).gameObject);
            }

            UpdateCurrentBulletsBar();
        }

        private void Update()
        {
            var _currentBullets = GetBullets();

            if (currentBullets != _currentBullets)
            {
                currentBullets = _currentBullets;
                UpdateCurrentBulletsBar();
            }

            // - inner function
            static int GetBullets()
            {
                var weapon = WeaponSystem.CurrentWeapon.Weapon;

                if (weapon == Weapon.ak)
                {
                    return AK_Availability.AmmoInMag;
                }

                if (weapon == Weapon.de)
                {
                    return DE_Availability.AmmoInMag;
                }

                return 0;
            }
        }

        static void UpdateCurrentBulletsBar()
        {
            if (currentBullets < 0) 
            {
                foreach(var cell in cellList)
                {
                    cell.SetActive(false);
                }

                return;
            }

            for(var n = 0; n < cellList.Count; n++)
            {
                if (n < currentBullets)
                {
                    cellList[n].SetActive(true);
                }

                else
                {
                    cellList[n].SetActive(false);
                }
            }
        }
    }
}

