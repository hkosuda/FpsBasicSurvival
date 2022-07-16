using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public delegate Vector3 PointToPosition(int[] point, float height = 0.0f);
    public delegate int[] PositionToPoint(Vector3 position);
    public delegate Vector3 PositionToWallSize(Vector3 position, float sizeY = 1.0f);

    public class ShareSystem : MonoBehaviour
    {
        static public PointToPosition Point2Position { get; set; }
        static public PositionToPoint Position2Point { get; set; }
        static public PositionToWallSize Position2WallSize { get; set; }

        static public bool[,] Passable { get; set; }
    }
}

