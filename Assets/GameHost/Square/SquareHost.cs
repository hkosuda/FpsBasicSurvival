using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SquareHost : GameHost
    {
        public SquareHost(HostName hostName) : base(hostName)
        {
            componentList = new List<HostComponent>()
            {
                new BHop_Command(),
                new BHop_Back(),

                new Square_Map(),

                new SV_Round(),
                new SV_Weapon(),
            };
        }
    }
}

