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

            new ChainCommand(TxtUtil.L(CommandName.Chain)),
            new BhopSettingsCommand("bhop_settings"),
        };

        public override void Initialize()
        {
            foreach(var command in commandList)
            {
                CommandReceiver.AddCommand(command);
            }

#if UNITY_EDITOR
            //CommandReceiver.RequestCommand("bind z \"back 0 -f\"", null);
            //CommandReceiver.RequestCommand("bind mouse0 \"chain save -f\"", null);
            //CommandReceiver.RequestCommand("bind q \"chain back -f\"", null);
            //CommandReceiver.RequestCommand("bind f \"chain start -f\"", null);
            //CommandReceiver.RequestCommand("bind b \"chain rollback -f\"", null);
            CommandReceiver.RequestCommand("bind o \"chain output -f\"", null);
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

