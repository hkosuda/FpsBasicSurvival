using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ShopPanel : MonoBehaviour
    {
        static GameObject myself;

        private void Awake()
        {
            myself = gameObject;
        }

        void Start()
        {
            SV_Shop.Initialize();
        }

        private void OnDestroy()
        {
            SV_Shop.ReflectUpgrades();
            TimerSystem.Resume();
        }

        void Update()
        {
            TimerSystem.Pause();
        }

        static public void DestroyShopPanel()
        {
            Destroy(myself);
        }
    }
}

