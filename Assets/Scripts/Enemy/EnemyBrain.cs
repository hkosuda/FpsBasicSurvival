using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public abstract class EnemyBrain : MonoBehaviour
    {
        static protected readonly int updateCycle = 6;
        static protected readonly float yOffset = 1.0f;

        static public EventHandler<EnemyBrain> PlayerDetected { get; set; }

        public int ID { get; set; } = 0;
        public bool IsTracking { get; protected set; }
        public EnemyType EnemyType { get; protected set; }

        protected MovingSystem movingSystem;
        protected int roamingCounter;
        protected int updateCounter;

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

        protected abstract void UpdateMethod(object obj, float dt);

        protected void Init(EnemyType enemyType)
        {
            EnemyType = enemyType;
            IsTracking = false;

            movingSystem = gameObject.GetComponent<MovingSystem>();
            roamingCounter = Mathf.RoundToInt(Mathf.Pow(ID, 3));
            updateCounter = roamingCounter % updateCycle;
        }

        protected void IncrementCounter()
        {
            updateCounter = (updateCounter + 1) % updateCycle;
        }

        protected void Face2Target()
        {
            var dx = Player.Myself.transform.position.x - gameObject.transform.position.x;
            var dz = Player.Myself.transform.position.z - gameObject.transform.position.z;
            var rotY = Mathf.Atan2(dx, dz) * Mathf.Rad2Deg;

            gameObject.transform.rotation = Quaternion.Euler(0.0f, rotY, 0.0f);
        }

        protected bool SearchStrikerInRoamingMode(float detectableRange)
        {
            if (updateCounter != 0) { return false; }

            var sightAngle = 90.0f;

            var originPosition = new Vector3(gameObject.transform.position.x, yOffset, gameObject.transform.position.z);
            var targetPosition = new Vector3(Player.Myself.transform.position.x, yOffset, Player.Myself.transform.position.z);

            var dX = targetPosition - originPosition;
            if (dX.magnitude > detectableRange) { return false; }

            var rotY = gameObject.transform.eulerAngles.y;
            var theta = Mathf.Atan2(dX.x, dX.z) * Mathf.Rad2Deg;

            var deltaTheta = Calcf.AngleDelta(rotY, theta);
            if (Mathf.Abs(deltaTheta) > sightAngle) { return false; }

            Physics.Raycast(originPosition, dX.normalized, out RaycastHit hit, dX.magnitude);
            if (hit.collider == null) { return false; }

            return hit.collider.gameObject.layer == Const.playerLayer;
        }

        protected bool SearchStrikerInTrackingMode()
        {
            if (updateCounter == 0)
            {
                var originPosition = new Vector3(gameObject.transform.position.x, yOffset, gameObject.transform.position.z);
                var targetPosition = new Vector3(Player.Myself.transform.position.x, yOffset, Player.Myself.transform.position.z);

                var dX = targetPosition - originPosition;

                // raycast test
                Physics.Raycast(originPosition, dX.normalized, out RaycastHit hit, dX.magnitude, Const.bounceLayer);
                if (hit.collider != null) { return false; }

                return true;
            }

            else
            {
                return false;
            }
        }

        protected void UpdateMethodInRoaming(float dt, float speed)
        {
            if (movingSystem.PathLength() == 0)
            {
                var startPosition = gameObject.transform.position;
                var goalPosition = GetRandomPosition();

                SetSeed();

                var field = ShareSystem.Passable;
                movingSystem.SetPath(AStar.GetPath(field, startPosition, goalPosition));
            }

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

        protected void TrackingUpdateMethod(GameObject target, float dt, bool updatePath, float speed)
        {
            if (updateCounter == 0)
            {
                if (updatePath)
                {
                    var startPosition = gameObject.transform.position;
                    var goalPosition = target.transform.position;

                    var path = RaycastPathSearch.GetPath(startPosition, goalPosition, 1 << 6);

                    movingSystem.SetPath(path);
                }
            }

            movingSystem.MoveOn(dt, speed);
        }

        protected bool InTheActiveRange()
        {
            var distance = (Player.Myself.transform.position - gameObject.transform.position).magnitude;
            return distance < Const.enemy_active_range;
        }
    }
}

