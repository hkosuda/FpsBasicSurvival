using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class BHop_Command : HostComponent
    {
        static List<Command> commandList = new List<Command>()
        {
            new BackCommand(TxtUtil.L(CommandName.Back)),
            new NextCommand(TxtUtil.L(CommandName.Next)),

            new RecorderCommand(TxtUtil.L(CommandName.Recorder)),
            new ReplayCommand(TxtUtil.L(CommandName.Replay)),
        };

        public override void Initialize()
        {
            foreach(var command in commandList)
            {
                CommandReceiver.AddCommand(command);
            }

            CommandReceiver.RequestCommand("bind z back", null);
            CommandReceiver.RequestCommand("invoke add on_course_out back", null);
        }

        public override void Shutdown()
        {
            foreach(var command in commandList)
            {
                CommandReceiver.SubCommand(command.commandName);
            }
        }
    }
}
