using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SvUtil_MazeLikeMapGenerator
    {
        static public bool[,] Generate(int row, int col)
        {
            var spaceList = GetRandomSpaceList(row, col);
            var reservedPoints = GetReservedPointList(spaceList);

            var map = SvUtil_MazeGenerator.Generate(row, col, reservedPoints: reservedPoints);

            map = AddRooms(map, spaceList);
            map = CorrectWalls(map);

            return map;
        }

        static bool[,] CorrectWalls(bool[,] map)
        {
            var row = map.GetLength(0);
            var col = map.GetLength(1);

            for (var c = 0; c < col; c++)
            {
                map[0, c] = true;
                map[row - 1, c] = true;
            }

            for (var r = 0; r < row; r++)
            {
                map[r, 0] = true;
                map[r, col - 1] = true;
            }

            return map;
        }

        static List<MapSpace> GetRandomSpaceList(int row, int col)
        {
            var spaceList = new List<MapSpace>();
            var reserved = new bool[row, col];

            int counter = 0;

            for (int roopA = 0; roopA < row * col; roopA++)
            {
                var pointList = GetPointList(reserved);
                if (pointList.Count < 1) { break; }

                var isRoom = DecideRoomOrNot();
                var size = GetRandomSize(isRoom);

                var listLength = pointList.Count;

                for (int roopB = 0; roopB < listLength; roopB++)
                {
                    if (pointList.Count == 0) { break; }
                    int randomIndex = UnityEngine.Random.Range(0, pointList.Count);

                    var basePoint = pointList[randomIndex];
                    pointList.RemoveAt(randomIndex);

                    bool flag = true;

                    for (int c = 0; c < size[1]; c++)
                    {
                        for (int r = 0; r < size[0]; r++)
                        {
                            int rr = r + basePoint[0];
                            int cc = c + basePoint[1];

                            if (!Check(rr, cc, reserved)) { flag = false; break; }
                        }

                        if (!flag) { break; }
                    }

                    if (flag)
                    {
                        var pList = new List<int[]>();

                        for (int c = 0; c < size[1]; c++)
                        {
                            for (int r = 0; r < size[0]; r++)
                            {
                                int rr = r + basePoint[0];
                                int cc = c + basePoint[1];

                                reserved[rr, cc] = true;
                                pList.Add(new int[2] { rr, cc });
                            }

                            if (!flag) { break; }
                        }

                        spaceList.Add(new MapSpace(isRoom, pList));
                        counter++;

                        break;
                    }
                }
            }

            return spaceList;

            //
            // functio 
            static List<int[]> GetPointList(bool[,] reserved)
            {
                int row = reserved.GetLength(0);
                int col = reserved.GetLength(1);

                var pointList = new List<int[]>();

                for (int c = 0; c < col; c++)
                {
                    for (int r = 0; r < row; r++)
                    {
                        if (reserved[r, c]) { continue; }

                        pointList.Add(new int[2] { r, c });
                    }
                }

                return pointList;
            }

            static bool Check(int r, int c, bool[,] reserved)
            {
                int row = reserved.GetLength(0);
                int col = reserved.GetLength(1);

                if (r < 0 || r >= row) { return false; }
                if (c < 0 || c >= col) { return false; }

                for (int n = 0; n < SvUtil.PanelPoints.GetLength(0); n++)
                {
                    int rr = r + SvUtil.PanelPoints[n, 0];
                    int cc = c + SvUtil.PanelPoints[n, 1];

                    if (rr < 0 || rr >= row) { continue; }
                    if (cc < 0 || cc >= col) { continue; }

                    if (reserved[rr, cc]) { return false; }
                }

                return true;
            }

            static bool DecideRoomOrNot()
            {
                var val = Random.Range(0.0f, 1.0f);

                if (val < Params.sv_room_space_ratio)
                {
                    return true;
                }

                return false;
            }

            static int[] GetRandomSize(bool isRoom)
            {
                if (isRoom)
                {
                    int z = Random.Range(3, 4);
                    int x = Random.Range(3, 4);

                    return new int[2] { z, x };
                }

                else
                {
                    int z = Random.Range(1, 2);
                    int x = Random.Range(1, 2);

                    return new int[2] { z, x };
                }
            }
        }

        static List<int[]> GetReservedPointList(List<MapSpace> spaces)
        {
            var reservedList = new List<int[]>();

            foreach (var space in spaces)
            {
                foreach (var point in space.pointList)
                {
                    reservedList.Add(point);
                }
            }

            return reservedList;
        }

        static bool[,] AddRooms(bool[,] map, List<MapSpace> spaceList)
        {
            foreach (var space in spaceList)
            {
                if (space.isRoom)
                {
                    map = space.AddRoom(map);
                }
            }

            return map;
        }
    }

    public class MapSpace
    {
        public bool isRoom;
        public List<int[]> pointList;

        public MapSpace(bool isRoom, List<int[]> pointList)
        {
            this.isRoom = isRoom;
            this.pointList = pointList;
        }

        public bool[,] AddRoom(bool[,] map)
        {
            var mazeRow = map.GetLength(0);
            var mazeCol = map.GetLength(1);

            var minRow = int.MaxValue;
            var maxRow = 0;
            var minCol = int.MaxValue;
            var maxCol = 0;

            foreach (var p in pointList)
            {
                p[0] *= 2;
                p[1] *= 2;

                if (p[0] < minRow) { minRow = p[0]; }
                if (p[0] > maxRow) { maxRow = p[0]; }

                if (p[1] < minCol) { minCol = p[1]; }
                if (p[1] > maxCol) { maxCol = p[1]; }
            }

            for (var c = minCol; c <= maxCol; c++)
            {
                map[minRow, c] = true;
                map[maxRow, c] = true;
            }

            for (int r = minRow; r <= maxRow; r++)
            {
                map[r, minCol] = true;
                map[r, maxCol] = true;
            }

            var idxList = GetRandomList();
            var gates = Random.Range(2, 4);

            var n_gates = 0;

            foreach (var idx in idxList)
            {
                if (n_gates >= gates) { break; }

                if (MakeGate(map, minRow, maxRow, minCol, maxCol, idx))
                {
                    n_gates++;
                }
            }

            if (n_gates == 0) { map = PaddingClosedRoom(map, minRow, maxRow, minCol, maxCol); }

            return map;

            // 
            // function
            List<int> GetRandomList()
            {
                var numList = new List<int> { 0, 1, 2, 3 };
                var randomList = new List<int>();

                for (int n = 0; n < 4; n++)
                {
                    int idx = Random.Range(0, numList.Count);
                    int num = numList[idx];
                    randomList.Add(num);

                    numList.RemoveAt(idx);
                }

                return randomList;
            }

            bool MakeGate(bool[,] map, int minRow, int maxRow, int minCol, int maxCol, int idx)
            {
                var halfRow = Mathf.RoundToInt((minRow + maxRow) / 2);
                var halfCol = Mathf.RoundToInt((minCol + maxCol) / 2);

                if (idx == 0)
                {
                    if (minCol - 1 > 0)
                    {
                        if (map[halfRow, minCol - 1]) { return false; }
                        map[halfRow, minCol] = false;
                        return true;
                    }
                }

                if (idx == 1)
                {
                    if (maxCol + 1 < mazeCol)
                    {
                        if (map[halfRow, maxCol + 1]) { return false; }
                        map[halfRow, maxCol] = false;
                        return true;
                    }
                }

                if (idx == 2)
                {
                    if (minRow - 1 > 0)
                    {
                        if (map[minRow - 1, halfCol]) { return false; }
                        map[minRow, halfCol] = false;
                        return true;
                    }
                }

                if (idx == 3)
                {
                    if (maxRow + 1 < mazeRow)
                    {
                        if (map[maxRow + 1, halfCol]) { return false; }
                        map[maxRow, halfCol] = false;
                        return true;
                    }
                }
                   
                return false;
            }

            bool[,] PaddingClosedRoom(bool[,] map, int minRow, int maxRow, int minCol, int maxCol)
            {
                for (int c = minCol; c <= maxCol; c++)
                {
                    for (int r = minRow; r <= maxRow; r++)
                    {
                        map[r, c] = true;
                    }
                }

                return map;
            }
        }
    }
}
