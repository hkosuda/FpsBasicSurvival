using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class RecorderCommand : Command
    {
        static public readonly float recorderLimitTime = 180.0f;

        public enum Option
        {
            Start, End, Stop,
        }

        static readonly List<string> availables = new List<string>()
        {
            TxtUtil.L(Option.Start), TxtUtil.L(Option.End), TxtUtil.L(Option.Stop),
        };

        public RecorderCommand(string commandName) : base(commandName)
        {
            description = "�v���C���[�̓������L�^����@�\�i���R�[�_�[�j��񋟂��܂��D\n";
            detail = "'recorder start' �ŋL�^���J�n���C'recorder end' �ŋL�^���~���܂��D�L�^�����f�[�^�́C���̋L�^���I������܂ňꎞ�I�ɕۑ�����܂��D" +
                "'ghost' �� 'replay' �̎��s���ɗ��p�����f�[�^�́C���̈ꎞ�I�ɕۑ����ꂽ�f�[�^�ł��D\n" +
                "'recorder stop' �����s����ƁC�ꎞ�I�ȕۑ��f�[�^�����������邱�ƂȂ����R�[�_�[���~�ł��܂��D" +
                "���R�[�_�[�́C" + recorderLimitTime.ToString() + "�Ŏw�肳�ꂽ���Ԃ��o�߂���Ǝ����Œ�~���܂��D";
        }

        public override List<string> AvailableValues(List<string> values)
        {
            if (values == null || values.Count == 0) { return new List<string>(); }

            if (values.Count < 3)
            {
                return availables;
            }

            return new List<string>();
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if (values == null || values.Count == 0) { return; }

            if (values.Count == 1)
            {
                ERROR_NeedValue(tracer);
            }

            else if (values.Count == 2)
            {
                var value = values[1];

                if (value == TxtUtil.L(Option.Start))
                {
                    RecordSystem.BeginRecorder();

                    var message = "���R�[�_�[���N�����܂���";
                    tracer.AddMessage(message, Tracer.Level.normal);
                }

                else if (value == TxtUtil.L(Option.End))
                {
                    RecordSystem.FinishRecorder(true);

                    var message = "���R�[�_�[�ɂ��L�^���I�����܂���";
                    tracer.AddMessage(message, Tracer.Level.normal);
                }

                else if (value == TxtUtil.L(Option.Stop))
                {
                    RecordSystem.FinishRecorder(false);

                    var message = "���R�[�_�[�ɂ��L�^�𒆒f���܂���";
                    tracer.AddMessage(message, Tracer.Level.normal);
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
        }
    }
}
