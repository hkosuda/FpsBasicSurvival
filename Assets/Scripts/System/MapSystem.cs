using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public enum MapName
    {
        ez_tower,
        ez_stream,
        ez_square,
    }

    public class MapSystem : MonoBehaviour
    {
        static public EventHandler<bool> Initialized { get; set; }

        static public Dictionary<MapName, GameObject> MapList { get; private set; }
        static public Map CurrentMap { get; private set; }

        static GameObject mapRoot;

        private void Awake()
        {
            MapList = new Dictionary<MapName, GameObject>()
            {
                { MapName.ez_tower, GetMap("ez_tower") },
                { MapName.ez_stream, GetMap("ez_stream") },
                { MapName.ez_square, GetMap("ez_square") },
            };

            // check method
            foreach (var map in MapList.Values)
            {
                if (map == null) { Debug.LogError("No map manager"); }
            }

            // - inner function
            static GameObject GetMap(string mapName)
            {
                return Resources.Load<GameObject>("Map/" + mapName);
            }
        }

        static public void SwitchMap(MapName mapName)
        {
            if (CurrentMap != null) { CurrentMap.Shutdown(); }
            if (!MapList.ContainsKey(mapName)) { return; }

            CreateRoot();

            var map = GameHost.Instantiate(MapList[mapName]);
            map.transform.SetParent(mapRoot.transform);

            CurrentMap = map.GetComponent<Map>();

            CurrentMap.gameObject.SetActive(true);
            CurrentMap.Initialize();

            //if (Bools.Get(Bools.Item.write_events)) { CheckPoint.WriteToLog(InvokeCommand.GameEvent.on_map_changed); }
            Initialized?.Invoke(null, false);

            // - inner function
            static void CreateRoot()
            {
                if (mapRoot != null) { GameObject.Destroy(mapRoot); }

                mapRoot = new GameObject("MapRoot");
                mapRoot.transform.SetParent(GameHost.World.transform);
            }
        }
    }
}

