using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class NextCommand : Command
    {
        public NextCommand(string commandName) : base(commandName)
        {
            commandName = "next";
            description = "���Ԓn�_�������ݒ肳��Ă���}�b�v�ŁC���̃`�F�b�N�|�C���g�Ɉړ�����@�\��񋟂��܂��D";
            detail = "�`�F�b�N�|�C���g���ЂƂ����Ȃ��}�b�v�ł́C�˂ɓ����ꏊ�Ɉړ����܂��D";
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if (values == null || values.Count == 0) { return; }

            else if (values.Count == 1)
            {
                var prev = MapSystem.CurrentMap.Index;
                MapSystem.CurrentMap.Next();
                var current = MapSystem.CurrentMap.Index;

                if (MapSystem.CurrentMap.respawnPositions.Length == 1)
                {
                    tracer.AddMessage("���݂̃}�b�v�ɂ̓`�F�b�N�|�C���g��1��������܂���D", Tracer.Level.warning);
                }

                else
                {
                    tracer.AddMessage("check point : " + prev.ToString() + " -> " + current.ToString(), Tracer.Level.normal);
                }
            }

            else
            {
                ERROR_OverValues(tracer);
            }
        }
    }
}

