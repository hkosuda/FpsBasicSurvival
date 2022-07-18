using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public enum HostName
    {
        survival,
    }

    public abstract class GameHost
    {
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
        }

        static public void StopHost(GameHost host)
        {
            DestroyWorld();

            if (host == null) { return; }

            foreach (var manager in host.componentList)
            {
                manager.Stop();
            }
        }

        // utility
        static void CreateWorld()
        {
            if (World != null)
            {
                Object.Destroy(World);
            }

            World = new GameObject(worldName);
            World.transform.SetParent(GameSystem.Root.transform);
        }

        static void DestroyWorld()
        {
            if (World == null) { return; }

            Object.Destroy(World);
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

            var gameObject = Object.Instantiate(prefab);

            gameObject.transform.position = position ?? Vector3.zero;
            gameObject.transform.rotation = quaternion ?? Quaternion.identity;

            gameObject.transform.SetParent(World.transform);

            return gameObject;
        }
    }
}

