using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_GoalStart : HostComponent
    {
        static readonly Vector3 offset = new Vector3(0.1f, 0.0f, 0.1f);

        static public int[] StartPoint { get; private set; }
        static public int[] GoalPoint { get; private set; }

        static GameObject _startObject;
        static GameObject _goalObject;

        public override void Initialize()
        {
            if (_startObject == null) { _startObject = Resources.Load<GameObject>("SV/StartObject"); }
            if (_goalObject == null) { _goalObject = Resources.Load<GameObject>("SV/GoalObject"); }
        }

        public override void Shutdown()
        {
            _startObject = null;
            _goalObject = null;
        }

        public override void Begin()
        {
            var goalStart = LongestPathSolver.GetGoalStart(ShareSystem.Passable);

            GoalPoint = goalStart[0];
            StartPoint = goalStart[1];

            var start = Object.Instantiate(_startObject, ShareSystem.Point2Position(StartPoint), Quaternion.identity);
            var goal = Object.Instantiate(_goalObject, ShareSystem.Point2Position(GoalPoint), Quaternion.identity);

            start.transform.SetParent(GameHost.World.transform);
            goal.transform.SetParent(GameHost.World.transform);

            var startObjectSize = ShareSystem.Position2WallSize(start.transform.position);
            var goalObjectSize = ShareSystem.Position2WallSize(goal.transform.position);

            start.transform.localScale = startObjectSize - offset;
            goal.transform.localScale = goalObjectSize - offset;
        }

        public override void Stop()
        {

        }
    }

    public class LongestPathSolver
    {
        static int[,] boxPoint = new int[8, 2] {
        { 1, 1 }, { 1, 0 }, { 1, -1 }, { 0, 1 }, { 0, -1 }, { -1, 1 }, { -1, 0 }, { -1, -1 }
    };

        static public List<int[]> GetGoalStart(bool[,] noObject)
        {
            var start1 = GetStartPoint(noObject);

            var costMap1 = GetCostMap(noObject, start1);
            var point1 = GetPointWithMaxCost(costMap1);

            var costMap2 = GetCostMap(noObject, point1);
            var point2 = GetPointWithMaxCost(costMap2);

            var goalStart = new List<int[]>() { point1, point2 };

            return goalStart;

            //
            // functions
            static int[] GetStartPoint(bool[,] noObject)
            {
                int row = noObject.GetLength(0);
                int col = noObject.GetLength(1);

                for (int c = 0; c < col; c++)
                {
                    for (int r = 0; r < row; r++)
                    {
                        if (noObject[r, c])
                        {
                            return new int[2] { r, c };
                        }
                    }
                }

                return new int[2] { -1, -1 };
            }

            static float[,] GetCostMap(bool[,] noObject, int[] start)
            {
                int row = noObject.GetLength(0);
                int col = noObject.GetLength(1);

                var costMap = new float[row, col];
                var flag = new bool[row, col];

                var pointList = new List<int[]>() { start };

                flag[start[0], start[1]] = true;

                while (true)
                {
                    if (pointList.Count == 0) { break; }

                    var baseIndex = GetIndexWithMinCost(costMap, pointList);
                    var basePoint = pointList[baseIndex];

                    var baseCost = costMap[basePoint[0], basePoint[1]];

                    pointList.RemoveAt(baseIndex);

                    for (int n = 0; n < 8; n++)
                    {
                        int r = basePoint[0] + boxPoint[n, 0];
                        int c = basePoint[1] + boxPoint[n, 1];

                        if (r < 0 || r >= row) { continue; }
                        if (c < 0 || c >= col) { continue; }

                        if (flag[r, c]) { continue; }
                        if (!noObject[r, c]) { continue; }

                        pointList.Add(new int[2] { r, c });
                        flag[r, c] = true;

                        costMap[r, c] = baseCost + Magnitude(basePoint, new int[2] { r, c });
                    }
                }

                return costMap;

                // function
                int GetIndexWithMinCost(float[,] costMap, List<int[]> pointList)
                {
                    float minCost = float.MaxValue;
                    int minCostIndex = 0;

                    for (int n = 0; n < pointList.Count; n++)
                    {
                        int r = pointList[n][0];
                        int c = pointList[n][1];

                        float cost = costMap[r, c];

                        if (cost > minCost) { continue; }

                        minCost = cost;
                        minCostIndex = n;
                    }

                    return minCostIndex;
                }

                float Magnitude(int[] point1, int[] point2)
                {
                    int dz = point1[0] - point2[0];
                    int dx = point1[1] - point2[1];

                    return Mathf.Sqrt(Mathf.Pow(dz, 2.0f) + Mathf.Pow(dx, 2.0f));
                }
            }

            static int[] GetPointWithMaxCost(float[,] costMap)
            {
                int row = costMap.GetLength(0);
                int col = costMap.GetLength(1);

                float maxCost = 0.0f;

                int maxCostRow = 0;
                int maxCostCol = 0;

                for (var c = 0; c < col; c++)
                {
                    for (var r = 0; r < row; r++)
                    {
                        float cost = costMap[r, c];

                        if (cost < maxCost) { continue; }

                        maxCost = cost;
                        maxCostRow = r;
                        maxCostCol = c;
                    }
                }

                return new int[2] { maxCostRow, maxCostCol };
            }
        }
    }
}

