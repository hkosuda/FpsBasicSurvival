using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SV_Map : HostComponent
    {
        static public readonly float wall_width = 8.0f;
        static public readonly float wall_depth = 2.0f;
        static public readonly float wall_height = 10.0f;

        static public GameObject MazeRoot { get; private set; }

        static public List<GameObject> Walls { get; private set; }
        static public GameObject Floor { get; private set; }
        static public GameObject Roof { get; private set; }

        static GameObject _wall;
        static GameObject _floor;
        static GameObject _roof;

        public override void Initialize()
        {
            if (_wall == null)
            {
                _wall = Resources.Load<GameObject>("SV/Wall");
            }

            if (_floor == null)
            {
                _floor = Resources.Load<GameObject>("SV/Floor");
            }

            if (_roof == null)
            {
                _roof = Resources.Load<GameObject>("SV/Roof");
            }
        }

        public override void Shutdown()
        {
            _wall = null;
        }

        public override void Stop()
        {

        }

        public override void Begin()
        {
            MazeRoot = new GameObject("MazeRoot");
            MazeRoot.transform.SetParent(GameHost.World.transform);

            SV_Seed.Init();

            var map = SvUtil_MazeLikeMapGenerator.Generate(SV_Round.MazeRow, SV_Round.MazeCol);
            var posSizeList = GetWallPositionAndSize(map);

            Walls = new List<GameObject>();

            foreach (var posSize in posSizeList)
            {
                var pos = new Vector3(posSize[0], 0.0f, posSize[1]);
                var size = new Vector3(posSize[2], wall_height, posSize[3]);

                var wall = GameHost.Instantiate(_wall, pos, Quaternion.identity);

                wall.transform.localScale = size;
                wall.transform.SetParent(MazeRoot.transform);
                Walls.Add(wall);
            }

            SetFloorRoof(map.GetLength(0), map.GetLength(1));
            UpdatePassable(map);

            // - inner function
            void UpdatePassable(bool[,] map)
            {
                var row = map.GetLength(0);
                var col = map.GetLength(1);

                ShareSystem.Passable = new bool[row, col];

                for (var x = 0; x < col; x++)
                {
                    for (var z = 0; z < row; z++)
                    {
                        if (map[z, x])
                        {
                            ShareSystem.Passable[z, x] = false;
                        }

                        else
                        {
                            ShareSystem.Passable[z, x] = true;
                        }
                    }
                }
            }

            // - inner function
            void SetFloorRoof(int row, int col)
            {
                var sizeZ = Mathf.RoundToInt((row - 1) / 2) * (wall_width + wall_depth) + wall_depth;
                var sizeX = Mathf.RoundToInt((col - 1) / 2) * (wall_width + wall_depth) + wall_depth;

                var centerZ = sizeZ / 2 - wall_depth / 2;
                var centerX = sizeX / 2 - wall_depth / 2;

                var pos = new Vector3(centerX, 0.0f, centerZ);
                var size = new Vector3(sizeX, 1.0f, sizeZ);

                var roof = GameHost.Instantiate(_roof, pos + new Vector3(0.0f, wall_height, 0.0f), Quaternion.identity);
                var floor = GameHost.Instantiate(_floor, pos, Quaternion.identity);

                roof.transform.localScale = size;
                floor.transform.localScale = size;

                roof.SetActive(false);
            }

            // - inner function
            // 0: x, 1: z, 2: size_x, 3: size_z
            List<float[]> GetWallPositionAndSize(bool[,] map)
            {
                // map : true -> wall
                var row = map.GetLength(0);
                var col = map.GetLength(1);

                var occupied = new bool[row, col];
                var posSizeList = new List<float[]>();

                for (var r = 0; r < row; r++)
                {
                    for (var c = 0; c < col; c++)
                    {
                        if (!map[r, c]) { continue; }
                        if (occupied[r, c]) { continue; }

                        var lateralPoints = GetLateralPoints(map, occupied, r, c);
                        var verticalPoints = GetVerticalPoints(map, occupied, r, c);

                        if (lateralPoints.Count > verticalPoints.Count)
                        {
                            occupied = GetLateralOccupiedPoints(lateralPoints, occupied, r);
                            posSizeList.Add(GetLateralPositionSize(lateralPoints, r));
                        }

                        else
                        {
                            occupied = GetVerticalOccupiedPoints(verticalPoints, occupied, c);
                            posSizeList.Add(GetVerticalPositionSize(verticalPoints, c));
                        }
                    }
                }

                return posSizeList;

                // - inner function
                static List<int> GetLateralPoints(bool[,] map, bool[,] occupied, int startRow, int startCol)
                {
                    var row = map.GetLength(0);
                    var col = map.GetLength(1);

                    var position = ShareSystem.Point2Position(new int[2] { startRow, startCol }, 0.0f);
                    var lateralPoints = new List<int>();

                    for (var c = startCol; c < col; c++)
                    {
                        if (!map[startRow, c] || occupied[startRow, c]) { break; }
                        lateralPoints.Add(c);
                    }

                    for (var c = startCol; c >= 0; c--)
                    {
                        if (!map[startRow, c] || occupied[startRow, c]) { break; }
                        if (c == startCol) { continue; }
                        lateralPoints.Add(c);
                    }

                    return lateralPoints;
                }

                // - inner function
                static List<int> GetVerticalPoints(bool[,] map, bool[,] occupied, int startRow, int startCol)
                {
                    var row = map.GetLength(0);
                    var col = map.GetLength(1);

                    var position = ShareSystem.Point2Position(new int[2] { startRow, startCol }, 0.0f);
                    var verticalPoints = new List<int>();

                    for (var r = startRow; r < row; r++)
                    {
                        if (!map[r, startCol] || occupied[r, startCol]) { break; }
                        verticalPoints.Add(r);
                    }

                    for (var r = startRow; r >= 0; r--)
                    {
                        if (!map[r, startCol] || occupied[r, startCol]) { break; }
                        if (r == startRow) { continue; }
                        verticalPoints.Add(r);
                    }

                    return verticalPoints;
                }

                // - inner function
                static bool[,] GetLateralOccupiedPoints(List<int> lateralPoints, bool[,] occupied, int row)
                {
                    foreach (var col in lateralPoints)
                    {
                        occupied[row, col] = true;
                    }

                    return occupied;
                }

                // - inner function
                static bool[,] GetVerticalOccupiedPoints(List<int> verticalPoints, bool[,] occupied, int col)
                {
                    foreach (var row in verticalPoints)
                    {
                        occupied[row, col] = true;
                    }

                    return occupied;
                }

                // - inner function
                static float[] GetLateralPositionSize(List<int> lateralPoints, int row)
                {
                    // 0: pox_x, 1: pos_z, 2: size_x, 3: size_z
                    var pos_size = new float[4];

                    var minX = float.MaxValue;
                    var maxX = float.MinValue;

                    for (var n = 0; n < lateralPoints.Count; n++)
                    {
                        var col = lateralPoints[n];
                        var pos = ShareSystem.Point2Position(new int[2] { row, col });
                        var size = ShareSystem.Position2WallSize(pos);

                        pos_size[1] = pos.z;
                        pos_size[3] = size.z;

                        var _minX = pos.x - size.x * 0.5f;
                        var _maxX = pos.x + size.x * 0.5f;

                        if (_minX < minX) { minX = _minX; }
                        if (_maxX > maxX) { maxX = _maxX; }
                    }

                    pos_size[0] = (minX + maxX) * 0.5f;
                    pos_size[2] = maxX - minX;

                    return pos_size;
                }

                // - inner function
                static float[] GetVerticalPositionSize(List<int> verticalPoints, int col)
                {
                    // 0: pox_x, 1: pos_z, 2: size_x, 3: size_z
                    var pos_size = new float[4];

                    var minZ = float.MaxValue;
                    var maxZ = float.MinValue;

                    for (var n = 0; n < verticalPoints.Count; n++)
                    {
                        var row = verticalPoints[n];
                        var pos = ShareSystem.Point2Position(new int[2] { row, col });
                        var size = ShareSystem.Position2WallSize(pos);

                        pos_size[0] = pos.x;
                        pos_size[2] = size.x;

                        var _minZ = pos.z - size.z * 0.5f;
                        var _maxZ = pos.z + size.z * 0.5f;

                        if (_minZ < minZ) { minZ = _minZ; }
                        if (_maxZ > maxZ) { maxZ = _maxZ; }
                    }

                    pos_size[1] = (minZ + maxZ) * 0.5f;
                    pos_size[3] = maxZ - minZ;

                    return pos_size;
                }
            }
        }
    }
}

