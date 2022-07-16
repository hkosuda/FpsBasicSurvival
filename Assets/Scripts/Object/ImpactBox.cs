using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ImpactBox
    {
        static readonly float impactBoxExistTime = 0.6f;

        static GameObject _impactBox;

        static List<GameObject> impactBoxList;
        static List<float> timeList;

        static public void Initialize()
        {
            _impactBox = Resources.Load<GameObject>("Object/ImpactBox");

            impactBoxList = new List<GameObject>();
            timeList = new List<float>();

            SetEvent(1);
        }

        static public void Shutdown()
        {
            SetEvent(-1);
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                WeaponController.ShootingHit += InstantiateImpactBox;
                TimerSystem.Updated += UpdateMethod;
            }

            else
            {
                WeaponController.ShootingHit -= InstantiateImpactBox;
                TimerSystem.Updated -= UpdateMethod;
            }
        }

        static void InstantiateImpactBox(object obj, RaycastHit hit)
        {
            var impactBox = GameHost.Instantiate(_impactBox, hit.point, Quaternion.identity);

            impactBoxList.Add(impactBox);
            timeList.Add(0.0f);
        }

        static void UpdateMethod(object obj, float dt)
        {
            if (TimerSystem.Paused)
            {
                foreach (var box in impactBoxList)
                {
                    box.SetActive(false);
                }
            }

            for (var n = impactBoxList.Count - 1; n > -1; n--)
            {
                timeList[n] += dt;

                if (timeList[n] > impactBoxExistTime)
                {
                    var box = impactBoxList[n];
                    GameObject.Destroy(box);

                    impactBoxList.RemoveAt(n);
                    timeList.RemoveAt(n);
                }
            }
        }
    }
}

