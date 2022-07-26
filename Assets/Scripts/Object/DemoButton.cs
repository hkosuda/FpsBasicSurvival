using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class DemoButton : MonoBehaviour
    {
        [SerializeField] string fileName = "";

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
            if(hit.collider.gameObject == body)
            {
                CommandReceiver.RequestCommand("demo " + fileName + " -m", null);
            }
        }
    }
}

