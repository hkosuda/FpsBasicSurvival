using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class BeginCommand : Command
    {
        static readonly List<string> availables = new List<string>()
        {
            TxtUtil.L(HostName.survival), TxtUtil.L(HostName.ez_tower), TxtUtil.L(HostName.ez_stream), TxtUtil.L(HostName.ez_square),
        };

        public BeginCommand(string commandName) : base(commandName)
        {

        }

        public override List<string> AvailableValues(List<string> values)
        {
            if (values.Count < 3)
            {
                return availables;
            }

            return new List<string>();
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if (values.Count == 1)
            {
                ERROR_NeedValue(tracer);
            }

            else if (values.Count == 2)
            {
                var value = values[1];

                if (TryGetHostName(value, out var hostName))
                {
                    var message = hostName + "‚ðŠJŽn‚µ‚Ü‚·";
                    tracer.AddMessage(message, Tracer.Level.normal);

                    GameSystem.SwitchHost(hostName);
                }

                else
                {
                    ERROR_AvailableOnly(tracer, availables);
                }
            }

            else
            {
                ERROR_OverValues(tracer);
            }
            
            // - inner function
            static bool TryGetHostName(string value, out HostName hostName)
            {
                value = value.ToLower().ToLower();

                foreach(HostName host in Enum.GetValues(typeof(HostName)))
                {
                    if(value == host.ToString().ToLower())
                    {
                        hostName = host;
                        return true;
                    }
                }

                hostName = HostName.survival;
                return false;
            }
        }
    }
}

