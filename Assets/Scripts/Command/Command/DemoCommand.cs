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
            description = "�f�����Đ�����@�\��񋟂��܂��D";
            detail = "�f���Ƃ́C���炩���ߗp�ӂ��ꂽ�f�[�^�̂��Ƃł��D��ɂ��̃}�b�v�̍U�����@���m�F����ۂɊ��p���܂��D\n" +
                "'replay'�Ɠ��l�ɁC�f�[�^�̃}�b�v��񂪌��݂̃}�b�v�ƈقȂ�ꍇ�C���s���Ƀ}�b�v���؂�ւ���Ă��܂��̂Œ��ӂ��Ă��������D";

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
                tracer.AddMessage("�Đ�����f�[�^���w�肵�Ă��������D", Tracer.Level.error);
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

