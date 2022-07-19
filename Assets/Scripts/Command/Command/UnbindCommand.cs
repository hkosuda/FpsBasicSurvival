using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class UnbindCommand : Command
    {
        public UnbindCommand(string commandName) : base(commandName)
        {
            commandName = "unbind";
            description = "作成したバインドを削除する機能を提供します．";
            detail = "バインドを指定するには，'unbind 0' のように 'bind' のあとに番号を指定します．" +
                "そのため，事前にコンソールで'bind'と入力し削除したいバインドが何番に指定されているかを確認しましょう．";
        }

        public override List<string> AvailableValues(List<string> values)
        {
            if (values == null || values.Count == 0) { return new List<string>(); }

            if (values.Count < 3)
            {
                var available = new List<string>();

                for (var n = 0; n < BindCommand.KeyBindingList.Count - 1; n++)
                {
                    available.Add(n.ToString());
                }

                available.Add("all");

                return available;
            }

            return new List<string>();
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if (values == null || values.Count == 0) { return; }

            if (values.Count == 1)
            {
                tracer.AddMessage("削除するバインドのインデックスを指定してください", Tracer.Level.error);
            }

            // unbind(0) 1(1) 4(2) 5(3) ... / unbinid(0) all(1)
            else
            {
                var value = values[1];

                if (values.Count == 2 && value == "all")
                {
                    BindCommand.RemoveAll(tracer);
                    return;
                }

                var indexes = GetIndexes(values, 1, tracer);
                if (indexes == null) { return; }

                BindCommand.RemoveKeybind(indexes, tracer);
            }
        }


        static public List<int> GetIndexes(List<string> values, int startIndex, Tracer tracer)
        {
            var indexLim = values.Count - 1;
            if (startIndex < 0 || startIndex > indexLim) { return null; }

            var list = new List<int>();

            for (var n = startIndex; n < values.Count; n++)
            {
                var value = values[n];

                if (int.TryParse(value, out var index))
                {
                    if (list.Contains(index))
                    {
                        tracer.AddMessage("同じインデックス（" + index.ToString() + "）が含まれています．", Tracer.Level.error);
                    }

                    else
                    {
                        list.Add(index);
                    }
                }

                else
                {
                    ERROR_NotInteger(tracer, value);
                }
            }

            if (!tracer.NoError) { return null; }

            list.Sort();
            list.Reverse();

            return list;
        }
    }
}

