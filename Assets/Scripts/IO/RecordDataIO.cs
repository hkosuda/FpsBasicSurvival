using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MyGame
{
    public class RecordDataIO : MonoBehaviour
    {
        public enum Info
        {
            map, value,
        }

        static readonly string end = "end";
        static readonly string accuracy = "f3";

        static public readonly string folderName = "Record";
        static public readonly string extension = ".txt";

        static public bool TryLoad(string fileName, out CachedData cachedData, Tracer tracer)
        {
            if (TryRead(fileName, out var completeData))
            {
                var splitted = completeData.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (splitted == null) { cachedData = null; return false; }

                var lineList = new List<string>(splitted);

                if (!Value2MapName(GetValue(TxtUtil.L(Info.map), lineList), out var mapName))
                {
                    var error = "マップ情報を読み込めませんでした";
                    tracer.AddMessage(error, Tracer.Level.error);
                }

                var value = GetValue(TxtUtil.L(Info.value), lineList);

                if (!Value2DataList(value, out var dataList))
                {
                    var error = "値の情報を読み込めませんでした";
                    tracer.AddMessage(error, Tracer.Level.error);
                }

                if (!tracer.NoError) { cachedData = null; return false; }

                cachedData = new CachedData(dataList, mapName);
                return true;
            }

            else
            {
                var error = "ファイルを読み込めません";
                tracer.AddMessage(error, Tracer.Level.error);
            }

            cachedData = null;
            return false;

            // - inner function
            static bool Value2DataList(string value, out List<float[]> dataList)
            {
                dataList = new List<float[]>();

                var splitted = value.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (splitted == null) { return false; }

                foreach (var _line in splitted)
                {
                    var line = _line.Trim();

                    if (Line2Data(line, out var data))
                    {
                        dataList.Add(data);
                    }

                    else
                    {
                        return false;
                    }
                }

                return true;

                // - - inner function
                static bool Line2Data(string line, out float[] data)
                {
                    data = new float[RecordSystem.dataSize];

                    var splitted = line.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (splitted.Length != RecordSystem.dataSize) { return false; }

                    for (var n = 0; n < RecordSystem.dataSize; n++)
                    {
                        var s = splitted[n];

                        if (float.TryParse(s, out var num))
                        {
                            data[n] = num;
                        }

                        else
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }
        }

        // - inner function
        static bool TryRead(string fileName, out string fullText)
        {
            var asset = Resources.Load<TextAsset>(ReadFilePath(fileName));

            if (asset == null)
            {
                fullText = "";
                return false;
            }

            else
            {
                fullText = asset.text;
                return true;
            }
        }

        // - inner function
        static string GetValue(string name, List<string> splitted)
        {
            if (splitted == null) { return ""; }

            var begin = false;
            var value = "";

            foreach (var _line in splitted)
            {
                var line = _line.Trim();
                if (line == name) { begin = true; continue; }

                if (begin && line == end) { return value.TrimEnd(new char[] { '\n' }); }
                if (!begin) { continue; }

                value += line + "\n";
            }

            return "";
        }

        // - inner function
        static bool Value2MapName(string value, out MapName mapName)
        {
            foreach (MapName name in Enum.GetValues(typeof(MapName)))
            {
                if (value.ToLower() == name.ToString().ToLower())
                {
                    mapName = name;
                    return true;
                }
            }

            mapName = MapName.ez_tower;
            return false;
        }

        // - inner function
        static bool Value2DateTime(string value, out DateTime dateTime)
        {
            var dataSize = 6;

            var s = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length != dataSize) { dateTime = DateTime.Now; return false; }

            var n = new int[dataSize];

            for (var i = 0; i < dataSize; i++)
            {
                if (int.TryParse(s[i], out var num))
                {
                    n[i] = num;
                }

                else
                {
                    dateTime = DateTime.Now;
                    return false;
                }
            }

            dateTime = new DateTime(n[0], n[1], n[2], n[3], n[4], n[5]);
            return true;
        }

        static public bool TrySave(string fileName, CachedData cachedData, Tracer tracer)
        {
            CreateDirectory();
            var filePath = WriteFilePath(fileName);

            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, false))
                {
                    var content = CreateFileContent(cachedData);
                    sw.WriteLine(content);
                }

                return true;
            }

            catch
            {
                tracer.AddMessage("ファイルの作成に失敗しました", Tracer.Level.error);
                return false;
            }

            // - inner function
            static string CreateFileContent(CachedData cachedData)
            {
                var content = "";
                var value = DataList2DataLines(cachedData.dataList);

                content += WriteHeader(cachedData, value);

                content += TxtUtil.L(Info.value) + "\n";
                content += value;
                content += end + "\n\n";

                return content;
            }

            // - inner function
            static string DataList2DataLines(List<float[]> dataList)
            {
                var dataLines = "";

                foreach (var data in dataList)
                {
                    var text = Data2Line(data);
                    dataLines += "\t" + text + "\n";
                }

                return dataLines;

                // - - inner function
                static string Data2Line(float[] data)
                {
                    var line = "";

                    for (var n = 0; n < data.Length; n++)
                    {
                        var value = data[n];

                        if (n >= 10)
                        {
                            line += value.ToString() + ",";
                        }

                        else
                        {
                            line += value.ToString(accuracy) + ",";
                        }
                    }

                    return line;
                }
            }
        }

        static public string WriteFilePath(string filename)
        {
            return WriteFileDirectory() + filename + extension;
        }

        static public string WriteFileDirectory()
        {
            return Application.dataPath + "/" + folderName + "/";
        }

        static public string ReadFilePath(string fileName)
        {
            return ReadFileDirectory() + fileName;
        }

        static public string ReadFileDirectory()
        {
            return "Record/";
        }

        static void CreateDirectory()
        {
            var message = "";

            if (!Directory.Exists(WriteFileDirectory()))
            {
                try
                {
                    message = "フォルダを作成しました．";
                    Directory.CreateDirectory(WriteFileDirectory());
                }

                catch
                {
                    message = "フォルダの作成に失敗しました．";
                }
            }

#if UNITY_EDITOR
            Debug.Log(message);
#endif
        }

        static string WriteHeader(CachedData cachedData, string value)
        {
            var header = "";
            header = AddInfo(header, TxtUtil.L(Info.map), cachedData.mapName.ToString());

            return header;

            // - inner function
            static string AddInfo(string header, string name, string value)
            {
                header += name + "\n";
                header += "\t" + value + "\n";
                header += end + "\n\n";

                return header;
            }
        }
    }
}

