using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    static public class SvUtil_DropSystem
    {
        static int[,] crossPoints = new int[4, 2]
        {
            { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 }
        };

        static public List<int[]> GetCornerPoints(List<int[]> startGoal, bool[,] passable, int offsetSize = 5)
        {
            var row = passable.GetLength(0);
            var col = passable.GetLength(1);

            var cornerPointList = new List<int[]>();

            for (var c = 0; c < col; c++)
            {
                for (var r = 0; r < row; r++)
                {
                    var point = new int[2] { r, c };

                    if (!IsPassable(point, passable)) { continue; }
                    if (!IsCorner(point, passable, row, col)) { continue; }

                    if (!CheckOffset(point, startGoal[0], offsetSize)) { continue; }
                    if (!CheckOffset(point, startGoal[1], offsetSize)) { continue; }

                    cornerPointList.Add(new int[2] { r, c });
                }
            }

            return cornerPointList;

            // - inner function
            static bool IsCorner(int[] point, bool[,] passable, int row, int col)
            {
                var points = crossPoints.GetLength(0);

                for (var n = 0; n < points; n++)
                {
                    var i1 = n % points;
                    var i2 = (n + 1) % points;

                    var p1 = new int[2] { point[0] + crossPoints[i1, 0], point[1] + crossPoints[i1, 1] };
                    var p2 = new int[2] { point[0] + crossPoints[i2, 0], point[1] + crossPoints[i2, 1] };

                    if (OutOfRange(p1, row, col) || OutOfRange(p2, row, col)) { continue; }
                    
                    if (!IsPassable(p1, passable) && !IsPassable(p2, passable))
                    {
                        return true;    
                    }
                }

                return false;
            }

            // - inner function
            static bool CheckOffset(int[] point1, int[] point2, int offset)
            {
                var dr = Mathf.Abs(point1[0] - point2[0]);
                var dc = Mathf.Abs(point1[1] - point2[1]);

                if (dr > offset && dc > offset)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }

            // - inner function
            static bool OutOfRange(int[] point, int row, int col)
            {
                if (point[0] < 0 || point[0] >= row) { return true; }
                if (point[1] < 0 || point[1] >= col) { return true; }

                return false;
            }

            // - inner function
            static bool IsPassable(int[] point, bool[,] passable)
            {
                return passable[point[0], point[1]];
            }
        }

        static public List<DropInfo> RandomDrop<T>(List<int[]> _pointList, Dictionary<T, GameObject> prefabList, Dictionary<T, float> dropRateList, int maxDrops, GameObject root = null)
        {
            SV_Seed.Init();
            var pointList = new List<int[]>(_pointList);

            var sortedPointList = RandomSort(pointList);
            var normalizedDropRateList = NormalizeDropRateList(dropRateList);
            var dropNumberList = DropNumberList(pointList, normalizedDropRateList, maxDrops);

            return Drop(sortedPointList, dropNumberList, prefabList, root);

            // - inner function
            static List<int[]> RandomSort(List<int[]> _list)
            {
                var list = new List<int[]>(_list);
                var sorted = new List<int[]>();

                while (true)
                {
                    if (list.Count == 0) { break; }
                    var index = UnityEngine.Random.Range(0, list.Count);
                    Debug.Log("INdex : " + index.ToString());
                    sorted.Add(list[index]);
                    list.RemoveAt(index);
                }

                return sorted;
            }

            // - inner function
            static Dictionary<T, float> NormalizeDropRateList(Dictionary<T, float> dropRateList)
            {
                var sum = 0.0f;

                foreach (var rate in dropRateList.Values)
                {
                    sum += rate;
                }

                var normalizedDropRate = new Dictionary<T, float>();

                foreach (var pair in dropRateList)
                {
                    normalizedDropRate.Add(pair.Key, Calcf.SafetyDiv(pair.Value, sum, 0.0f));
                }

                return normalizedDropRate;
            }

            // - inner function
            static Dictionary<T, int> DropNumberList(List<int[]> pointList, Dictionary<T, float> normalizedDropRateList, int maxDrops)
            {
                var maxPoints = pointList.Count;
                if (maxPoints < maxDrops) { maxDrops = maxPoints; }

                var dropNumberList = new Dictionary<T, int>();
                var counter = 0;

                foreach(var pair in normalizedDropRateList)
                {
                    var number = Mathf.FloorToInt(pair.Value * maxDrops);
                    counter += number;

                    dropNumberList.Add(pair.Key, number);
                }

                while (true)
                {
                    if (counter >= maxDrops) { break; }

                    foreach(var pair in normalizedDropRateList)
                    {
                        dropNumberList[pair.Key]++;
                        counter++;

                        if (counter >= maxDrops) { break; }
                    }
                }

                return dropNumberList;
            }

            // - inner function
            static List<DropInfo> Drop(List<int[]> sortedPointList, Dictionary<T, int> dropNumberList, Dictionary<T, GameObject> prefabList, GameObject root)
            {
                var dropInfoList = new List<DropInfo>();

                var pointIndex = 0;

                foreach(var dropNum in dropNumberList)
                {
                    var prefab = prefabList[dropNum.Key];
                    var num = dropNum.Value;

                    for(var n = 0; n < num; n++)
                    {
                        var point = sortedPointList[pointIndex];
                        pointIndex++;

                        var position = ShareSystem.Point2Position(point);
                        var gameObject = GameObject.Instantiate(prefab, position, Quaternion.identity);

                        dropInfoList.Add(new DropInfo(dropNum.Key.ToString(), point));

                        if (root != null) { gameObject.transform.SetParent(root.transform); }
                    }
                }

                return dropInfoList;
            }
        }

        public class DropInfo
        {
            public string objectName;
            public int[] point;

            public DropInfo(string objectName, int[] point)
            {
                this.objectName = objectName;
                this.point = point;
            }
        }
    }
}

