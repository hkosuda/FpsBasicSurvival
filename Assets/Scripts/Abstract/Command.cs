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
            tracer.AddMessage("整数に変換できません : " + str, Tracer.Level.error, indent);
        }

        static protected void ERROR_OutOfRange(Tracer tracer, int index, int indexLim, int indent = 1)
        {
            var error = "有効なインデックスの範囲外です．有効な範囲は' 0 ～ " + indexLim.ToString() + "' です : " + index.ToString();
            tracer.AddMessage(error, Tracer.Level.error, indent);
        }

        static protected void ERROR_OverValues(Tracer tracer, int indent = 1)
        {
            var error = "値の数が過剰です";
            tracer.AddMessage(error, Tracer.Level.error, indent);
        }

        static protected void ERROR_AvailableOnly(Tracer tracer, List<string> values, int indent = 1)
        {
            if (values == null || values.Count == 0) { return; }

            var message = "指定された値が有効ではありません．利用可能な値は次の通りです : ";

            foreach (var value in values)
            {
                message += value + " ";
            }

            tracer.AddMessage(message, Tracer.Level.error, indent);
        }

        static protected void ERROR_InvalidKey(Tracer tracer, string keyString, int indent = 1)
        {
            var error = "有効なキーに変換できません : " + keyString;
            tracer.AddMessage(error, Tracer.Level.error, indent);
        }

        static protected string ERROR_InvalidKeyAlert()
        {
            return "ゲーム内で使用できるキーの名称を調べるには，'keycheck'コマンドを使用してください．";
        }

        static protected void ERROR_NeedValue(Tracer tracer)
        {
            var error = "値を指定してください";
            tracer.AddMessage(error, Tracer.Level.error);
        }
    }
}

