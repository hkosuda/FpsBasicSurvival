using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class ConsoleLogManager : MonoBehaviour
    {
        static readonly int defaultOffsetSize = 2;
        static readonly int offsetSize = 12;

        static public EventHandler<bool> Updated { get; set; }

        static List<GameObject> logList = new List<GameObject>();
        static GameObject _consoleLog;

        static List<Log> cachedLogList = new List<Log>();

        static public void Initialize()
        {
            AddMessage(TxtUtil.C("- FPS_Basic_Survival - ver 01.04", Clr.lime));
            SetEvent(1);
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                CommandReceiver.RequestEnd += WriteLog;
                Console.Opened += WriteCachedLog;
            }

            else
            {
                CommandReceiver.RequestEnd -= WriteLog;
                Console.Opened -= WriteCachedLog;
            }
        }

        static void WriteLog(object obj, Tracer tracer)
        {
            tracer.WriteLog();
        }

        static void WriteCachedLog(object obj, bool mute)
        {
            if (cachedLogList != null)
            {
                foreach(var log in cachedLogList)
                {
                    InstantiateLog(log.message, log.indent);
                }
            }

            cachedLogList = new List<Log>();
        }

        static public void AddMessage(string message, int indent = 0)
        {
            if (_consoleLog == null) { _consoleLog = Resources.Load<GameObject>("UiComponent/ConsoleLog"); }
            if (logList == null) { logList = new List<GameObject>(); }

            if (cachedLogList == null) { cachedLogList = new List<Log>(); }

            if (Console.Active)
            {
                InstantiateLog(message, indent);
            }

            else
            {
                cachedLogList.Add(new Log(message, indent));
            }

            Updated?.Invoke(null, false);
        }

        static public void ClearLog()
        {
            if (logList != null)
            {
                for (var n = logList.Count - 1; n > -1; n--)
                {
                    Destroy(logList[n]);
                }
            }

            logList = new List<GameObject>();
            cachedLogList = new List<Log>();
        }

        static void InstantiateLog(string message, int indent)
        {
            var consoleLog = GameObject.Instantiate(_consoleLog);
            consoleLog.transform.SetParent(Console.ConsoleLogContent.transform);

            logList.Add(consoleLog);

            var text = consoleLog.GetComponent<TextMeshProUGUI>();
            var offset = defaultOffsetSize + offsetSize * indent;

            text.text = message;
            text.margin = new Vector4(offset, 0, 0, 0);
        }

        class Log
        {
            public string message;
            public int indent;

            public Log(string message, int indent)
            {
                this.message = message;
                this.indent = indent;
            }
        }
    }
}

