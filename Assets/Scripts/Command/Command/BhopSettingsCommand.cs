using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class BhopSettingsCommand : Command
    {
        public BhopSettingsCommand(string commandName) : base(commandName)
        {
            description = "BHop用のおすすめ設定を適用します．";
            detail = "自動で設定されるコマンド：\n" +
                "bind z \"back 0 -f\"" +
                "bind mouse0 \"chain save -f\"" +
                "bind q \"chain back -f\"" +
                "bind f \"chain start -f\"" +
                "bind b \"chain rollback -f\"" +
                "";
        }

        static readonly List<string> commandList = new List<string>()
        {
            "bind z \"back 0 -f\"",
            "bind mouse0 \"chain save -f\"",
            "bind q \"chain back -f\"",
            "bind f \"chain start -f\"",
            "bind b \"chain rollback -f\"",
        };

        public override List<string> AvailableValues(List<string> values)
        {
            return new List<string>();
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if (values.Count == 1)
            {
                tracer.AddMessage("コマンドの自動実行を開始します．", Tracer.Level.emphasis);

                foreach(var command in commandList)
                {
                    CommandReceiver.RequestCommand(command, tracer);
                }
            }

            else
            {
                ERROR_OverValues(tracer);
            }
        }
    }
}

