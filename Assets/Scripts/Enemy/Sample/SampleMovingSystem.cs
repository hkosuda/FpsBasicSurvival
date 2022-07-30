using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SampleMovingSystem : MonoBehaviour
    {
        static readonly float gridSize = 5.0f;

        static readonly int row = 4;
        static readonly int col = 6;

        [SerializeField] float speed = 2.0f;

        List<Vector3> path;

        private void Awake()
        {
            path = new List<Vector3>();
        }

        void Start()
        {
            RandomSpawn();
            SetEvent(1);
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                TimerSystem.Updated += UpdateMethod;
            }

            else
            {
                TimerSystem.Updated -= UpdateMethod;
            }
        }

        void UpdateMethod(object obj, float dt)
        {
            if (path == null || path.Count == 0)
            {
                ResetPath();
            }

            MoveOn(dt, speed);
        }

        void ResetPath()
        {
            var now = DateTime.Now;
            UnityEngine.Random.InitState(now.Millisecond);

            var rndCol = UnityEngine.Random.Range(0, col);
            var rndRow = UnityEngine.Random.Range(0, row);

            var field = GetField();
            var start = gameObject.transform.position;
            var goal = Point2Position(new int[2] { rndRow, rndCol });

            path = AStar.GetPath(field, start, goal, Point2Position, Position2Point);

            // - inner function
            static bool[,] GetField()
            {
                var field = new bool[row, col];

                for (var c = 0; c < col; c++)
                {
                    for (var r = 0; r < row; r++)
                    {
                        field[r, c] = true;
                    }
                }

                return field;
            }
        }

        void MoveOn(float dt, float speed)
        {
            if (path == null || path.Count == 0) { return; }

            var currentPosition = gameObject.transform.position;
            var nextPosition = currentPosition;

            var movingDistance = dt * speed;
            var movingDistanceRemain = movingDistance;

            var removingPathIndexes = new List<int>();

            for (int n = 0; n < path.Count; n++)
            {
                float disp2Next = (path[n] - currentPosition).magnitude;

                if (movingDistanceRemain < disp2Next)
                {
                    nextPosition = gameObject.transform.position + (path[n] - currentPosition).normalized * movingDistanceRemain;
                    break;
                }

                else
                {
                    movingDistanceRemain -= disp2Next;
                    removingPathIndexes.Add(n);
                    currentPosition = path[n];
                    nextPosition = path[n];
                }
            }

            if (removingPathIndexes.Count > 0)
            {
                if (path.Count == 1)
                {
                    path.Clear();
                }

                else
                {
                    for (int n = removingPathIndexes.Count - 1; n > -1; n--)
                    {
                        path.RemoveAt(removingPathIndexes[n]);
                    }
                }

                removingPathIndexes.Clear();
            }

            Face2MovingDirection(currentPosition, nextPosition);
            gameObject.transform.position = nextPosition;

            // - inner function
            void Face2MovingDirection(Vector3 originalPosition, Vector3 nextPosition)
            {
                var direction = (nextPosition - originalPosition).normalized;

                var theta = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

                gameObject.transform.rotation = Quaternion.Euler(0.0f, theta, 0.0f);
            }
        }

        static Vector3 Point2Position(int[] point, float y = 0.0f)
        {
            var row = point[0];
            var col = point[1];

            var z = gridSize * row - 2.5f;
            var x = gridSize * col - 12.5f;

            return new Vector3(x, y, z);
        }

        static int[] Position2Point(Vector3 position)
        {
            var z = position.z;
            var x = position.x;

            var row = Mathf.RoundToInt((z + 2.5f) / gridSize);
            var col = Mathf.RoundToInt((x + 12.5f) / gridSize);

            return new int[2] { row, col };
        }

        public void RandomSpawn()
        {
            var now = DateTime.Now;
            var id = Mathf.Abs(gameObject.GetInstanceID());

            UnityEngine.Random.InitState(id + now.Millisecond);

            var rndCol = UnityEngine.Random.Range(0, col);
            var rndRow = UnityEngine.Random.Range(0, row);

            var position = Point2Position(new int[2] { rndRow, rndCol });

            gameObject.transform.position = position;

            ResetPath();
        }

    }
}