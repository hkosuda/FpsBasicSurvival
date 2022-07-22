using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [RequireComponent(typeof(BoxCollider))]
    public class FirstGoal : MonoBehaviour
    {
        [SerializeField] Difficulty difficulty;

        private void Start()
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != Const.playerLayer) { return; }

            SvParams.SwitchDifficulty(difficulty);

            TimerSystem.Pause();
            SV_ShopItem.BeginShopping();
        }
    }
}