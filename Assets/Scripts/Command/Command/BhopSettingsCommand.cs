using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class BhopSettingsCommand : Command
    {
        public BhopSettingsCommand(string commandName) : base(commandName)
        {
            description = "BHop�p�̂������ߐݒ��K�p���܂��D";
            detail = "�����Őݒ肳���R�}���h�F\n" +
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
                tracer.AddMessage("�R�}���h�̎������s���J�n���܂��D", Tracer.Level.emphasis);

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

