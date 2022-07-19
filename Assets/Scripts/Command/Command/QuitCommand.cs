using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class QuitCommand : Command
    {
        public QuitCommand(string commandName) : base(commandName)
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
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            }

            else
            {
                ERROR_OverValues(tracer);
            }
        }
    }
}

