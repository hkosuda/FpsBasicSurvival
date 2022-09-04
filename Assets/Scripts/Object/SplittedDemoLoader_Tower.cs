using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SplittedDemoLoader_Tower : MonoBehaviour
    {
        static readonly List<string> fileNames = new List<string>()
        {
            "ez_tower_demo_01", "ez_tower_demo_02", "ez_tower_demo_03",
            "ez_tower_demo_04", "ez_tower_demo_05", "ez_tower_demo_06",
            "ez_tower_demo_07", "ez_tower_demo_08", "ez_tower_demo_09", "ez_tower_demo_10",
        };

        GameObject body;

        private void Awake()
        {
            body = gameObject.transform.GetChild(0).gameObject;
        }

        void Start()
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
                WeaponController.ShootingHit += PlayDemo;
            }

            else
            {
                WeaponController.ShootingHit -= PlayDemo;
            }
        }

        void PlayDemo(object obj, RaycastHit hit)
        {
            if (hit.collider.gameObject == body)
            {
                var tracer = new Tracer(null, Tracer.Option.mute);
                var demoData = new CachedData(new List<float[]>(), MapName.ez_tower);

                foreach (var fileName in fileNames)
                {
                    if (RecordDataIO.TryLoad(fileName, out var data, tracer))
                    {
                        demoData.dataList.AddRange(data.dataList);
                    }
                }

                if (ReplaySystem.TryBeginReplay(demoData, tracer))
                {
                    Console.CloseConsole();
                }
            }
        }
    }
}


