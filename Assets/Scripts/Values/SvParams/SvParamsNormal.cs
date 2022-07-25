using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SvParamsNormal : MonoBehaviour
    {
        static public readonly Dictionary<SvParam, float> normalParamList = new Dictionary<SvParam, float>()
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
            { SvParam.shop_mag_extension_cost_increase, 800 },

            { SvParam.shop_bag_extension_amount, 15 },
            { SvParam.shop_bag_extension_cost_default, 1000 },
            { SvParam.shop_bag_extension_cost_increase, 1200 },

            { SvParam.shop_time_remain_amount, 25 },
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
            { SvParam.shop_firing_speed_cost_increase, 1200 },

            //
            // field item

            { SvParam.field_hp_amount, 200 },
            { SvParam.field_armor_amount, 150 },
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
            { SvParam.de_damage, 180.0f },
            
            //
            // enemy's hp, damage
            
            { SvParam.mine_hp, 100.0f },
            { SvParam.turret_hp, 100.0f },
            { SvParam.mine_hp_increase, 0.4f },
            { SvParam.turret_hp_increase, 0.4f },

            { SvParam.mine_damage, 130.0f },
            { SvParam.turret_damage, 18.0f },
            { SvParam.mine_damage_increase, 0.4f },
            { SvParam.turret_damage_increase, 0.4f },
            
            //
            // enemy's movement
            
            { SvParam.mine_default_roaming_speed, 5.0f },
            { SvParam.mine_roaming_speed_increase, 0.2f },

            { SvParam.mine_default_tracking_speed, 8.0f },
            { SvParam.mine_tracking_speed_increase, 0.2f },

            { SvParam.turret_default_roaming_speed, 4.0f },
            { SvParam.turret_roaming_speed_increase, 0.2f },

            { SvParam.turret_default_tracking_speed, 4.0f },
            { SvParam.turret_tracking_speed_increase, 0.2f },

            { SvParam.mine_tracking_duration, 3.0f },
            { SvParam.turret_tracking_duration, 3.0f },
            
            //
            // turret
            
            { SvParam.turret_shooting_interval, 0.3f },
            { SvParam.turret_shell_exist_time, 2.0f },
            { SvParam.turret_shell_speed, 150.0f },
            { SvParam.turret_shell_h_spread, 0.25f },
            { SvParam.turret_shell_v_spread, 0.1f },

            //
            // spawn

            { SvParam.n_enemies, 16 },
            { SvParam.mine_spawn_rate, 0.7f },
            { SvParam.min_enemies_rate, 0.4f },
            { SvParam.enemy_respawn_probability, 0.5f },
            
            //
            // system
            
            { SvParam.initial_money, 35000 },
            { SvParam.clear_round, 5 },

            //
            // field size

            { SvParam.maze_row, 12 },
            { SvParam.maze_col, 12 },
            
            //
            // key, compass
            
            { SvParam.drop_keys, 6 },
            { SvParam.drop_compass, 3 },
            { SvParam.drop_items, 20 },

            { SvParam.require_keys, 3 },
        };
    }
}

