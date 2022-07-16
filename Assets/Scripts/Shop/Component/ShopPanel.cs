using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    static GameObject myself;

    private void Awake()
    {
        myself = gameObject;
    }

    void Start()
    {
        SV_ShopAdmin.Initialize();
    }

    private void OnDestroy()
    {
        SV_ShopAdmin.ReflectUpgrades();
        TimerSystem.TimerResume();
    }

    void Update()
    {
        TimerSystem.TimerPause();
    }

    static public void DestroyShopPanel()
    {
        Destroy(myself);
    }
}
