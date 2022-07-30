using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class MineRotar : MonoBehaviour
    {
        static readonly float speed = 5.0f;

        List<GameObject> propellerList;
        float rot;

        private void Awake()
        {
            propellerList = new List<GameObject>()
            {
                gameObject.transform.GetChild(0).gameObject,
                gameObject.transform.GetChild(1).gameObject,
                gameObject.transform.GetChild(2).gameObject,
                gameObject.transform.GetChild(3).gameObject,
            };
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
                TimerSystem.Updated += UpdateMethod;
            }

            else
            {
                TimerSystem.Updated -= UpdateMethod;
            }
        }

        void UpdateMethod(object obj, float dt)
        {
            rot = (rot + dt * speed * 360.0f) % 360.0f;

            foreach(var propeller in propellerList)
            {
                propeller.transform.eulerAngles = new Vector3(0.0f, rot, 0.0f);
            }
        }
    }
}

