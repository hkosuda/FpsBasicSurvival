using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class RecorderCommand : Command
    {
        static public readonly float recorderLimitTime = 180.0f;

        public enum Option
        {
            Start, End, Save, Stop,
        }

        static readonly List<string> availables = new List<string>()
        {
            TxtUtil.L(Option.Start), TxtUtil.L(Option.End), TxtUtil.L(Option.Stop), TxtUtil.L(Option.Save)
        };

        public RecorderCommand(string commandName) : base(commandName)
        {
            description = "プレイヤーの動きを記録する機能（レコーダー）を提供します．\n";
            detail = "'recorder start' で記録を開始し，'recorder end' で記録を停止します．記録したデータは，次の記録が終了するまで一時的に保存されます．" +
                "'ghost' や 'replay' の実行時に利用されるデータは，この一時的に保存されたデータです．\n" +
                "'recorder stop' を実行すると，一時的な保存データを書き換えることなくレコーダーを停止できます．" +
                "一時的に保存されている間に'recorder save <name>'を実行すると，ゲームを起動している間だけ名前付きでデータを保持し続けます（<name>の部分に任意の名前を入力します）．" +
                "ここで作成した名前付きデータは，'replay' コマンドや 'ghost' コマンドで利用可能となります．\n" +
                "レコーダーは，" + recorderLimitTime.ToString() + "で指定された時間が経過すると自動で停止します．\n" +
                "保存したデータを削除するには，'recorder remove <name>'を実行してください．また，'remove_last' で最後に保存が行われたデータ名を持つデータを削除することができます．";
        }

        public override List<string> AvailableValues(List<string> values)
        {
            if (values == null || values.Count == 0) { return new List<string>(); }

            if (values.Count < 3)
            {
                return availables;
            }

            return new List<string>();
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
                    RecordSystem.BeginRecorder();

                    var message = "レコーダーを起動しました";
                    tracer.AddMessage(message, Tracer.Level.normal);
                }

                else if (value == TxtUtil.L(Option.End))
                {
                    RecordSystem.FinishRecorder(true);

                    var message = "レコーダーによる記録を終了しました";
                    tracer.AddMessage(message, Tracer.Level.normal);
                }

                else if (value == TxtUtil.L(Option.Stop))
                {
                    RecordSystem.FinishRecorder(false);

                    var message = "レコーダーによる記録を中断しました";
                    tracer.AddMessage(message, Tracer.Level.normal);
                }

                else
                {
                    ERROR_AvailableOnly(tracer, availables);
                }
            }

            else
            {
                ERROR_OverValues(tracer);
            }
        }
    }
}
