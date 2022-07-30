using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public enum HostName
    {
        survival,
        ez_tower,
        ez_stream,
        ez_square,
    }

    public abstract class GameHost
    {
        static public EventHandler<bool> HostInitialized { get; set; }
        static public EventHandler<bool> HostShutdown { get; set; }
        static public EventHandler<bool> HostBegan { get; set; }
        static public EventHandler<bool> HostStopped { get; set; }


        static readonly string worldName = "World";

        public HostName HostName { get; protected set; }

        static public GameObject World { get; private set; }

        protected List<HostComponent> componentList = new List<HostComponent>();

        public GameHost(HostName hostName)
        {
            HostName = hostName;
        }

        protected virtual void Initialize() { }

        protected virtual void Shutdown() { }

        public virtual void Begin() { }

        public virtual void Stop() { }

        static public void InitializeHost(GameHost host)
        {
            CreateWorld();

            if (host == null) { return; }

            foreach (var manager in host.componentList)
            {
                manager.Initialize();
            }

            host.Initialize();
            HostInitialized?.Invoke(null, false);
        }

        static public void ShutdownHost(GameHost host)
        {
            DestroyWorld();

            if (host == null) { return; }

            foreach (var manager in host.componentList)
            {
                manager.Shutdown();
            }

            host.Shutdown();
            HostShutdown?.Invoke(null, false);
        }

        static public void BeginHost(GameHost host)
        {
            CreateWorld();

            if (host == null) { return; }

            foreach (var manager in host.componentList)
            {
                manager.Begin();
            }

            host.Begin();
            HostBegan?.Invoke(null, false);
        }

        static public void StopHost(GameHost host)
        {
            DestroyWorld();

            if (host == null) { return; }

            foreach (var manager in host.componentList)
            {
                manager.Stop();
            }

            host.Stop();
            HostStopped?.Invoke(null, false);
        }

        // utility
        static void CreateWorld()
        {
            if (World != null)
            {
                GameObject.Destroy(World);
            }

            World = new GameObject(worldName);
            World.transform.SetParent(GameSystem.Root.transform);
        }

        static void DestroyWorld()
        {
            if (World == null) { return; }

            GameObject.Destroy(World);
        }

        static public GameObject Instantiate(GameObject prefab, Vector3? position = null, Quaternion? quaternion = null)
        {
            if (World == null) 
            {
#if UNITY_EDITOR
                Debug.LogError("World is NULL");
#endif
                return null; 
            }

            var gameObject = GameObject.Instantiate(prefab);

            gameObject.transform.position = position ?? Vector3.zero;
            gameObject.transform.rotation = quaternion ?? Quaternion.identity;

            gameObject.transform.SetParent(World.transform);

            return gameObject;
        }
    }
}

