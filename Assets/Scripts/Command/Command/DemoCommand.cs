using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class DemoCommand : Command
    {
        static public readonly List<string> availableValues = new List<string>()
        {
           
        };

        public DemoCommand(string commandName) : base(commandName)
        {
            commandName = "demo";
            description = "デモを再生する機能を提供します．";
            detail = "デモとは，あらかじめ用意されたデータのことです．主にそのマップの攻略方法を確認する際に活用します．\n" +
                "'replay'と同様に，データのマップ情報が現在のマップと異なる場合，実行時にマップが切り替わってしまうので注意してください．";

#if UNITY_EDITOR
            foreach (var value in availableValues)
            {
                var asset = Resources.Load<TextAsset>("Record/" + value + ".ghost");
                if (asset == null)
                {
                    Debug.LogError("Not available : " + value);
                }
            }
#endif
        }

        public override List<string> AvailableValues(List<string> values)
        {
            if (values == null || values.Count == 0) { return new List<string>(); }

            if (values.Count < 3)
            {
                return availableValues;
            }

            return new List<string>();
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if (values == null || values.Count == 0) { return; }

            if (values.Count == 1)
            {
                tracer.AddMessage("再生するデータを指定してください．", Tracer.Level.error);
            }

            else if (values.Count == 2)
            {
                var fileName = values[1];

                if (RecordDataIO.TryLoad(fileName, out var demoData, tracer))
                {
                    if (ReplaySystem.TryBeginReplay(demoData, tracer))
                    {
                        Console.CloseConsole();
                    }
                }
            }

            else
            {
                ERROR_OverValues(tracer);
            }
        }
    }
}

