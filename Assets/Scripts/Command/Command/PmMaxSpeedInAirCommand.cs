using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class PmMaxSpeedInAirCommand : Command
    {
        static readonly float speed_min = 0.0f;
        static readonly float speed_max = 10.0f;

        public PmMaxSpeedInAirCommand(string commandName) : base(commandName)
        {
            description = "�󒆂ł̍ő呬�x��ݒ�ł��܂��D";
            detail = "���̒l�̓X�g���C�t���̉����ɉe�����܂��D�l���傫���قǃX�g���C�t���̉������傫���Ȃ�܂��D\n" +
                "�Ȃ��C�����Őݒ肵���l�̓Q�[�����[�h��؂�ւ��邽�тɃf�t�H���g�̒l�Ƀ��Z�b�g����܂��D�܂��C�T�o�C�o�����[�h�ł͗��p�ł��܂���D\n" +
                "0.0�ȏ�10.0�ȉ��̒l��ݒ�\�ł��D�f�t�H���g�l��" + Params.default_pm_max_speed_in_air.ToString("F1") + "�ł��D";
        }

        public override List<string> AvailableValues(List<string> values)
        {
            return new List<string>();
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if(values.Count == 1)
            {
                tracer.AddMessage("�l���w�肵�Ă��������D", Tracer.Level.error);
            }

            else if (values.Count == 2)
            {
                var value = values[1];

                if (float.TryParse(value, out var num))
                {
                    if (speed_min <= num && num <= speed_max)
                    {
                        var before = Params.pm_max_speed_in_air.ToString();
                        var after = num.ToString();

                        tracer.AddMessage("pm_max_speed_in_air : " + before + " -> " + after, Tracer.Level.normal);
                        Params.pm_max_speed_in_air = num;
                        
                    }

                    else
                    {
                        tracer.AddMessage("�w�肵���l���L���Ȕ͈͓��ł͂���܂���D�L���Ȓl�͈̔͂�0.0�ȏ�10.0�ȉ��ł��D", Tracer.Level.error);
                    }
                }

                else
                {
                    tracer.AddMessage(value + "��L���Ȑ��l�ɕϊ��ł��܂���", Tracer.Level.error);
                }
            }

            else
            {
                ERROR_OverValues(tracer);
            }
        }

        public override void Shutdown()
        {
            Params.pm_max_speed_in_air = Params.default_pm_max_speed_in_air;
        }
    }
}

