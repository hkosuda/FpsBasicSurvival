using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public enum Difficulty
    {
        eazy = 0, normal = 1, hard = 2, inferno = 3,
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
        static public Difficulty CurrentDifficulty { get; private set; } = Difficulty.eazy;

        static Dictionary<SvParam, float> currentList;
        static bool initialized;

        static void CheckParams()
        {
#if UNITY_EDITOR

            var message = "";

            message = CheckList(message, eazyParamList, "eazy");

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

            if (CurrentDifficulty == Difficulty.eazy)
            {
                currentList = eazyParamList;
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
            if (difficulty == Difficulty.eazy)
            {
                currentList = eazyParamList;
            }
        }

        static Dictionary<SvParam, float> eazyParamList = new Dictionary<SvParam, float>()
        {
            //
            // shop items

            { SvParam.shop_hp_amount, 100 },
            { SvParam.shop_hp_cost_default, 100 },
            { SvParam.shop_hp_cost_increase, 80 },

            { SvParam.shop_armor_amount, 100 },
            { SvParam.shop_armor_cost_default, 100 },
            { SvParam.shop_armor_cost_increase, 120 },

            { SvParam.shop_max_hp_amount, 50 },
            { SvParam.shop_max_hp_cost_default, 800 },
            { SvParam.shop_max_hp_cost_increase, 600 },

            { SvParam.shop_max_armor_amount, 50 },
            { SvParam.shop_max_armor_cost_default, 800 },
            { SvParam.shop_max_armor_cost_increase, 1000 },

            { SvParam.shop_damage_rate_amount, 10 },
            { SvParam.shop_damage_rate_cost_default, 1000 },
            { SvParam.shop_damage_rate_cost_increase, 1500 },

            { SvParam.shop_money_rate_amount, 20 },
            { SvParam.shop_money_rate_cost_default, 1000 },
            { SvParam.shop_money_rate_cost_increase, 1500 },

            { SvParam.shop_mag_extension_amount, 3 },
            { SvParam.shop_mag_extension_cost_default, 800 },
            { SvParam.shop_mag_extension_cost_increase, 1000 },

            { SvParam.shop_bag_extension_amount, 15 },
            { SvParam.shop_bag_extension_cost_default, 1000 },
            { SvParam.shop_bag_extension_cost_increase, 1500 },

            { SvParam.shop_time_remain_amount, 20 },
            { SvParam.shop_time_remain_cost_default, 1000 },
            { SvParam.shop_time_remain_cost_increase, 1000 },

            { SvParam.shop_moving_speed_amount, 3 },
            { SvParam.shop_moving_speed_cost_default, 1000 },
            { SvParam.shop_moving_speed_cost_increase, 1000 },

            { SvParam.shop_weapon_speed_amount, 5 },
            { SvParam.shop_weapon_speed_cost_default, 1000 },
            { SvParam.shop_weapon_speed_cost_increase, 1000 },

            { SvParam.shop_firing_speed_amount, 3 },
            { SvParam.shop_firing_speed_cost_default, 1000 },
            { SvParam.shop_firing_speed_cost_increase, 1500 },

            //
            // field item

            { SvParam.field_hp_amount, 200 },
            { SvParam.field_armor_amount, 100 },
            { SvParam.field_money_amount, 2500 },
            { SvParam.field_compass_money_amount, 1500 },
            { SvParam.field_key_money_amount, 1500 },

            //
            // reward

            { SvParam.mine_destroy_reward, 500 },
            { SvParam.turret_destroy_reward, 600 },

            //
            // after round
            
            { SvParam.money_increase_after_round, 1.0f },
            { SvParam.additional_time_after_round, 180.0f },
            
            //
            // weapon damage
            
            { SvParam.ak_damage, 40.0f },
            { SvParam.de_damage, 140.0f },
            
            //
            // enemy's hp, damage
            
            { SvParam.mine_hp, 100.0f },
            { SvParam.turret_hp, 100.0f },
            { SvParam.mine_hp_increase, 0.5f },
            { SvParam.turret_hp_increase, 0.5f },

            { SvParam.mine_damage, 150.0f },
            { SvParam.turret_damage, 20.0f },
            { SvParam.mine_damage_increase, 0.5f },
            { SvParam.turret_damage_increase, 0.5f },
            
            //
            // enemy's movement
            
            { SvParam.mine_default_roaming_speed, 5.0f },
            { SvParam.mine_roaming_speed_increase, 0.3f },

            { SvParam.mine_default_tracking_speed, 8.2f },
            { SvParam.mine_tracking_speed_increase, 0.4f },

            { SvParam.turret_default_roaming_speed, 4.0f },
            { SvParam.turret_roaming_speed_increase, 0.2f },

            { SvParam.turret_default_tracking_speed, 4.0f },
            { SvParam.turret_tracking_speed_increase, 0.4f },

            { SvParam.mine_tracking_duration, 3.0f },
            { SvParam.turret_tracking_duration, 3.0f },
            
            //
            // turret
            
            { SvParam.turret_shooting_interval, 0.3f },
            { SvParam.turret_shell_exist_time, 2.0f },
            { SvParam.turret_shell_speed, 150.0f },
            { SvParam.turret_shell_h_spread, 0.2f },
            { SvParam.turret_shell_v_spread, 0.1f },

            //
            // spawn

            { SvParam.n_enemies, 25 },
            { SvParam.mine_spawn_rate, 0.7f },
            { SvParam.min_enemies_rate, 0.5f },
            { SvParam.enemy_respawn_probability, 0.5f },
            
            //
            // system
            
            { SvParam.initial_money, 35000 },
            { SvParam.clear_round, 6 },

            //
            // field size

            { SvParam.maze_row, 13 },
            { SvParam.maze_col, 13 },
            
            //
            // key, compass
            
            { SvParam.drop_keys, 6 },
            { SvParam.drop_compass, 3 },
            { SvParam.drop_items, 24 },

            { SvParam.require_keys, 3 },
        };
    }
}

