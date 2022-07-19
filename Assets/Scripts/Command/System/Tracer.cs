using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Tracer
    {
        public enum Level
        {
            normal,
            emphasis,
            warning,
            error,
        }

        public enum Option
        {
            none,
            mute,
            flash,
            echo,
        }

        public List<MessageIndent> messageList;

        public Tracer parent;
        public Option option;

        public bool NoError;

        public Tracer(Tracer tracer, Option option)
        {
            parent = tracer;
            this.option = option;

            NoError = true;
            messageList = new List<MessageIndent>();
        }

        public void AddMessage(string message, Level level, int indent = 1)
        {
            if (parent != null) { parent.AddMessage(message, level, indent + 1); return; }

            if (level == Level.error) { NoError = false; }

            if (messageList == null) { messageList = new List<MessageIndent>(); }
            MessageIndent mi;

            if (level == Level.emphasis) { mi = new MessageIndent(TxtUtil.C(message, Clr.lime), indent); }
            else if (level == Level.warning) { mi = new MessageIndent(TxtUtil.C(message, Clr.orange), indent); }
            else if (level == Level.error) { mi = new MessageIndent(TxtUtil.C(message, Clr.red), indent); }
            else { mi = new MessageIndent(message, indent); }

            messageList.Add(mi);
        }

        public void WriteLog()
        {
            if (parent != null) { return; }
            if (messageList == null || messageList.Count == 0) { return; }

            if (option == Option.flash || option == Option.mute) { return; }

            foreach (var m in messageList)
            {
                ConsoleLogManager.AddMessage(m.message, m.indent);
            }
        }

        public string FullText()
        {
            if (parent != null) { return parent.FullText(); }
            if (messageList == null || messageList.Count == 0) { return ""; }

            var text = "";

            foreach(var message in messageList)
            {
                text += Offset(message.indent) + message.message + "\n";
            }

            return text;

            // - inner function
            static string Offset(int indent)
            {
                var offset = "";

                for(var n = 0; n < indent; n++)
                {
                    offset += " ";
                }

                return offset;
            }
        }

        public class MessageIndent
        {
            public string message = "";
            public int indent;

            public MessageIndent(string message, int indent)
            {
                this.message = message;
                this.indent = indent;
            }
        }
    }
}

