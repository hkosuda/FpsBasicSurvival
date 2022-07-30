using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public enum Difficulty
    {
        ez = 0, normal = 1, hard = 2, inferno = 3,
    }

    public enum SvParam
    {
        //
        // shop items

        shop_hp_amount,
        shop_hp_cost_default,
        shop_hp_cost_increase,

        shop_armor_amount,
        shop_armor_cost_default,
        shop_armor_cost_increase,

        shop_max_hp_amount,
        shop_max_hp_cost_default,
        shop_max_hp_cost_increase,

        shop_max_armor_amount,
        shop_max_armor_cost_default,
        shop_max_armor_cost_increase,

        shop_damage_rate_amount,
        shop_damage_rate_cost_default,
        shop_damage_rate_cost_increase,

        shop_money_rate_amount,
        shop_money_rate_cost_default,
        shop_money_rate_cost_increase,

        shop_mag_extension_amount,
        shop_mag_extension_cost_default,
        shop_mag_extension_cost_increase,

        shop_bag_extension_amount,
        shop_bag_extension_cost_default,
        shop_bag_extension_cost_increase,

        shop_time_remain_amount,
        shop_time_remain_cost_default,
        shop_time_remain_cost_increase,

        shop_moving_speed_amount,
        shop_moving_speed_cost_default,
        shop_moving_speed_cost_increase,

        shop_weapon_speed_amount,
        shop_weapon_speed_cost_default,
        shop_weapon_speed_cost_increase,

        shop_firing_speed_amount,
        shop_firing_speed_cost_default,
        shop_firing_speed_cost_increase,

        //
        // field item

        field_hp_amount,
        field_armor_amount,
        field_money_amount,
        field_compass_money_amount,
        field_key_money_amount,

        //
        // reward

        mine_destroy_reward,
        turret_destroy_reward,

        //
        // after round

        money_increase_after_round,
        additional_time_after_round,

        //
        // weapon damage

        ak_damage,
        de_damage,

        //
        // enemy's hp, damage

        mine_hp,
        mine_hp_increase,
        turret_hp,
        turret_hp_increase,

        mine_damage,
        turret_damage,
        mine_damage_increase,
        turret_damage_increase,

        //
        // enemy's movement

        mine_default_roaming_speed,
        mine_roaming_speed_increase,

        mine_default_tracking_speed,
        mine_tracking_speed_increase,

        turret_default_roaming_speed,
        turret_roaming_speed_increase,

        turret_default_tracking_speed,
        turret_tracking_speed_increase,

        mine_tracking_duration,
        turret_tracking_duration,

        //
        // turret

        turret_shooting_interval,
        turret_shell_exist_time,
        turret_shell_speed,
        turret_shell_h_spread,
        turret_shell_v_spread,

        //
        // spawn

        n_enemies,
        mine_spawn_rate,
        min_enemies_rate,
        enemy_respawn_probability,

        //
        // system

        initial_money,
        clear_round,

        //
        // field size

        maze_row,
        maze_col,

        //
        // key, compass

        drop_keys,
        drop_compass,
        drop_items,

        require_keys,
    }

    static public class SvParams
    {
        static public Difficulty CurrentDifficulty { get; private set; } = Difficulty.ez;

        static Dictionary<SvParam, float> currentList;
        static bool initialized;

        static void CheckParams()
        {
#if UNITY_EDITOR

            var message = "";

            message = CheckList(message, SvParamsEazy.eazyParamList, "eazy");
            message = CheckList(message, SvParamsNormal.normalParamList, "normal");
            message = CheckList(message, SvParamsHard.hardParamList, "hard");
            message = CheckList(message, SvParamsInferno.infernoParamList, "inferno");

            if (message != "")
            {
                Debug.LogError(message);
            }

            // - inner function
            static string CheckList(string message, Dictionary< SvParam, float> list, string listName)
            {
                foreach (SvParam sv in Enum.GetValues(typeof(SvParam)))
                {
                    if (!list.ContainsKey(sv))
                    {
                        message += "'" + listName + "' ‚Í '" + sv.ToString() + "' ‚ð’è‹`‚µ‚Ä‚¢‚Ü‚¹‚ñ\n";
                    }
                }

                return message;
            }
#endif
        }

        static public void Initialize()
        {
            CheckParams();

            if (CurrentDifficulty == Difficulty.ez)
            {
                currentList = SvParamsEazy.eazyParamList;
            }
        }

        static public float Get(SvParam sv, bool asInteger = false)
        {
            if (!initialized) { initialized = true; Initialize(); }
            return currentList[sv];
        }

        static public int GetInt(SvParam sv)
        {
            if (!initialized) { initialized = true; Initialize(); }
            return (int)currentList[sv];
        }

        static public void SwitchDifficulty(Difficulty difficulty)
        {
            if (difficulty == Difficulty.ez)
            {
                CurrentDifficulty = Difficulty.ez;
                currentList = SvParamsEazy.eazyParamList;
            }

            if (difficulty == Difficulty.normal)
            {
                CurrentDifficulty = Difficulty.normal;
                currentList = SvParamsNormal.normalParamList;
            }

            if (difficulty == Difficulty.hard)
            {
                CurrentDifficulty = Difficulty.hard;
                currentList = SvParamsHard.hardParamList;
            }

            if (difficulty == Difficulty.inferno)
            {
                CurrentDifficulty = Difficulty.inferno;
                currentList = SvParamsInferno.infernoParamList;
            }
        }
    }
}

