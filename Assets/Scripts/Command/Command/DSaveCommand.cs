using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class DSaveCommand : Command
    {
        public DSaveCommand (string commandName) : base(commandName)
        {

        }

        public override List<string> AvailableValues(List<string> values)
        {
            return new List<string>();
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if (values.Count == 1)
            {
                var fileName = "";
                var now = DateTime.Now;

                fileName += MapSystem.CurrentMap.MapName.ToString() + "_";
                fileName += now.ToString("MMdd_HHmm");

                RecordSystem.TrySave(fileName, tracer);
            }

            else
            {
                ERROR_OverValues(tracer);
            }
        }
    }
}

