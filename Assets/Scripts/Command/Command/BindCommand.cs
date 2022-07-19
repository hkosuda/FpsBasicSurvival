using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class BindCommand : Command
    {
        static public EventHandler<bool> BindingUpdated { get; set; }
        static public List<Binding> KeyBindingList { get; private set; }

        public BindCommand(string commandName) : base(commandName)
        {
            commandName = "bind";
            description = "特定のキーにコマンドを割り当てる機能を提供します．";
            detail = "単に'bind'を実行すると，現在の設定を確認することができます．\n" +
                "1番目の値にキーの名称，2番目の値に実行したいコードを二重引用符（\"）で囲んで指定すると，そのキーを押したときに指定したコードが実行されます．" +
                " たとえば，'bind c \"anchor back -m\"'とコンソールで入力すると，Cキーを押した際に'anchor back -m'が実行されるようになります．" +
                "これにより，記録された座標に素早く戻ることができるようになります．\n" +
                "バインドを削除するには，unbindコマンドを使用します．" +
                "unbindでバインドを削除するには，削除したいバインドの番号を把握する必要があります．" +
                "番号を確認するには'bind'を実行しましょう．";

            // initialize method
            KeyBindingList = new List<Binding>();
            TimerSystem.Updated += UpdateMethod;
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

                return available;
            }

            return new List<string>();
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if (values == null || values.Count == 0) { return; }

            if (values.Count == 1)
            {
                tracer.AddMessage(CurrentBindings(), Tracer.Level.normal);
            }

            else if (values.Count == 2)
            {
                tracer.AddMessage("キーのあとに，バインドするコマンドを指定してください．", Tracer.Level.error);
            }

            // ex) bind(0) i(1) /timer/stop/(2)
            else if (values.Count == 3)
            {
                if (KeyBindingList == null) { KeyBindingList = new List<Binding>(); }

                var keyString = values[1];
                var key = Keyconfig.Key.StringToKey(keyString);

                if (key == null)
                {
                    ERROR_InvalidKey(tracer, keyString);
                    tracer.AddMessage(ERROR_InvalidKeyAlert(), Tracer.Level.warning);
                    return;
                }

                var command = CommandReceiver.UnpackGrouping(values[2]);

                if (CheckDuplication(key, command))
                {
                    tracer.AddMessage("※ 重複の判定はオプションを無視して行われます．", Tracer.Level.warning);
                    tracer.AddMessage("重複するコマンドがあるため，処理に失敗しました．", Tracer.Level.error);
                }

                else
                {
                    KeyBindingList.Add(new Binding(key.keyCode, key.wheelDelta, command));
                    tracer.AddMessage("バインドを追加しました：" + BindingInfo(KeyBindingList.Last()), Tracer.Level.normal);
                }
            }

            else
            {
                ERROR_OverValues(tracer);
            }

            BindingUpdated?.Invoke(null, false);

            // - inner function
            static string CurrentBindings()
            {
                if (KeyBindingList == null || KeyBindingList.Count == 0)
                {
                    return "現在バインドされているコマンドはありません．";
                }

                var message = "現在バインドされているコマンドは以下の通りです．\n";

                for (var n = 0; n < KeyBindingList.Count; n++)
                {
                    var binding = KeyBindingList[n];

                    message += "\t\t[" + n.ToString() + "] " + binding.key.GetKeyString() + " : " + binding.command + "\n";
                }

                return message.TrimEnd(new char[1] { '\n' });
            }

            // - inner function
            static bool CheckDuplication(Keyconfig.Key key, string command)
            {
                if (KeyBindingList == null) { KeyBindingList = new List<Binding>(); }

                command = CorrectCommand(command);

                foreach (var keybind in KeyBindingList)
                {
                    if (keybind.key.GetKeyString() == key.GetKeyString())
                    {
                        var _command = CorrectCommand(keybind.command);

                        if (command == _command)
                        {
                            return true;
                        }
                    }
                }

                return false;

                static string CorrectCommand(string command)
                {
                    var values = CommandReceiver.GetValues(command);
                    var c = "";

                    foreach (var v in values)
                    {
                        c += v + " ";
                    }

                    return c.Trim();
                }
            }
        }

        // update method
        static void UpdateMethod(object obj, float dt)
        {
            if (KeyBindingList == null) { return; }
            if (!Input.anyKeyDown && Input.mouseScrollDelta.y == 0.0f) { return; }

            foreach (var keybind in KeyBindingList)
            {
                // key
                if (InputSystem.CheckInput(keybind.key, true))
                {
                    CommandReceiver.RequestCommand(keybind.command, null);
                }
            }
        }

        static public void RemoveKeybind(List<int> indexes, Tracer tracer)
        {
            if (KeyBindingList == null || KeyBindingList.Count == 0)
            {
                tracer.AddMessage("現在バインドは作成されていません．", Tracer.Level.error);
                return;
            }

            var indexLim = KeyBindingList.Count - 1;

            foreach (var index in indexes)
            {
                if (index < 0 || index > indexLim)
                {
                    ERROR_OutOfRange(tracer, index, indexLim);
                }
            }

            if (!tracer.NoError) { return; }

            foreach (var index in indexes)
            {
                var bind = KeyBindingList[index];

                KeyBindingList.RemoveAt(index);
                tracer.AddMessage("バインドを削除しました：" + BindingInfo(bind), Tracer.Level.normal);
            }
        }

        static public void RemoveAll(Tracer tracer)
        {
            KeyBindingList = new List<Binding>();
            tracer.AddMessage("バインドをすべて削除しました．", Tracer.Level.normal);
        }

        static string BindingInfo(BindCommand.Binding binding)
        {
            return "key : " + binding.key.GetKeyString() + ", command : " + binding.command;
        }

        public class Binding
        {
            public Keyconfig.Key key;
            public string command;

            public Binding(KeyCode keyCode, float wheelDelta, string command)
            {
                key = new Keyconfig.Key(keyCode, wheelDelta);
                this.command = command;
            }
        }
    }
}

