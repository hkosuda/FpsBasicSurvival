using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class BackCommand : Command
    {
        public BackCommand(string commandName) : base(commandName)
        {
            description = "最後に到達したチェックポイントまで戻ります．値を指定すると，その値に対応するチェックポイントまで戻ります．";
            detail = "単に'back'として実行すると，最後に到達したチェックポイントまで戻ります．\n" +
                "'back 1'など，値を指定して実行するとその値に対応するチェックポイントまで戻ります．チェックポイントの番号は，" +
                "スタート位置となるチェックポイントが0，その次が1...といったように，0から始まるので注意してください．\n" +
                "主にon_course_outが発生したときに自動実行させるか，手動で特定のチェックポイントに移動したいときに使用します．";
        }

        public override List<string> AvailableValues(List<string> values)
        {
            if (values == null || values.Count == 0) { return new List<string>(); }

            else if (values.Count < 3)
            {
                var list = new List<string>();

                for (var n = 0; n < MapSystem.CurrentMap.respawnPositions.Length; n++)
                {
                    list.Add(n.ToString());
                }

                return list;
            }

            else
            {
                return new List<string>();
            }
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if (values == null || values.Count == 0) { return; }

            else if (values.Count == 1)
            {
                MapSystem.CurrentMap.Back();
                tracer.AddMessage("check point : " + MapSystem.CurrentMap.Index.ToString(), Tracer.Level.normal);
            }

            // ex) back(0) 0(1)
            else if (values.Count == 2)
            {
                var indexString = values[1];

                if (int.TryParse(indexString, out var index))
                {
                    MapSystem.CurrentMap.Back(index);
                    tracer.AddMessage("check point : " + MapSystem.CurrentMap.Index.ToString(), Tracer.Level.normal);
                }

                else
                {
                    ERROR_NotInteger(tracer, indexString);
                }
            }

            else
            {
                ERROR_OverValues(tracer);
            }
        }
    }
}

