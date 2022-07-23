using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class TurretBrain : EnemyBrain
    {
        public EventHandler<bool> Detected { get; set; }

        enum Mode
        {
            roaming,
            shooting,
            tracking,
        }

        Mode mode;
        float missingTime;

        TurretShooter shootingSystem;

        private void Awake()
        {
            Init(EnemyType.mine);

            mode = Mode.roaming;
            shootingSystem = gameObject.GetComponent<TurretShooter>();
        }

        // Update is called once per frame
        protected override void UpdateMethod(object obj, float dt)
        {
            if (!InTheActiveRange()) { return; }
            IncrementCounter();

            if (mode == Mode.roaming)
            {
                IsTracking = false;

                if (SearchStrikerInRoamingMode(Const.enemy_detect_range))
                {
                    mode = Mode.shooting;
                    missingTime = 0.0f;
                    movingSystem.SetPath(new List<Vector3>());

                    shootingSystem.ResetCooldownTime();

                    IsTracking = true;

                    PlayerDetected?.Invoke(null, this);
                    Face2Target();
                }

                else
                {
                    UpdateMethodInRoaming(dt);
                }
            }

            else if (mode == Mode.shooting)
            {
                IsTracking = true;

                if (SearchPlayerInShootingMode(Const.enemy_detect_range))
                {
                    missingTime = 0.0f;
                    shootingSystem.Shoot(dt);

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

                    shootingSystem.ResetCooldownTime();

                    Face2Target();
                }

                else
                {
                    missingTime += dt;
                    var trackingDuration = SvParams.Get(SvParam.turret_tracking_duration);

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
        }

        protected bool SearchPlayerInShootingMode(float detectableRange)
        {
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
    }
}

