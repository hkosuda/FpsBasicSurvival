using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SvUtil_MazeGenerator
    {
        class Node
        {
            public enum Status
            {
                open,
                close,
                none,
            }

            public Status status;
            public int[] point;
            public Node parent;
            public bool[] lines;
            public bool[] searched;
            public bool reserved;
        }

        static Node[,] nodes;
        static List<int[]> indexList;

        /// <summary>
        /// True : Wall, False : Space
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="reservedPoints"></param>
        /// <returns></returns>

        static public bool[,] Generate(int row, int col, List<int[]> reservedPoints = null)
        {
            nodes = GetInitialNodes(row, col, reservedPoints);
            indexList = GetInitialNodeIndexList(nodes);

            for (int roopA = 0; roopA < int.MaxValue; roopA++)
            {
                if (indexList.Count == 0) { break; }

                // initial node
                var baseIndex = indexList[Random.Range(0, indexList.Count)];
                Get(baseIndex).status = Node.Status.open;
                Get(baseIndex).parent = null;

                // search
                for (int roopB = 0; roopB < int.MaxValue; roopB++)
                {
                    var dir = GetValidDirection(baseIndex);

                    if (dir < 0)
                    {
                        baseIndex = Get(baseIndex).parent.point;
                        continue;
                    }

                    var nextIndex = new int[2] { baseIndex[0] + SvUtil.CrossPoints[dir, 0], baseIndex[1] + SvUtil.CrossPoints[dir, 1] };

                    Get(baseIndex).searched[dir] = true;

                    if (Get(nextIndex).status == Node.Status.close)
                    {
                        Get(baseIndex).lines[dir] = true;

                        CloseNodes(baseIndex);
                        break;
                    }

                    else if (Get(nextIndex).status == Node.Status.open)
                    {
                        var parentIndex = Get(baseIndex).parent.point;
                        baseIndex = new int[2] { parentIndex[0], parentIndex[1] };
                    }

                    else if (Get(nextIndex).status == Node.Status.none)
                    {
                        Get(nextIndex).searched[(dir + 2) % 4] = true;
                        Get(nextIndex).parent = Get(baseIndex);
                        Get(nextIndex).status = Node.Status.open;

                        baseIndex = new int[2] { nextIndex[0], nextIndex[1] };
                    }
                }

                ResetNodes();
            }

            var maze = Unpack();
            maze = CorrectMaze(maze);

            return maze;
        }

        //
        // function 
        static Node Get(int[] index)
        {
            return nodes[index[0], index[1]];
        }

        static Node[,] GetInitialNodes(int row, int col, List<int[]> reservedPoints)
        {
            Node[,] nodes = new Node[row, col];

            for (int x = 0; x < col; x++)
            {
                for (int z = 0; z < row; z++)
                {
                    nodes[z, x] = new Node
                    {
                        status = Node.Status.none,
                        point = new int[2] { z, x },
                        parent = null,
                        lines = new bool[4] { false, false, false, false },
                        searched = new bool[4] { false, false, false, false },
                        reserved = false,
                    };
                }
            }

            if (reservedPoints != null)
            {
                foreach (var p in reservedPoints)
                {
                    nodes[p[0], p[1]].status = Node.Status.close;
                    nodes[p[0], p[1]].reserved = true;
                }
            }

            // draw lines to right direction
            for (int x = 0; x < (col - 1); x++)
            {
                // lines[1] ... right
                nodes[0, x].lines[1] = true;
                nodes[0, x].status = Node.Status.close;

                nodes[(row - 1), x].lines[1] = true;
                nodes[(row - 1), x].status = Node.Status.close;
            }

            // draw lines to upper direction
            for (int z = 0; z < (row - 1); z++)
            {
                // lines[0] ... upper
                nodes[z, 0].lines[0] = true;
                nodes[z, 0].status = Node.Status.close;

                nodes[z, (col - 1)].lines[0] = true;
                nodes[z, (col - 1)].status = Node.Status.close;
            }

            nodes[(row - 1), (col - 1)].status = Node.Status.close;

            return nodes;
        }

        static List<int[]> GetInitialNodeIndexList(Node[,] nodes)
        {
            int row = nodes.GetLength(0);
            int col = nodes.GetLength(1);

            var nodeIndexList = new List<int[]>();

            for (int x = 0; x < col; x++)
            {
                for (int z = 0; z < row; z++)
                {
                    if (nodes[z, x].status == Node.Status.close) { continue; }

                    nodeIndexList.Add(new int[2] { z, x });
                }
            }

            return nodeIndexList;
        }

        static void CloseNodes(int[] baseIndex)
        {
            for (int roopC = 0; roopC < int.MaxValue; roopC++)
            {
                Get(baseIndex).status = Node.Status.close;
                RemoveIndex(baseIndex);

                if (Get(baseIndex).parent == null) { break; }

                var dz = Get(baseIndex).parent.point[0] - Get(baseIndex).point[0];
                var dx = Get(baseIndex).parent.point[1] - Get(baseIndex).point[1];

                for (int n = 0; n < 4; n++)
                {
                    if (dz != SvUtil.CrossPoints[n, 0] || dx != SvUtil.CrossPoints[n, 1]) { continue; }
                    Get(baseIndex).lines[n] = true;
                    break;
                }

                var parentIndex = Get(baseIndex).parent.point;
                baseIndex = new int[2] { parentIndex[0], parentIndex[1] };
            }

            //
            // functions
            static void RemoveIndex(int[] index)
            {
                int removeAt = 0;

                for (int n = 0; n < indexList.Count; n++)
                {
                    if (index[0] == indexList[n][0] && index[1] == indexList[n][1])
                    {
                        removeAt = n;
                        break;
                    }
                }

                indexList.RemoveAt(removeAt);
            }
        }

        static void ResetNodes()
        {
            int row = nodes.GetLength(0);
            int col = nodes.GetLength(1);

            for (int x = 0; x < col; x++)
            {
                for (int z = 0; z < row; z++)
                {
                    if (nodes[z, x].status == Node.Status.close) { continue; }

                    nodes[z, x].parent = null;
                    nodes[z, x].searched = new bool[4] { false, false, false, false };
                    nodes[z, x].status = Node.Status.none;
                }
            }
        }

        static bool[,] Unpack()
        {
            int nodeRow = nodes.GetLength(0);
            int nodeCol = nodes.GetLength(1);

            int mazeRow = 2 * nodeRow - 1;
            int mazeCol = 2 * nodeCol - 1;

            bool[,] maze = new bool[mazeRow, mazeCol];

            for (int x = 0; x < nodeCol; x++)
            {
                for (int z = 0; z < nodeRow; z++)
                {
                    if (nodes[z, x].reserved) { continue; }

                    var node = nodes[z, x];
                    maze[2 * z, 2 * x] = true;

                    for (int n = 0; n < 4; n++)
                    {
                        if (!node.lines[n]) continue;

                        int zz = 2 * z + SvUtil.CrossPoints[n, 0];
                        int xx = 2 * x + SvUtil.CrossPoints[n, 1];

                        var spot = new int[2] { zz, xx };

                        if (spot[0] < 0 || spot[0] > mazeRow - 1) continue;
                        if (spot[1] < 0 || spot[1] > mazeCol - 1) continue;

                        maze[spot[0], spot[1]] = true;
                    }
                }
            }

            return maze;
        }

        static int GetValidDirection(int[] index)
        {
            var validDirList = new List<int>();

            for (int m = 0; m < 4; m++)
            {
                if (Get(index).searched[m]) { continue; }
                validDirList.Add(m);
            }

            if (validDirList.Count == 0) { return -1; }

            return validDirList[Random.Range(0, validDirList.Count)];
        }

        static bool[,] CorrectMaze(bool[,] maze)
        {
            int row = maze.GetLength(0);
            int col = maze.GetLength(1);

            for (int c = 1; c < (col - 1); c++)
            {
                for (int r = 1; r < (row - 1); r++)
                {
                    if (maze[r, c]) { continue; }

                    for (int n = 0; n < 4; n++)
                    {
                        var cross1 = new int[2] { SvUtil.CrossPoints[(n + 0) % 4, 0], SvUtil.CrossPoints[(n + 0) % 4, 1] };
                        var cross2 = new int[2] { SvUtil.CrossPoints[(n + 1) % 4, 0], SvUtil.CrossPoints[(n + 1) % 4, 1] };

                        var true1z = r + cross1[0];
                        var true1x = c + cross1[1];

                        var true2z = r + cross2[0];
                        var true2x = c + cross2[1];

                        var false1z = r + cross1[0] + cross2[0];
                        var false1x = c + cross1[1] + cross2[1];

                        var false2z = r - cross1[0] - cross2[0];
                        var false2x = c - cross1[1] - cross2[1];

                        if (!maze[true1z, true1x] || !maze[true2z, true2x]) { continue; }
                        if (maze[false1z, false1x] || maze[false2z, false2x]) { continue; }

                        maze[r, c] = true;
                        break;
                    }
                }
            }

            return maze;
        }
    }
}

