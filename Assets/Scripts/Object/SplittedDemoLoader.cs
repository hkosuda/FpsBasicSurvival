using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SplittedDemoLoader : MonoBehaviour
    {
        static readonly List<string> fileNames = new List<string>()
        {
            "ez_stream_demo_01", "ez_stream_demo_02", "ez_stream_demo_03",
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
                var demoData = new CachedData(new List<float[]>(), MapName.ez_stream);

                foreach(var fileName in fileNames)
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

