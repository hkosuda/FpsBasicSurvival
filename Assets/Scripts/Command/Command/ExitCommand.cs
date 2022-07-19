using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ExitCommand : Command
    {
        public ExitCommand(string name) : base(name)
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
                Console.CloseConsole();
            }

            else
            {
                ERROR_OverValues(tracer);
            }
        }
    }
}

