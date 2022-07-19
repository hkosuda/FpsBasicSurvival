using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ToggleCommand : Command
    {
        static public EventHandler<bool> ToggleUpdated { get; set; }
        static public List<ToggleGroup> ToggleGroupList = new List<ToggleGroup>();

        public ToggleCommand(string commandName) : base(commandName)
        {
            description = "ふたつのコマンドを，トグルで実行する機能を提供します．";
            detail = "使用方法としては，'toggle r \"recorder start\" \"recorder end\"' のように 'toggle' の後にキーの名前，" +
                "そのあとにトグルで実行するコードをふたつ指定します．\n" +
                "上記を実行することで，Rキーを押すことで 'recorder start' と 'recorder end' を交互に実行することができます．\n" +
                "トグルの設定を削除するには，'toggle remove 0' のように 'toggle remove' の後に削除したい設定の番号を指定します．" +
                "番号およびトグルの設定を確認するには，'toggle' を実行してください．";
        }

        public override void Update(float dt)
        {
            if (ToggleGroupList == null) { return; }
            if (!Input.anyKeyDown && Input.mouseScrollDelta.y == 0.0f) { return; }

            foreach (var group in ToggleGroupList)
            {
                // key
                if (InputSystem.CheckInput(group.key, true))
                {
                    group.Exec();
                }
            }
        }

        public override List<string> AvailableValues(List<string> values)
        {
            if (values == null || values.Count == 0) { return new List<string>(); }

            if (values.Count < 3)
            {
                var available = new List<string>();

                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (keyCode == KeyCode.None) { continue; }
                    available.Add(keyCode.ToString().ToLower());
                }

                available.Add("1");
                available.Add("-1");
                available.Add("remove");

                return available;
            }

            else if (values.Count < 4)
            {
                if (values[1] == "remove")
                {
                    var list = new List<string>();

                    for (var n = 0; n < ToggleGroupList.Count - 1; n++)
                    {
                        list.Add(n.ToString());
                    }

                    list.Add("all");
                    return list;
                }

                else
                {
                    return new List<string>();
                }
            }

            else
            {
                return new List<string>();
            }
        }

        // ex) toggle(0) t(1) /observer/start/(2) /observer/end/(3)
        // ex) toggle(0) remove(1) 3(2)
        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if (values == null || values.Count == 0) { return; }

            if (values.Count == 1)
            {
                CurrentBindingMessage(tracer);
            }

            else if (values.Count == 2)
            {
                tracer.AddMessage(ERROR_InvalidValues(), Tracer.Level.error);
            }

            else if (values.Count == 3)
            {
                var action = values[1];

                if (action == "remove")
                {
                    var indexString = values[2];

                    if (indexString == "all")
                    {
                        ToggleGroupList = new List<ToggleGroup>();
                        tracer.AddMessage("トグルの設定をすべて削除しました．", Tracer.Level.normal);
                    }

                    else if (int.TryParse(indexString, out var index))
                    {
                        TryRemove(index, tracer);
                    }

                    else
                    {
                        ERROR_NotInteger(tracer, indexString);
                    }
                }

                else
                {
                    tracer.AddMessage(ERROR_InvalidValues(), Tracer.Level.error);
                }
            }

            else if (values.Count == 4)
            {
                var keyString = values[1];
                var key = Keyconfig.Key.StringToKey(keyString);

                if (key == null)
                {
                    ERROR_InvalidKey(tracer, keyString);
                    tracer.AddMessage(ERROR_InvalidKeyAlert(), Tracer.Level.warning);
                    return;
                }

                var command1 = CommandReceiver.UnpackGrouping(values[2]);
                var command2 = CommandReceiver.UnpackGrouping(values[3]);

                TryAddToggleGroup(key, command1, command2, tracer);
            }

            else
            {
                ERROR_OverValues(tracer);
            }

            ToggleUpdated?.Invoke(null, false);

            // - inner function
            static string ERROR_InvalidValues()
            {
                return "キーのあとにトグルで実行するコマンドを二つ指定するか，'remove'のあとに削除するトグル設定のインデックスもしくは'all'を指定してください．";
            }
        }

        static void CurrentBindingMessage(Tracer tracer)
        {
            var info = "";

            foreach (var group in ToggleGroupList)
            {
                info += "\t" + group.key.GetKeyString() + "\t| " + group.command1 + "\t| " + group.command2 + "\n";
            }

            if (info == "")
            {
                tracer.AddMessage("現在，トグル設定は存在しません．", Tracer.Level.normal);
            }

            else
            {
                info = "現在のトグル設定は以下の通りです．\n" + info;
                tracer.AddMessage(info, Tracer.Level.normal);
            }
        }

        static void TryRemove(int index, Tracer tracer)
        {
            var indexLim = ToggleGroupList.Count - 1;

            if (index < 0 || index > indexLim)
            {
                ERROR_OutOfRange(tracer, index, indexLim);
            }

            else
            {
                var group = ToggleGroupList[index];

                ToggleGroupList.RemoveAt(index);
                tracer.AddMessage("トグルの設定を削除しました．", Tracer.Level.normal);
                tracer.AddMessage("削除したトグル設定：" + group.Info(), Tracer.Level.normal);
            }
        }

        static void TryAddToggleGroup(Keyconfig.Key key, string command1, string command2, Tracer tracer)
        {
            var keyString = key.GetKeyString();

            foreach (var group in ToggleGroupList)
            {
                if (group.key.GetKeyString() == keyString)
                {
                    tracer.AddMessage("同じキーに，複数のトグル設定を割り当てることはできません．", Tracer.Level.error);
                    return;
                }
            }

            var g = new ToggleGroup(key, command1, command2);

            ToggleGroupList.Add(g);
            tracer.AddMessage("トグル設定を作成しました．", Tracer.Level.normal);
            tracer.AddMessage("作成されたトグル設定：" + g.Info(), Tracer.Level.normal);
        }

        public class ToggleGroup
        {
            public Keyconfig.Key key;

            public string command1;
            public string command2;

            bool toggleSwitch;

            public ToggleGroup(Keyconfig.Key key, string command1, string command2)
            {
                this.key = key;
                this.command1 = command1;
                this.command2 = command2;

                toggleSwitch = true;
            }

            public void Exec()
            {
                if (toggleSwitch)
                {
                    CommandReceiver.RequestCommand(command1, null);
                }

                else
                {
                    CommandReceiver.RequestCommand(command2, null);
                }

                toggleSwitch = !toggleSwitch;
            }

            public string Info()
            {
                return key.GetKeyString() + "\t| " + command1 + "\t| " + command2;
            }
        }
    }
}

