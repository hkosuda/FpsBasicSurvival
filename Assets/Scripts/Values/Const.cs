using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Const : MonoBehaviour
    {
        //
        // layer

        static public readonly int bounceLayer = 6;
        static public readonly int enemyLayer = 7;
        static public readonly int itemLayer = 8;
        static public readonly int playerLayer = 9;

        //
        // params

        static public readonly float ak_firing_interval = 0.095f;
        static public readonly float de_firing_interval = 0.1f;

        static public readonly float enemy_detect_range = 50.0f;
        static public readonly float enemy_active_range = 300.0f;

        static public readonly float damage_reduction_const = 1000.0f;
        static public readonly float armor_reduction_rate = 0.5f;

        static public readonly float room_space_ratio = 0.3f;
    }
}

