using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class StreamHost : GameHost
    {
        public StreamHost(HostName hostName) : base(hostName)
        {
            componentList = new List<HostComponent>()
            {
                new BHop_Command(),

                new SV_Round(),
                new SV_Status(),

                new Stream_Map(),
            };
        }
    }
}
