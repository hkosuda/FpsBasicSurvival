using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class MineBrain : EnemyBrain
    {
        public EventHandler<bool> Detected { get; set; }

        enum Mode
        {
            roaming,
            tracking,
        }

        Mode mode;
        float missingTime;

        private void Awake()
        {
            Init(EnemyType.mine);
            mode = Mode.roaming;
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
                    mode = Mode.tracking;
                    missingTime = 0.0f;

                    IsTracking = true;
                    movingSystem.SetPath(new List<Vector3>());
                    Face2Target();

                    PlayerDetected?.Invoke(null, this);
                    Detected?.Invoke(null, false);
                }

                else
                {
                    UpdateMethodInRoaming(dt, SvParams.Get(SvParam.mine_roaming_speed));
                }
            }

            else if (mode == Mode.tracking)
            {
                IsTracking = true;

                if (SearchStrikerInTrackingMode())
                {
                    missingTime = 0.0f;
                    TrackingUpdateMethod(Player.Myself, dt, true, SvParams.Get(SvParam.mine_tracking_speed));

                    Face2Target();
                }

                else
                {
                    missingTime += dt;
                    var trackingDuration = SvParams.Get(SvParam.mine_tracking_duration);

                    if (movingSystem.PathLength() == 0 && missingTime > trackingDuration)
                    {
                        mode = Mode.roaming;
                    }

                    else if (movingSystem.PathLength() == 0)
                    {
                        TrackingUpdateMethod(Player.Myself, dt, true, SvParams.Get(SvParam.mine_tracking_speed));
                    }

                    else
                    {
                        TrackingUpdateMethod(Player.Myself, dt, false, SvParams.Get(SvParam.mine_tracking_speed));
                    }
                }
            }
        }
    }
}
