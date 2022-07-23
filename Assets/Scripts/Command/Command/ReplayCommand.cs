using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ReplayCommand : Command
    {
        public ReplayCommand(string commandName) : base(commandName)
        {

        }

        public override List<string> AvailableValues(List<string> values)
        {
            return base.AvailableValues(values);
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if (values.Count == 1)
            {
                if (ReplaySystem.TryBeginReplay(RecordSystem.CachedData, tracer))
                {
                    Console.CloseConsole();
                }
            }

            else
            {
                ERROR_OverValues(tracer);
            }
        }
    }
}

