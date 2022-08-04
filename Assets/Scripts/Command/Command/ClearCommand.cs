using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ClearCommand : Command
    {
        public ClearCommand(string commandName) : base(commandName)
        {
            description = "�R���\�[���̃��O�����ׂč폜���܂��D";
            detail = "";
        }

        public override List<string> AvailableValues(List<string> values)
        {
            return new List<string>();
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if (values.Count == 1)
            {
                ConsoleLogManager.ClearLog();
                tracer.messageList = new List<Tracer.MessageIndent>();
            }

            else
            {
                ERROR_OverValues(tracer);
            }
        }
    }
}

