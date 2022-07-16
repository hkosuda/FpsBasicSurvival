using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_ItemAdmin : HostComponent
    {
        static GameObject _key;
        static GameObject _compass;

        static GameObject _healing;
        static GameObject _armor;
        static GameObject _ammo;
        static GameObject _money;

        public SV_ItemAdmin()
        {
            
        }

        public override void Initialize()
        {
            _key = Load("Key");
            _compass = Load("Compass");
            _healing = Load("Healing");
            _armor = Load("Armor");
            _money = Load("Money");

            static GameObject Load(string name)
            {
                return Resources.Load<GameObject>("Items/" + name);
            }
        }

        public override void Shutdown()
        {
            _key = null;
            _compass = null;
            _healing = null;
            _armor = null;
            _money = null;
        }

        public override void Begin()
        {

        }

        public override void Stop()
        {

        }

        
    }
}

