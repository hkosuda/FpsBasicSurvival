using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class CompassItem : FieldItem
    {
        static GameObject root;
        static bool[,,] flag;

        static GameObject _dashLine;

        private void Awake()
        {
            if (_dashLine == null) { _dashLine = Resources.Load<GameObject>("SV/Navigation"); }

            Init(Item.compass);
        }

        protected override bool ItemMethod()
        {
            SV_Status.AddMoney(SvParams.GetInt(SvParam.field_compass_money_amount));
            BeginNavigation(gameObject.transform.position);

            return true;
        }

        static bool BeginNavigation(Vector3 origin)
        {
            Initialize();
            GenerateNavigation(origin);

            return true;

            //
            //
            // function
            static void Initialize()
            {
                if (root == null)
                {
                    var noObject = ShareSystem.Passable;

                    var row = noObject.GetLength(0);
                    var col = noObject.GetLength(1);

                    flag = new bool[row, col, 4];

                    root = new GameObject("CompassNavigationRoot");
                    root.transform.SetParent(GameHost.World.transform);
                }
            }

            static void GenerateNavigation(Vector3 startPosition)
            {
                var goalPosition = ShareSystem.Point2Position(SV_GoalStart.GoalPoint, 0.0f);

                var path = AStar.GetPath(ShareSystem.Passable, startPosition, goalPosition);

                if (path.Count < 2) { return; }

                for (int n = 0; n < (path.Count - 1); n++)
                {
                    var position1 = path[n];
                    var position2 = path[n + 1];

                    var point1 = ShareSystem.Position2Point(position1);
                    var point2 = ShareSystem.Position2Point(position2);

                    var len_x = Mathf.Abs(position2.x - position1.x);
                    var len_z = Mathf.Abs(position2.z - position1.z);

                    int _lineDirection;
                    int _moveDirection;
                    float basePosition;

                    // direction : Z -> 1
                    if (len_z > len_x)
                    {
                        // point1 -> point2
                        if (position2.z > position1.z)
                        {
                            if (flag[point1[0], point1[1], 0] || flag[point2[0], point2[1], 2]) { continue; }

                            flag[point1[0], point1[1], 0] = true;
                            flag[point2[0], point2[1], 2] = true;

                            _lineDirection = 1;
                            _moveDirection = 1;

                            basePosition = position1.z;
                        }

                        // point2 -> point1
                        else
                        {
                            if (flag[point1[0], point1[1], 2] || flag[point2[0], point2[1], 0]) { continue; }

                            flag[point1[0], point1[1], 2] = true;
                            flag[point2[0], point2[1], 0] = true;

                            _lineDirection = 1;
                            _moveDirection = -1;

                            basePosition = position2.z;
                        }
                    }

                    // direction : X -> -1
                    else
                    {
                        // point1 -> point2
                        if (position2.x > position1.x)
                        {
                            if (flag[point1[0], point1[1], 1] || flag[point2[0], point2[1], 3]) { continue; }

                            flag[point1[0], point1[1], 1] = true;
                            flag[point2[0], point2[1], 3] = true;

                            _lineDirection = -1;
                            _moveDirection = 1;

                            basePosition = position1.x;
                        }

                        else
                        {
                            if (flag[point1[0], point1[1], 3] || flag[point2[0], point2[1], 1]) { continue; }

                            flag[point1[0], point1[1], 3] = true;
                            flag[point2[0], point2[1], 1] = true;

                            _lineDirection = -1;
                            _moveDirection = -1;

                            basePosition = position2.x;
                        }
                    }

                    var renderer = GenerateLine(position1, position2);
                    SetRenderer(renderer, _lineDirection, _moveDirection, basePosition);
                }

                //
                // function
                static Renderer GenerateLine(Vector3 position1, Vector3 position2)
                {
                    var mid = (position1 + position2) / 2.0f;
                    var size = (position2 - position1).magnitude / 2.0f;
                    var theta = Mathf.Atan2(position2.x - position1.x, position2.z - position1.z) * Mathf.Rad2Deg;

                    var dashLine = GameObject.Instantiate(_dashLine);

                    dashLine.transform.position = mid;
                    dashLine.transform.localScale = new Vector3(1.0f, 1.0f, size);
                    dashLine.transform.rotation = Quaternion.Euler(0.0f, theta, 0.0f);

                    dashLine.transform.SetParent(root.transform);

                    var lineBody = dashLine.transform.GetChild(0).gameObject;
                    var renderer = lineBody.GetComponent<Renderer>();

                    return renderer;
                }

                //
                // function
                static void SetRenderer(Renderer renderer, float lineDirection, float movingDirection, float basePosition)
                {
                    renderer.material.SetFloat("_LineDirection", lineDirection);
                    renderer.material.SetFloat("_MoveDirection", movingDirection);
                    renderer.material.SetFloat("_BasePosition", basePosition);
                }
            }
        }
    }
}

