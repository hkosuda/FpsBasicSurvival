using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class TestrunCommand : Command
    {
        static readonly List<string> availables = new List<string>()
        {
            "reset", "add", "rollback", "back",
        };

        static List<List<float[]>> dataListList;

        public TestrunCommand(string commandName) : base(commandName)
        {

        }

        public override List<string> AvailableValues(List<string> values)
        {
            if (values.Count < 3) { return availables; }

            return new List<string>();
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if (values.Count == 1)
            {
                tracer.AddMessage("’l‚ðŽw’è‚µ‚Ä‚­‚¾‚³‚¢", Tracer.Level.error);
            }

            else if (values.Count == 2)
            {
                var value = values[1];

                if (value == "reset")
                {
                    dataListList = new List<List<float[]>>() { new List<float[]>() };
                }

                else if (value == "")
                {

                }
            }
        }

        public override void Update(float dt)
        {
            if (dataListList == null) { dataListList = new List<List<float[]>>() { new List<float[]>() }; }
        }
    }
}
