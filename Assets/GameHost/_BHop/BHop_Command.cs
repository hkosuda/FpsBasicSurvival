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
        };

        public override void Initialize()
        {
            foreach(var command in commandList)
            {
                CommandReceiver.AddCommand(command);
            }

            CommandReceiver.RequestCommand("bind z \"back\"", null);
            CommandReceiver.RequestCommand("toggle r \"recorder start\" \"recorder end\"", null);
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

