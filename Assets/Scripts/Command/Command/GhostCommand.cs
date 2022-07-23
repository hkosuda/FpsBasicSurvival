using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class GhostCommand : Command
    {
        static public readonly List<string> availableValues = new List<string>()
        {
            TxtUtil.L(Option.Start), TxtUtil.L(Option.End),
        };

        public enum Option
        {
            Start, End,
        }

        public GhostCommand(string commandName) : base(commandName)
        {
            description = "�S�[�X�g���N������@�\��񋟂��܂��D";
            detail = "�P��'ghost'�Ɠ��͂���ƁC���O�ɋL�^�����v���C���[�̓����Ɋւ���f�[�^���Č����܂��D�f�[�^���Ȃ���΍Č��͍s���܂���D" +
                "�f�[�^�̍쐬��'recorder'�R�}���h���g�p���čs���܂��D'ghost' �������Ŏ��s����ꍇ�́C�K�� 'recorder end' �����s���Ă���s���悤�ɂ��܂��傤�D\n" +
                "�ۑ������f�[�^���Ăяo���Ď��s����Ƃ��́C'ghost play <name>' �����s���Ă��������i<name>�͔C�ӂ̃f�[�^���j�D" +
                "�S�[�X�g���I������ɂ́C'ghost end'�����s���܂��D\n" +
                "�S�[�X�g�́C'demo' �� 'replay' �ƈقȂ�C�f�[�^�̃}�b�v��񂪌��݂̃}�b�v�ƈقȂ�ꍇ�͍Đ����s���Ȃ����ߒ��ӂ��Ă��������D";
        }

        public override List<string> AvailableValues(List<string> values)
        {
            if (values == null || values.Count == 0) { return new List<string>(); }

            if (values.Count < 2)
            {
                return availableValues;
            }

            else
            {
                return new List<string>();
            }
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
                    var data = RecordSystem.CachedData;

                    if (data == null)
                    {
                        tracer.AddMessage("�f�[�^�����݂��Ȃ����߁C���p�ł��܂���D", Tracer.Level.error);
                    }

                    else
                    {
                        BeginGhost(data, tracer);
                    }
                }

                else if (value == TxtUtil.L(Option.End))
                {
                    Ghost.EndReplay();
                    tracer.AddMessage("�S�[�X�g���~���܂����D", Tracer.Level.normal);
                }

                else
                {
                    tracer.AddMessage("1�Ԗڂ̒l�Ƃ��ẮC'play'��������'end'�̂ݐݒ�\�ł��D", Tracer.Level.error);
                }
            }

            else
            {
                ERROR_OverValues(tracer);
            }


            // - inner function
            static void BeginGhost(CachedData data, Tracer tracer)
            {
                if (data.mapName != MapSystem.CurrentMap.MapName)
                {
                    tracer.AddMessage("���݂̃}�b�v�ƈقȂ�}�b�v�̃f�[�^�ł��邽�ߎ��s�ł��܂���D", Tracer.Level.error);
                    return;
                }

                Ghost.BeginReplay(data.dataList, data.mapName);
                tracer.AddMessage("�S�[�X�g���N�����܂����D", Tracer.Level.normal);
            }
        }
    }
}

