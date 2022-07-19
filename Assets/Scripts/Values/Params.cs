using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    static public class Params
    {
        static public readonly float pm_jumping_velocity = 5.85f;
        static public readonly float pm_crouching_speed = 5.6f;

        static public readonly float volume_shooting = 0.5f;
        static public readonly float volume_footstep = 0.5f;
        static public readonly float volume_landing = 0.5f;

        static public readonly float ak_firing_interval = 0.095f;
        static public readonly float ak_damage = 20.0f;

        static public readonly float de_firing_interval = 0.1f;
        static public readonly float de_damage = 20.0f;

        static public readonly float mine_spawn_rate = 0.7f;

        static public readonly int shop_hp_healing_amount = 100;
        static public readonly int shop_hp_healing_cost_default = 300;
        static public readonly int shop_hp_healing_cost_increase = 150;
        static public readonly int shop_armor_repairing_amount = 100;
        static public readonly int shop_armor_repairing_cost_default = 300;
        static public readonly int shop_armor_repairing_cost_increase = 200;
        static public readonly int shop_hp_upgrade_amount = 50;
        static public readonly int shop_hp_upgrade_cost_default = 1500;
        static public readonly int shop_hp_upgrade_cost_increase = 500;
        static public readonly int shop_armor_upgrade_amount = 50;
        static public readonly int shop_armor_upgrade_cost_default = 1500;
        static public readonly int shop_armor_upgrade_cost_increase = 1000;
        static public readonly int shop_damage_rate_booster_amount = 5;
        static public readonly int shop_damage_rate_booster_cost_default = 2000;
        static public readonly int shop_damage_rate_booster_cost_increase = 1000;
        static public readonly int shop_money_rate_booster_amount = 20;
        static public readonly int shop_money_rate_booster_cost_default = 2000;
        static public readonly int shop_money_rate_booster_cost_increase = 1500;
        static public readonly int shop_mag_extension_amount = 2;
        static public readonly int shop_mag_extension_cost_default = 1000;
        static public readonly int shop_mag_extension_cost_increase = 500;
        static public readonly int shop_bag_extension_amount = 15;
        static public readonly int shop_bag_extension_cost_default = 1000;
        static public readonly int shop_bag_extension_cost_increase = 500;

        static public readonly int mine_destroy_reward = 200;
        static public readonly int turret_destroy_reward = 200;

        static public readonly float money_increase_after_round = 1.0f;

        static public readonly float mine_damage = 20.0f;
        static public readonly float mine_damage_increase = 0.5f;
        static public readonly float turret_damage = 2.0f;
        static public readonly float turret_damage_increase = 0.5f;

        static public readonly float mine_hp = 100.0f;
        static public readonly float mine_hp_increase = 1.0f;
        static public readonly float turret_hp = 100.0f;
        static public readonly float turret_hp_increase = 1.0f;

        static public readonly float mine_tracking_duration = 3.0f;
        static public readonly float mine_detect_range = 50.0f;
        static public readonly float turret_tracking_duration = 5.0f;
        static public readonly float turret_detect_range = 50.0f;

        static public readonly float mine_roaming_speed = 5.0f;
        static public readonly float mine_tracking_speed = 8.0f;
        static public readonly float turret_roaming_speed = 3.0f;
        static public readonly float turret_tracking_speed = 3.0f;

        static public readonly float turret_shell_exist_time = 3.0f;
        static public readonly float turret_shell_speed = 100.0f;
        static public readonly float turret_shooting_interval = 0.3f;

        static public readonly float turret_shell_h_spread = 0.1f;
        static public readonly float turret_shell_v_spread = 0.1f;

        static public readonly float damage_reduction_const = 100.0f;
        static public readonly float sv_armor_reduction_rate = 0.5f;

        static public readonly float enemy_active_range = 300.0f;

        static public readonly int sv_clear_round = 7;

        static public readonly float sv_room_space_ratio = 0.3f;


        static public readonly float sv_min_enemies_rate = 0.3f;
        static public readonly float sv_enemy_respawn_probability = 0.5f;

        

        static public float mouse_sens = 1.0f;
        static public int initial_money = 20000;

        
        // - volume
        static public float volume_turret_engine = 0.5f;
        static public float volume_turret_shot = 0.5f;

        static public float volume_mine_engine = 0.5f;

        static public float volume_detection_alert;
    }
}

