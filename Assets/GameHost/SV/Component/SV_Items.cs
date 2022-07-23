using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public enum Item
    {
        key, compass, healing, armor, money
    }

    public class SV_Items : HostComponent
    {
        static readonly int offsetSize = 3;

        static readonly Dictionary<Item, float> keyDropRateList = new Dictionary<Item, float>() { { Item.key, 1.0f } };
        static readonly Dictionary<Item, float> compassDropRateList = new Dictionary<Item, float>() { { Item.compass, 1.0f } };
        static readonly Dictionary<Item, float> normalItemDropRateList = new Dictionary<Item, float>()
        {
            { Item.healing, 1.0f },
            { Item.armor, 1.0f },
            { Item.money, 2.0f },
        };

        static Dictionary<Item, GameObject> keyPrefabList;
        static Dictionary<Item, GameObject> compassPrefabList;
        static Dictionary<Item, GameObject> normalItemPrefabList;

        public override void Initialize()
        {
            keyPrefabList = new Dictionary<Item, GameObject>() { { Item.key, Load("Key") } };
            compassPrefabList = new Dictionary<Item, GameObject>() { { Item.compass, Load("Compass") }, };
            normalItemPrefabList = new Dictionary<Item, GameObject>()
            {
                { Item.healing, Load("Healing") },
                { Item.armor, Load("Armor") },
                { Item.money, Load("Money") },
            };

            static GameObject Load(string name)
            {
                return Resources.Load<GameObject>("Item/" + name);
            }
        }

        public override void Shutdown()
        {
            keyPrefabList = null;
            compassPrefabList = null;
            normalItemPrefabList = null;
        }

        public override void Begin()
        {
            if (SV_Round.RoundNumber == 0) { return; }

            var root = new GameObject("Items");
            root.transform.SetParent(GameHost.World.transform);

            var startGoal = new List<int[]> { SV_GoalStart.StartPoint, SV_GoalStart.GoalPoint };
            var passable = ShareSystem.Passable;

            var cornerPoints = SvUtil_DropSystem.GetCornerPoints(startGoal, passable, offsetSize);

            var keyDropInfo = SvUtil_DropSystem.RandomDrop(cornerPoints, keyPrefabList, keyDropRateList, SvParams.GetInt(SvParam.drop_keys), root);
            RemovePoints(cornerPoints, keyDropInfo);

            var compassDropInfo = SvUtil_DropSystem.RandomDrop(cornerPoints, compassPrefabList, compassDropRateList, SvParams.GetInt(SvParam.drop_compass), root);
            RemovePoints(cornerPoints, compassDropInfo);

            SvUtil_DropSystem.RandomDrop(cornerPoints, normalItemPrefabList, normalItemDropRateList, SvParams.GetInt(SvParam.drop_items), root);

            // - inner function
            static void RemovePoints(List<int[]> points, List<SvUtil_DropSystem.DropInfo> dropInfoList)
            {
                foreach(var info in dropInfoList)
                {
                    points.Remove(info.point);
                }
            }
        }

        public override void Stop()
        {

        }
    }
}

