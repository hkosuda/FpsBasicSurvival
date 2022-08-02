using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class HistoryCommand : Command
    {
        public HistoryCommand(string commandName) : base(commandName)
        {
            description = "�o�[�W�����̍X�V�������m�F���邱�Ƃ��ł��܂��D";
            detail = "";
        }

        public override List<string> AvailableValues(List<string> values)
        {
            return new List<string>();
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if (values.Count == 1)
            {
                AddHistoryInfo(tracer);
            }

            else
            {
                ERROR_OverValues(tracer);
            }
        }

        static void AddHistoryInfo(Tracer tracer)
        {
            tracer.AddMessage("ver 01.00", Tracer.Level.emphasis);
            tracer.AddMessage(v0100, Tracer.Level.normal, 2);

            tracer.AddMessage("ver 01.01", Tracer.Level.emphasis);
            tracer.AddMessage(v0101, Tracer.Level.normal, 2);

            tracer.AddMessage("ver 01.02", Tracer.Level.emphasis);
            tracer.AddMessage(v0102, Tracer.Level.normal, 2);

            tracer.AddMessage("ver 01.03 (latest)", Tracer.Level.emphasis);
            tracer.AddMessage(v0103, Tracer.Level.normal, 2);
        }

        static readonly string v0100 = "'FPS_Basic �T�o�C�o��'�������[�X";
        static readonly string v0101 = "" +
            "�EHistory�R�}���h��ǉ��D\n" +
            "�EClear�R�}���h��ǉ�\n" +
            "�EHP�������̓A�[�}�[�̏���������グ��A�b�v�O���[�g�𕡐��I�����������ŁCHP�������̓A�[�}�[�̉񕜂𕡐��I�������̂��C��q�̏���������グ��A�b�v�O���[�g�̌������炵���Ƃ��C�񕜂̌����]���ƂȂ��Ă��X�V����Ȃ��o�O���C���D\n" +
            "�E�A�b�v�O���[�g�w���E�B���h�E�̃��C�A�E�g��ύX�D\n" +
            "�E�A�b�v�O���[�h���w������ƁC�Y������A�b�v�O���[�g�̃e�L�X�g�̐F���ς��悤�ɕύX�D\n" +
            "�E���_�ړ��̃��J�j�Y�����C���D\n" +
            "�E�w���v��\�����C�L�[�o�C���h���m�F�ł���悤�ɁD\n" +
            "�E�T�o�C�o�����[�h�́C�ŏ��̃��E���h�̃f�U�C�����ꕔ�ύX�D\n" +
            "�E�R���\�[���̐������̃t�H���g����菬�����T�C�Y�ɕύX�D\n" +
            "�E�T�o�C�o�����[�h�̕ǂ̐F��ύX�D";
        static readonly string v0102 = "" +
            "�E�T�o�C�o�����[�h�̍ŏ��̃��E���h�̃f�U�C����啝�ɕύX���Csurf�}�b�v��bhop�}�b�v�ɃA�N�Z�X���₷�����܂����D\n" +
            "�E�u�V�ѕ��v�ɂ��Ă̐������C�����܂����D";
        static readonly string v0103 = "" +
            "�E�O�i�C��ށC�E�Ɉړ��C���Ɉړ��̃L�[��ύX�ł��Ȃ������C���D\n" +
            "�E�D";

    }
}

