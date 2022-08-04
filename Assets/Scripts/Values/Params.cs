using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    static public class Params
    {
        static public readonly float default_pm_max_speed_in_air = 1.4f;

        //
        // player movement
        static public float pm_max_speed_on_ground = 7.7f;
        static public float pm_max_speed_in_air = default_pm_max_speed_in_air;
        static public float pm_accel_on_ground = 65.0f;
        static public float pm_accel_in_air = 5000.0f;
        static public float pm_dragging_accel = 45.0f;

        static public float pm_max_speed_in_crouching = 3.5f;
        static public float pm_accel_in_crouching = 95.0f;
        static public float pm_dragging_accel_in_crouching = 90.0f;

        static public float pm_jumping_velocity = 5.85f;
        static public float pm_crouching_speed = 6.0f;

        //
        // sensitivity

        static public float mouse_sens = 1.0f;

        // volume

        static public float volume_shooting = 0.5f;
        static public float volume_footstep = 0.5f;
        static public float volume_landing = 0.5f;

        static public float volume_turret_engine = 0.5f;
        static public float volume_turret_shot = 0.5f;
        static public float volume_mine_engine = 0.5f;
        static public float volume_detection_alert = 0.5f;
    }
}

