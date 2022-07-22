using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SvHost : GameHost
    {
        public SvHost(HostName hostName) : base(hostName)
        {
            componentList = new List<HostComponent>()
            {
                new SV_Seed(),
                new SV_Round(),
                new SV_Map(),
                new SV_GoalStart(),
                new SV_Player(),
                new SV_Enemy(),
                new SV_Items(),
                new SV_Status(),
                new SV_Time(),
                new SV_ShopItem(),
                new SV_History(),
            };
        }

        static public void BeginNext()
        {
            StopHost(GameSystem.HostList[HostName.survival]);
            BeginHost(GameSystem.HostList[HostName.survival]);
        }
    }
}

