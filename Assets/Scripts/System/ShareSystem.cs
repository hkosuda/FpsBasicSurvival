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
        static public PointToPosition Point2Position { get; set; } = SV_Point2Position;
        static public PositionToPoint Position2Point { get; set; } = SV_Position2Point;
        static public PositionToWallSize Position2WallSize { get; set; } = SV_Position2WallSize;

        static public bool[,] Passable { get; set; }

        static public Vector3 SV_Point2Position(int[] point, float height = 0.0f)
        {
            var z = GetPosition(point[0], SV_Map.wall_width, SV_Map.wall_depth);
            var x = GetPosition(point[1], SV_Map.wall_width, SV_Map.wall_depth);

            return new Vector3(x, height, z);

            // function
            static float GetPosition(int index, float width, float thickness)
            {
                if (index % 2 == 0)
                {
                    var r = Mathf.RoundToInt(index / 2);
                    return r * (thickness + width);
                }

                else
                {
                    var r = Mathf.RoundToInt((index - 1) / 2);
                    return (r + 0.5f) * (thickness + width);
                }
            }
        }

        static public int[] SV_Position2Point(Vector3 position)
        {
            var row = GetIndex(position.z, SV_Map.wall_width, SV_Map.wall_depth);
            var col = GetIndex(position.x, SV_Map.wall_width, SV_Map.wall_depth);

            return new int[2] { row, col };

            // function
            static int GetIndex(float position, float width, float thickness)
            {
                var r = Mathf.FloorToInt((position + thickness / 2) / (width + thickness));
                var dx = position - r * (width + thickness);

                if (dx < thickness)
                {
                    return 2 * r;
                }

                else
                {
                    return 2 * r + 1;
                }
            }
        }

        static public Vector3 SV_Position2WallSize(Vector3 position, float sizeY = 1.0f)
        {
            var point = Position2Point(position);

            var z = SV_Map.wall_depth;
            var x = SV_Map.wall_depth;

            if (point[0] % 2 == 1)
            {
                z = SV_Map.wall_width;
            }

            if (point[1] % 2 == 1)
            {
                x = SV_Map.wall_width;
            }

            return new Vector3(x, sizeY, z);
        }
    }

    
}

