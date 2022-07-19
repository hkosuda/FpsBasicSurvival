using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class NextCommand : Command
    {
        public NextCommand(string commandName) : base(commandName)
        {
            commandName = "next";
            description = "中間地点が複数設定されているマップで，次のチェックポイントに移動する機能を提供します．";
            detail = "チェックポイントがひとつしかないマップでは，つねに同じ場所に移動します．";
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if (values == null || values.Count == 0) { return; }

            else if (values.Count == 1)
            {
                var prev = MapSystem.CurrentMap.Index;
                MapSystem.CurrentMap.Next();
                var current = MapSystem.CurrentMap.Index;

                if (MapSystem.CurrentMap.respawnPositions.Length == 1)
                {
                    tracer.AddMessage("現在のマップにはチェックポイントが1つしかありません．", Tracer.Level.warning);
                }

                else
                {
                    tracer.AddMessage("check point : " + prev.ToString() + " -> " + current.ToString(), Tracer.Level.normal);
                }
            }

            else
            {
                ERROR_OverValues(tracer);
            }
        }
    }
}

