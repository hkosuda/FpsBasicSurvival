using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public abstract class Command
    {
        

        public enum CommandType
        {
            normal,
            values,
        }

        public string commandName;
        public string description = "";
        public string detail = "";
        public int counter = 0;

        public CommandType commandType = CommandType.normal;

        public Command(string commandName)
        {
            this.commandName = commandName;
        }

        public virtual List<string> AvailableValues(List<string> values)
        {
            return new List<string>();
        }

        public abstract void CommandMethod(Tracer tracer, List<string> values);

        public virtual void Update(float dt) { return; }

        public virtual void Initialize() { }
        public virtual void Shutdown() { }

        //
        // UTILITY FUNCTIONS

        static protected void ERROR_NotInteger(Tracer tracer, string str, int indent = 1)
        {
            tracer.AddMessage("�����ɕϊ��ł��܂��� : " + str, Tracer.Level.error, indent);
        }

        static protected void ERROR_OutOfRange(Tracer tracer, int index, int indexLim, int indent = 1)
        {
            var error = "�L���ȃC���f�b�N�X�͈̔͊O�ł��D�L���Ȕ͈͂�' 0 �` " + indexLim.ToString() + "' �ł� : " + index.ToString();
            tracer.AddMessage(error, Tracer.Level.error, indent);
        }

        static protected void ERROR_OverValues(Tracer tracer, int indent = 1)
        {
            var error = "�l�̐����ߏ�ł�";
            tracer.AddMessage(error, Tracer.Level.error, indent);
        }

        static protected void ERROR_AvailableOnly(Tracer tracer, List<string> values, int indent = 1)
        {
            if (values == null || values.Count == 0) { return; }

            var message = "�w�肳�ꂽ�l���L���ł͂���܂���D���p�\�Ȓl�͎��̒ʂ�ł� : ";

            foreach (var value in values)
            {
                message += value + " ";
            }

            tracer.AddMessage(message, Tracer.Level.error, indent);
        }

        static protected void ERROR_InvalidKey(Tracer tracer, string keyString, int indent = 1)
        {
            var error = "�L���ȃL�[�ɕϊ��ł��܂��� : " + keyString;
            tracer.AddMessage(error, Tracer.Level.error, indent);
        }

        static protected string ERROR_InvalidKeyAlert()
        {
            return "�Q�[�����Ŏg�p�ł���L�[�̖��̂𒲂ׂ�ɂ́C'keycheck'�R�}���h���g�p���Ă��������D";
        }

        static protected void ERROR_NeedValue(Tracer tracer)
        {
            var error = "�l���w�肵�Ă�������";
            tracer.AddMessage(error, Tracer.Level.error);
        }
    }
}

