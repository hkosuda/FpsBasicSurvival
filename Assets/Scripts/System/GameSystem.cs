using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class GameSystem : MonoBehaviour
    {
        static readonly string rootName = "root";

        [SerializeField] HostName defaultHost = HostName.survival;

        static public Dictionary<HostName, GameHost> HostList { get; private set; } = new Dictionary<HostName, GameHost>()
        {
            { HostName.survival, new SvHost(HostName.survival) }
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
            GameHost.BeginHost(CurrentHost);
        }
    }
}

