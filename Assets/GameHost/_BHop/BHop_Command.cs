using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class BHop_Command : HostComponent
    {
        static readonly List<Command> commandList = new List<Command>()
        {
            new BackCommand(TxtUtil.L(CommandName.Back)),
            new NextCommand(TxtUtil.L(CommandName.Next)),

            new RecorderCommand(TxtUtil.L(CommandName.Recorder)),
            new ReplayCommand(TxtUtil.L(CommandName.Replay)),

            new ObserverCommand(TxtUtil.L(CommandName.Observer)),
            new GhostCommand(TxtUtil.L(CommandName.Ghost)),

            new PmMaxSpeedInAirCommand(TxtUtil.L(CommandName.Pm_Max_Speed_In_Air)),
            new DemoCommand(TxtUtil.L(CommandName.Demo)),
        };

        public override void Initialize()
        {
            foreach(var command in commandList)
            {
                CommandReceiver.AddCommand(command);
            }
#if UNITY_EDITOR
            CommandReceiver.RequestCommand("bind z \"back -f\" -e", null);
            CommandReceiver.RequestCommand("toggle e \"recorder start -f\" \"recorder end -f\" -e", null);
#endif
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

