using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class GameSystem : MonoBehaviour
    {
        static public EventHandler<bool> HostSwitched { get; set; }

        static readonly string rootName = "Root";

        [SerializeField] HostName defaultHost = HostName.survival;

        static public Dictionary<HostName, GameHost> HostList { get; private set; } = new Dictionary<HostName, GameHost>()
        {
            { HostName.survival, new SvHost(HostName.survival) },
            { HostName.ez_tower, new TowerHost(HostName.ez_tower) },
            { HostName.ez_stream, new StreamHost(HostName.ez_stream) },
        };

        static public GameObject Root { get; private set; }

        static public GameHost CurrentHost { get; private set; }

        private void Awake()
        {
            Root = new GameObject(rootName);
        }

        private void Start()
        {
            SwitchHost(defaultHost);
        }

        static public void SwitchHost(HostName hostName)
        {
            if (CurrentHost != null)
            {
                GameHost.StopHost(CurrentHost);
                GameHost.ShutdownHost(CurrentHost);
            }

            CurrentHost = HostList[hostName];

            GameHost.InitializeHost(CurrentHost);
            HostSwitched?.Invoke(null, false);

            GameHost.BeginHost(CurrentHost);
        }
    }
}

