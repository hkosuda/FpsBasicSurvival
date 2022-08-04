using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class PmMaxSpeedInAirCommand : Command
    {
        static readonly float speed_min = 0.0f;
        static readonly float speed_max = 10.0f;

        public PmMaxSpeedInAirCommand(string commandName) : base(commandName)
        {
            description = "空中での最大速度を設定できます．";
            detail = "この値はストレイフ時の加速に影響します．値が大きいほどストレイフ時の加速が大きくなります．\n" +
                "なお，ここで設定した値はゲームモードを切り替えるたびにデフォルトの値にリセットされます．また，サバイバルモードでは利用できません．\n" +
                "0.0以上10.0以下の値を設定可能です．デフォルト値は" + Params.default_pm_max_speed_in_air.ToString("F1") + "です．";
        }

        public override List<string> AvailableValues(List<string> values)
        {
            return new List<string>();
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if(values.Count == 1)
            {
                tracer.AddMessage("値を指定してください．", Tracer.Level.error);
            }

            else if (values.Count == 2)
            {
                var value = values[1];

                if (float.TryParse(value, out var num))
                {
                    if (speed_min <= num && num <= speed_max)
                    {
                        var before = Params.pm_max_speed_in_air.ToString();
                        var after = num.ToString();

                        tracer.AddMessage("pm_max_speed_in_air : " + before + " -> " + after, Tracer.Level.normal);
                        Params.pm_max_speed_in_air = num;
                        
                    }

                    else
                    {
                        tracer.AddMessage("指定した値が有効な範囲内ではありません．有効な値の範囲は0.0以上10.0以下です．", Tracer.Level.error);
                    }
                }

                else
                {
                    tracer.AddMessage(value + "を有効な数値に変換できません", Tracer.Level.error);
                }
            }

            else
            {
                ERROR_OverValues(tracer);
            }
        }

        public override void Shutdown()
        {
            Params.pm_max_speed_in_air = Params.default_pm_max_speed_in_air;
        }
    }
}

