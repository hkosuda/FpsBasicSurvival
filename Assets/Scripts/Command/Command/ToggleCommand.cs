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
            description = "�ӂ��̃R�}���h���C�g�O���Ŏ��s����@�\��񋟂��܂��D";
            detail = "�g�p���@�Ƃ��ẮC'toggle r \"recorder start\" \"recorder end\"' �̂悤�� 'toggle' �̌�ɃL�[�̖��O�C" +
                "���̂��ƂɃg�O���Ŏ��s����R�[�h���ӂ��w�肵�܂��D\n" +
                "��L�����s���邱�ƂŁCR�L�[���������Ƃ� 'recorder start' �� 'recorder end' �����݂Ɏ��s���邱�Ƃ��ł��܂��D\n" +
                "�g�O���̐ݒ���폜����ɂ́C'toggle remove 0' �̂悤�� 'toggle remove' �̌�ɍ폜�������ݒ�̔ԍ����w�肵�܂��D" +
                "�ԍ�����уg�O���̐ݒ���m�F����ɂ́C'toggle' �����s���Ă��������D";
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
                        tracer.AddMessage("�g�O���̐ݒ�����ׂč폜���܂����D", Tracer.Level.normal);
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
                return "�L�[�̂��ƂɃg�O���Ŏ��s����R�}���h���w�肷�邩�C'remove'�̂��Ƃɍ폜����g�O���ݒ�̃C���f�b�N�X��������'all'���w�肵�Ă��������D";
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
                tracer.AddMessage("���݁C�g�O���ݒ�͑��݂��܂���D", Tracer.Level.normal);
            }

            else
            {
                info = "���݂̃g�O���ݒ�͈ȉ��̒ʂ�ł��D\n" + info;
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
                tracer.AddMessage("�g�O���̐ݒ���폜���܂����D", Tracer.Level.normal);
                tracer.AddMessage("�폜�����g�O���ݒ�F" + group.Info(), Tracer.Level.normal);
            }
        }

        static void TryAddToggleGroup(Keyconfig.Key key, string command1, string command2, Tracer tracer)
        {
            var keyString = key.GetKeyString();

            foreach (var group in ToggleGroupList)
            {
                if (group.key.GetKeyString() == keyString)
                {
                    tracer.AddMessage("�����L�[�ɁC�����̃g�O���ݒ�����蓖�Ă邱�Ƃ͂ł��܂���D", Tracer.Level.error);
                    return;
                }
            }

            var g = new ToggleGroup(key, command1, command2);

            ToggleGroupList.Add(g);
            tracer.AddMessage("�g�O���ݒ���쐬���܂����D", Tracer.Level.normal);
            tracer.AddMessage("�쐬���ꂽ�g�O���ݒ�F" + g.Info(), Tracer.Level.normal);
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

