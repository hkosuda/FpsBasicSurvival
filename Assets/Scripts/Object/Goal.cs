using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [RequireComponent(typeof(BoxCollider))]
    public class Goal : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == Const.playerLayer)
            {
                TimerSystem.Pause();
                SV_ShopItem.BeginShopping();
            }
        }
    }

}
