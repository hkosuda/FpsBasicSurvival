using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [RequireComponent(typeof(BoxCollider))]
    public class WarpGate : MonoBehaviour
    {
        [SerializeField] HostName hostName = HostName.ez_square;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == Const.playerLayer)
            {
                var code = TxtUtil.L(CommandName.Begin) + " " + hostName.ToString() + " -m";
                CommandReceiver.RequestCommand(code, null);
            }
        }
    }
}

