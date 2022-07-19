using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MyGame
{
    public enum CommandName
    {
        Invoke, Bind, Toggle, Exit, Back, Next, Local, Quit, Begin, Save, Load, Info, Recorder, Replay
    }

    public class CommandReceiver : MonoBehaviour
    {
        static public EventHandler<Tracer> RequestEnd { get; set; }
        static public List<Command> CommandList { get; private set; } = new List<Command>()
        {
            new InvokeCommand(TxtUtil.L(CommandName.Invoke)),
            new BindCommand(TxtUtil.L(CommandName.Bind)),
            new ToggleCommand(TxtUtil.L(CommandName.Toggle)),

            new ExitCommand(TxtUtil.L(CommandName.Exit)),
            new QuitCommand(TxtUtil.L(CommandName.Quit)),

            new BeginCommand(TxtUtil.L(CommandName.Begin)),
        };

        static public void AddCommand(Command command)
        {
            if (CommandList == null) { CommandList = new List<Command>(); }

            foreach(var c in CommandList)
            {
                if (c.commandName == command.commandName)
                {
                    return;
                }
            }

            CommandList.Add(command);
        }

        static public void SubCommand(string commandName)
        {
            if (CommandList == null || CommandList.Count == 0) { return; }

            for(var n = CommandList.Count - 1; n > -1; n--)
            {
                var command = CommandList[n];

                if (command.commandName == commandName)
                {
                    CommandList.RemoveAt(n);
                }
            }
        }

        static public Tracer RequestCommand(string _code, Tracer _tracer)
        {
            var code = string.Copy(_code);
            var options = GetOptions(code);

            code = code.ToLower();
            code = Grouping(code);
            code = CorrectSentence(code);

            var tracer = new Tracer(_tracer, Tracer.Option.none);
            tracer.AddMessage("> " + code, Tracer.Level.normal, 0);

            if (CommandList == null || CommandList.Count == 0) { NotAvailable(tracer); RequestEnd?.Invoke(null, tracer); return tracer; }

            var values = GetValues(code);
            if (values == null || values.Count == 0) { return tracer; }

            var command = GetCommand(values[0]);
            if (command == null) { UnknownCommand(values[0], tracer); RequestEnd?.Invoke(null, tracer); return tracer; }

            command.CommandMethod(tracer, values);
            RequestEnd?.Invoke(null, tracer);

            return tracer;

            // - inner function
            static void NotAvailable(Tracer tracer)
            {
                var error = "利用可能なコマンドはありません";
                tracer.AddMessage(error, Tracer.Level.error, 1);
            }

            // - inner function
            static void UnknownCommand(string commandName, Tracer tracer)
            {
                var error = "不明なコマンド : " + commandName;
                tracer.AddMessage(error, Tracer.Level.error, 1);
            }

            // - inner function
            static Command GetCommand(string commandName)
            {
                foreach(var command in CommandList)
                {
                    if (command.commandName == commandName)
                    {
                        return command;
                    }
                }

                return null;
            }
        }

        static public string Grouping(string sentence)
        {
            var rgx = new Regex(@"\"".*?\""");

            sentence = rgx.Replace(sentence, _Grouping);

            return sentence;

            // - inner function
            static string _Grouping(Match match)
            {
                if (match.Value == null)
                {
                    return "//";
                }

                var value = match.Value;
                value = value.TrimEnd(new char[1] { '"' });
                value = value.TrimStart(new char[1] { '"' });

                var splitted = SplitString(value);
                var group = "/";

                foreach (var s in splitted)
                {
                    group += s + "/";
                }

                return group;
            }
        }

        static List<string> SplitString(string str)
        {
            // 1st : hankaku, 2nd : zenkaku
            return new List<string>(str.Split(new string[] { " ", "　" }, StringSplitOptions.RemoveEmptyEntries));
        }

        static bool IsOption(string value)
        {
            return Regex.IsMatch(value, @"\A-[a-z]+\z");
        }

        static public string CorrectSentence(string sentence)
        {
            var values = GetValues(sentence);
            var options = GetOptions(sentence);

            var corrected = "";

            if (values != null)
            {
                foreach (var value in values)
                {
                    corrected += value + " ";
                }
            }

            if (options != null && options.Count > 0)
            {
                corrected += options.Last();
            }

            return corrected.TrimEnd(new char[1] { ' ' });
        }

        static public List<string> GetValues(string sentence)
        {
            var splitted = SplitString(sentence);
            if (splitted == null || splitted.Count == 0) { return null; }

            var values = new List<string>();

            for (var n = 0; n < splitted.Count; n++)
            {
                var value = splitted[n].ToLower();

                if (!IsOption(value))
                {
                    values.Add(value);
                }
            }

            return values;
        }

        static public List<string> GetOptions(string sentence)
        {
            var splitted = SplitString(sentence);
            if (splitted == null) { return null; }

            var options = new List<string>();

            for (var n = 0; n < splitted.Count; n++)
            {
                var value = splitted[n].ToLower();

                if (IsOption(value))
                {
                    options.Add(value);
                }
            }

            return options;
        }

        static public string UnpackGrouping(string group)
        {
            if (Regex.IsMatch(group, @"\/.*\/"))
            {
                var splitted = group.Split(new string[1] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                var corrected = "";

                if (splitted == null) { return corrected; }

                foreach (var s in splitted)
                {
                    corrected += s + " ";
                }

                return corrected.Trim();
            }

            else
            {
                return group.Trim();
            }
        }
    }
}

