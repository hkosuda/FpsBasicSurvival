using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [RequireComponent(typeof(BoxCollider))]
    public class Goal : MonoBehaviour
    {
        static public EventHandler<bool> InsufficientKeys { get; set; }

        private void Start()
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != Const.playerLayer) { return; }
            if (SV_Round.RoundNumber > 0 && SV_Round.CurrentKey < SvParams.GetInt(SvParam.require_keys)) { return; }

            TimerSystem.Pause();
            SV_ShopItem.BeginShopping();
        }
    }
}

