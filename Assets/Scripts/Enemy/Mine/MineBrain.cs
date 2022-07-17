using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class MineBrain : EnemyBrain
    {
        public EventHandler<bool> PlayerDetected { get; set; }

        public int ID { get; set; } = 0;

        float missingTime;
        int roamingCounter;

        enum Mode
        {
            roaming,
            tracking,
        }

        MovingSystem movingSystem;
        Mode mode;
        int counter;

        private void Awake()
        {
            IsTracking = false;
            EnemyType = EnemyType.mine;

            movingSystem = gameObject.GetComponent<MovingSystem>();

            mode = Mode.roaming;
            roamingCounter = Mathf.RoundToInt(Mathf.Pow(ID, 3));
        }

        private void Start()
        {
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

        // Update is called once per frame
        void UpdateMethod(object obj, float dt)
        {
            if (Player.Myself == null) { return; }
            if (CheckDistance()) { return; }

            CounterIncrement();

            if (mode == Mode.roaming)
            {
                IsTracking = false;

                if (SearchStrikerInRoamingMode())
                {
                    mode = Mode.tracking;
                    missingTime = 0.0f;

                    IsTracking = true;
                    movingSystem.SetPath(new List<Vector3>());
                    Face2Target();

                    DetectedPlayer?.Invoke(null, this);
                }

                else
                {
                    RoamingUpdateMethod(dt);
                }
            }

            else if (mode == Mode.tracking)
            {
                IsTracking = true;

                if (SearchStrikerInTrackingMode())
                {
                    missingTime = 0.0f;
                    TrackingUpdateMethod(Player.Myself, dt, true);

                    Face2Target();
                }

                else
                {
                    missingTime += dt;
                    var trackingDuration = Params.mine_tracking_duration;

                    if (movingSystem.PathLength() == 0 && missingTime > trackingDuration)
                    {
                        mode = Mode.roaming;
                    }

                    else if (movingSystem.PathLength() == 0)
                    {
                        TrackingUpdateMethod(Player.Myself, dt, true);
                    }

                    else
                    {
                        TrackingUpdateMethod(Player.Myself, dt, false);
                    }
                }
            }

            // function 
            void CounterIncrement()
            {
                counter = (counter + 1) % 5;
            }

            // function
            bool CheckDistance()
            {
                var distance = (Player.Myself.transform.position - gameObject.transform.position).magnitude;

                if (distance > Params.enemy_active_range)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }
        }

        // function
        void Face2Target()
        {
            var dx = Player.Myself.transform.position.x - gameObject.transform.position.x;
            var dz = Player.Myself.transform.position.z - gameObject.transform.position.z;
            var rotY = Mathf.Atan2(dx, dz) * Mathf.Rad2Deg;

            gameObject.transform.rotation = Quaternion.Euler(0.0f, rotY, 0.0f);
        }

        bool SearchStrikerInRoamingMode()
        {
            var originPosition = new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z);
            var targetPosition = new Vector3(Player.Myself.transform.position.x, 0.0f, Player.Myself.transform.position.z);

            var dX = targetPosition - originPosition;

            var range = Params.mine_detect_range;
            if (dX.magnitude > range) { return false; }

            var rotY = gameObject.transform.eulerAngles.y;
            var theta = Mathf.Atan2(dX.x, dX.z) * Mathf.Rad2Deg;

            var deltaTheta = Calcf.AngleDelta(rotY, theta);

            if (Mathf.Abs(deltaTheta) > 90.0f) { return false; }
            Physics.Raycast(originPosition, dX.normalized, out RaycastHit hit, dX.magnitude, Const.bounceLayer);

            if (hit.collider == null) { return true; }
            return false;
        }

        bool SearchStrikerInTrackingMode()
        {
            var originPosition = new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z);
            var targetPosition = new Vector3(Player.Myself.transform.position.x, 0.0f, Player.Myself.transform.position.z);

            var dX = targetPosition - originPosition;

            Physics.Raycast(originPosition, dX.normalized, out RaycastHit hit, dX.magnitude, Const.bounceLayer);

            if (hit.collider == null) { return true; }
            return false;
        }

        public void RoamingUpdateMethod(float dt)
        {
            if (movingSystem.PathLength() == 0)
            {
                var startPosition = gameObject.transform.position;
                var goalPosition = GetRandomPosition();

                SetSeed();

                var field = ShareSystem.Passable;
                movingSystem.SetPath(AStar.GetPath(field, startPosition, goalPosition));
            }

            var speed = Params.mine_roaming_speed;
            movingSystem.MoveOn(dt, speed);

            // - inner function
            static Vector3 GetRandomPosition()
            {
                if (GameSystem.CurrentHost.HostName == HostName.survival)
                {
                    var randomPointList = SvUtil.GetRandomBlankPointList(new List<int[]> { SV_GoalStart.StartPoint, SV_GoalStart.GoalPoint });
                    var point = randomPointList[0];
                    return ShareSystem.Point2Position(point, 0.0f);
                }

                else
                {
                    var randomPointList = SvUtil.GetRandomBlankPointList();
                    var point = randomPointList[0];
                    return ShareSystem.Point2Position(point, 0.0f);
                }
            }

            // - inner function
            void SetSeed()
            {
                roamingCounter++;
                var id_value = Mathf.RoundToInt(Mathf.Pow(ID, 2)) + 10;

                SV_Seed.Init(id_value + roamingCounter);
            }
        }

        public void TrackingUpdateMethod(GameObject target, float dt, bool updatePath)
        {
            if (updatePath)
            {
                var startPosition = gameObject.transform.position;
                var goalPosition = target.transform.position;

                var path = RaycastPathSearch.GetPath(startPosition, goalPosition, 1 << 6);

                movingSystem.SetPath(path);
            }

            var speed = Params.mine_tracking_speed;
            movingSystem.MoveOn(dt, speed);
        }
    }
}
