using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class TurretBrain : EnemyBrain
    {
        public EventHandler<bool> FindStriker;

        public int ID { get; set; } = 0;

        float missingTime;
        int roamingCounter;

        enum Mode
        {
            roaming,
            shooting,
            tracking,
        }

        MovingSystem movingSystem;
        TurretShooter shootingSystem;

        Mode mode;
        int counter;

        private void Awake()
        {
            IsTracking = false;
            EnemyType = EnemyType.turret;

            movingSystem = gameObject.GetComponent<MovingSystem>();
            shootingSystem = gameObject.GetComponent<TurretShooter>();

            mode = Mode.roaming;

            counter = UnityEngine.Random.Range(0, 5);
            roamingCounter = 0;

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
            if (Player.Myself == null) { Debug.Log("Player not found"); return; }
            if (OutOfRange()) { Debug.Log("Out Of Range"); return; }

            CounterIncrement();

            if (mode == Mode.roaming)
            {
                IsTracking = false;

                if (SearchStrikerInRoamingMode())
                {
                    mode = Mode.shooting;
                    missingTime = 0.0f;
                    movingSystem.SetPath(new List<Vector3>());

                    shootingSystem.ResetCooldownTime();

                    IsTracking = true;

                    FindStriker?.Invoke(null, false);
                    DetectedPlayer?.Invoke(null, this);
                    Face2Target();
                }

                else
                {
                    RoamingUpdateMethod(dt);
                }
            }

            else if (mode == Mode.shooting)
            {
                IsTracking = true;

                if (SearchStrikerInRoamingMode())
                {
                    missingTime = 0.0f;
                    shootingSystem.Shoot();

                    Face2Target();
                }

                else
                {
                    mode = Mode.tracking;
                }
            }

            else if (mode == Mode.tracking)
            {
                IsTracking = true;

                if (SearchStrikerInTrackingMode())
                {
                    missingTime = 0.0f;
                    mode = Mode.shooting;

                    Face2Target();
                }

                else
                {
                    missingTime += dt;
                    var trackingDuration = Params.turret_tracking_duration;

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
            bool SearchStrikerInRoamingMode()
            {
                var originPosition = gameObject.transform.position;
                var targetPosition = Player.Myself.transform.position;

                var dX = targetPosition - originPosition;

                // range test
                var range = Params.turret_detect_range;

                if (dX.magnitude > range) { return false; }

                // angle test
                var rotY = gameObject.transform.eulerAngles.y;
                var theta = Mathf.Atan2(dX.x, dX.z) * Mathf.Rad2Deg;

                var deltaTheta = Calcf.AngleDelta(rotY, theta);
                if (Mathf.Abs(deltaTheta) > 90.0f) { return false; }

                // raycast test
                Physics.Raycast(originPosition, dX.normalized, out RaycastHit hit, dX.magnitude, Const.bounceLayer);
                if (hit.collider != null) { return false; }

                return true;
            }

            bool SearchStrikerInTrackingMode()
            {
                var originPosition = new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z);
                var targetPosition = new Vector3(Player.Myself.transform.position.x, 0.0f, Player.Myself.transform.position.z);

                var dX = targetPosition - originPosition;

                // raycast test
                Physics.Raycast(originPosition, dX.normalized, out RaycastHit hit, dX.magnitude, Const.bounceLayer);
                if (hit.collider != null) { return false; }

                return true;
            }

            // function
            void Face2Target()
            {
                var dx = Player.Myself.transform.position.x - gameObject.transform.position.x;
                var dz = Player.Myself.transform.position.z - gameObject.transform.position.z;
                var rotY = Mathf.Atan2(dx, dz) * Mathf.Rad2Deg;

                gameObject.transform.rotation = Quaternion.Euler(0.0f, rotY, 0.0f);
            }

            // function
            void RoamingUpdateMethod(float dt)
            {
                if (movingSystem.PathLength() == 0)
                {
                    var startPosition = gameObject.transform.position;

                    roamingCounter++;
                    var id2 = Mathf.RoundToInt(Mathf.Pow(ID, 2)) + 20;
                    SV_Seed.Init(id2 + roamingCounter);
                    var goalPosition = GetRandomPosition();
                    var field = ShareSystem.Passable;

                    movingSystem.SetPath(AStar.GetPath(field, startPosition, goalPosition));
                }

                var speed = Params.turret_roaming_speed;
                movingSystem.MoveOn(dt, speed);

                // function
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
            }

            // function
            void TrackingUpdateMethod(GameObject target, float dt, bool updatePath)
            {
                if (target == null) { return; }
                if (movingSystem == null) { return; }

                if (counter == 0)
                {
                    if (updatePath)
                    {
                        var startPosition = gameObject.transform.position;
                        var goalPosition = target.transform.position;

                        var path = RaycastPathSearch.GetPath(startPosition, goalPosition, 1 << 6);

                        movingSystem.SetPath(path);
                    }
                }

                var speed = Params.turret_tracking_speed;
                movingSystem.MoveOn(dt, speed);
            }

            // - inner function
            bool OutOfRange()
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
    }
}

