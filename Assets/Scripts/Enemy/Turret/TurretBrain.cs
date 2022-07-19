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

                if (SearchStrikerInRoamingMode(Params.turret_detect_range))
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

                if (SearchStrikerInRoamingMode(Params.turret_detect_range))
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
        }
    }
}

