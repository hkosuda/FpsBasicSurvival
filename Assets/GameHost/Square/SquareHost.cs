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
                new Square_Map(),
            };
        }
    }
}

