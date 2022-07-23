using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class GhostCommand : Command
    {
        static public readonly List<string> availableValues = new List<string>()
        {
            TxtUtil.L(Option.Start), TxtUtil.L(Option.End),
        };

        public enum Option
        {
            Start, End,
        }

        public GhostCommand(string commandName) : base(commandName)
        {
            description = "ゴーストを起動する機能を提供します．";
            detail = "単に'ghost'と入力すると，直前に記録したプレイヤーの動きに関するデータを再現します．データがなければ再現は行われません．" +
                "データの作成は'recorder'コマンドを使用して行います．'ghost' を自動で実行する場合は，必ず 'recorder end' を実行してから行うようにしましょう．\n" +
                "保存したデータを呼び出して実行するときは，'ghost play <name>' を実行してください（<name>は任意のデータ名）．" +
                "ゴーストを終了するには，'ghost end'を実行します．\n" +
                "ゴーストは，'demo' や 'replay' と異なり，データのマップ情報が現在のマップと異なる場合は再生が行われないため注意してください．";
        }

        public override List<string> AvailableValues(List<string> values)
        {
            if (values == null || values.Count == 0) { return new List<string>(); }

            if (values.Count < 2)
            {
                return availableValues;
            }

            else
            {
                return new List<string>();
            }
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if (values == null || values.Count == 0) { return; }

            if (values.Count == 1)
            {
                ERROR_NeedValue(tracer);
            }

            else if (values.Count == 2)
            {
                var value = values[1];

                if (value == TxtUtil.L(Option.Start))
                {
                    var data = RecordSystem.CachedData;

                    if (data == null)
                    {
                        tracer.AddMessage("データが存在しないため，利用できません．", Tracer.Level.error);
                    }

                    else
                    {
                        BeginGhost(data, tracer);
                    }
                }

                else if (value == TxtUtil.L(Option.End))
                {
                    Ghost.EndReplay();
                    tracer.AddMessage("ゴーストを停止しました．", Tracer.Level.normal);
                }

                else
                {
                    tracer.AddMessage("1番目の値としては，'play'もしくは'end'のみ設定可能です．", Tracer.Level.error);
                }
            }

            else
            {
                ERROR_OverValues(tracer);
            }


            // - inner function
            static void BeginGhost(CachedData data, Tracer tracer)
            {
                if (data.mapName != MapSystem.CurrentMap.MapName)
                {
                    tracer.AddMessage("現在のマップと異なるマップのデータであるため実行できません．", Tracer.Level.error);
                    return;
                }

                Ghost.BeginReplay(data.dataList, data.mapName);
                tracer.AddMessage("ゴーストを起動しました．", Tracer.Level.normal);
            }
        }
    }
}

