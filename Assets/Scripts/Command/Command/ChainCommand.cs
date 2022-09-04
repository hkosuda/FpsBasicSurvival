using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices.ComTypes;
using System.Diagnostics;

namespace MyGame
{
    public class ChainCommand : Command
    {
        static List<string> availables = new List<string>()
        {
            "save", "rollback", "back", "start", "stop", "replay",
        };

        static List<List<float[]>> dataListList;

        static bool recording;
        static float pastTime;

        public ChainCommand(string commandName) : base(commandName)
        {
            description = "�L�^�𕡐��ɕ����āC�Ō�ɋL�^�����f�[�^����J�n����@�\�C�L�^�Ɋ�Â��ă��v���C���s���@�\�Ȃǂ�񋟂��܂��D";
            detail = "'start'�ŋL�^���J�n���܂��D�܂��C'stop'�ŋL�^���~���܂��D'save'�Ō��݂̃f�[�^��ۑ����C�V�����f�[�^�̋L�^���J�n���܂��D'rollback'�Œ��O�̃f�[�^��j�����܂��D\n" +
                "'back'�Œ��O�ɋL�^�����f�[�^�̍Ō�̏�Ԃ𕜌����܂��D'replay'�Ń��v���C���J�n���܂��D\n" +
                "�������ߐݒ�F\n" +
                "bind mouse0 \"chain save -f\"" +
                "bind q \"chain back -f\"" +
                "bind f \"chain start -f\"" +
                "bind b \"chain rollback -f\"" +
                "";
        }

        public override void Initialize()
        {
            dataListList = new List<List<float[]>>() { new List<float[]>() };
            SetEvent(1);
        }

        public override void Shutdown()
        {
            dataListList = null;
            SetEvent(-1);
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                MapSystem.Initialized += FinishRecording;
            }

            else
            {
                MapSystem.Initialized -= FinishRecording;
            }
        }

        static void FinishRecording(object obj, bool mute)
        {
            recording = false;
            dataListList = null;
        }

        public override List<string> AvailableValues(List<string> values)
        {
#if UNITY_EDITOR
            availables.Add("output");
#endif

            if (values.Count < 3) { return availables; }
            return new List<string>();
        }

        public override void CommandMethod(Tracer tracer, List<string> values)
        {
            if(dataListList == null) { dataListList = new List<List<float[]>>() { new List<float[]>() }; }

            if (values.Count == 1)
            {
                tracer.AddMessage("�l���w�肵�Ă�������", Tracer.Level.error);
            }

            else if (values.Count == 2)
            {
                var value = values[1];

                if (value == "save")
                {
                    if (dataListList.Count == 0) { dataListList.Add(new List<float[]>()); }

                    if (dataListList.Last().Count == 0) { return; }
                    dataListList.Add(new List<float[]>());
                }

                else if (value == "rollback")
                {
                    var currentIndex = dataListList.Count - 1;
                    var prevIndex = currentIndex - 1;

                    if (prevIndex < 0)
                    {
                        tracer.AddMessage("���݂̃C���f�b�N�X���O�ɖ߂邱�Ƃ��ł��܂���", Tracer.Level.error);
                        return; 
                    }

                    dataListList.RemoveAt(currentIndex);

                    var lastDataList = dataListList[prevIndex];
                    dataListList[prevIndex] = new List<float[]>();

                    if (dataListList.Count == 0)
                    {
                        dataListList.Add(new List<float[]>());

                        var data = lastDataList.First();

                        RestoreStatus(data);
                        pastTime = data[0];
                    }

                    else
                    {
                        if (dataListList.Count > 0 && dataListList.Last().Count > 0)
                        {
                            var data = dataListList.Last().Last();
                            RestoreStatus(data);
                            pastTime = data[0];
                        }
                    }
                }

                else if (value == "back")
                {
                    if (dataListList == null) { tracer.AddMessage("�f�[�^�����݂��܂���", Tracer.Level.error); return; }
                    if (dataListList.Count == 0 || dataListList.Last().Count == 0) { tracer.AddMessage("�f�[�^�����݂��܂���", Tracer.Level.error); return; }

                    var prevIndex = dataListList.Count - 2;

                    if (prevIndex < 0)
                    {
                        var data = dataListList[0].First();

                        RestoreStatus(data);
                        pastTime = data[0];

                        dataListList[0] = new List<float[]>();
                    }

                    else
                    {
                        var data = dataListList[prevIndex].Last();

                        RestoreStatus(data);
                        pastTime = data[0];

                        dataListList[prevIndex + 1] = new List<float[]>();
                    }
                }

                else if (value == "start")
                {
                    recording = true;
                    dataListList = new List<List<float[]>>() { new List<float[]>() };

                    pastTime = 0.0f;

                    tracer.AddMessage("�L�^���J�n���܂���", Tracer.Level.normal);
                }

                else if (value == "stop")
                {
                    recording = false;
                    tracer.AddMessage("�L�^���~���܂���", Tracer.Level.normal);
                }

#if UNITY_EDITOR
                else if (value == "output")
                {
                    if (dataListList == null) { tracer.AddMessage("�f�[�^�����݂��܂���", Tracer.Level.error); return; }
                    if (dataListList.Count == 0 || dataListList.Last().Count == 0) { tracer.AddMessage("�f�[�^�����݂��܂���", Tracer.Level.error); return; }

                    var dataList = new List<float[]>();

                    foreach(var list in dataListList)
                    {
                        if (list.Count == 0) { continue; }
                        dataList.AddRange(list);
                    }

                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var cachedData = new CachedData(dataList, MapSystem.CurrentMap.MapName);

                    RecordDataIO.TrySave(fileName, cachedData, tracer);
                }
#endif

                else if (value == "replay")
                {
                    if (dataListList == null) { tracer.AddMessage("�f�[�^�����݂��܂���", Tracer.Level.error); return; }
                    if (dataListList.Count == 0 || dataListList.Last().Count == 0) { tracer.AddMessage("�f�[�^�����݂��܂���", Tracer.Level.error); return; }

                    var dataList = new List<float[]>();

                    foreach (var list in dataListList)
                    {
                        if (list.Count == 0) { continue; }
                        dataList.AddRange(list);
                    }

                    var cachedData = new CachedData(dataList, MapSystem.CurrentMap.MapName);

                    if (ReplaySystem.TryBeginReplay(cachedData, tracer))
                    {
                        Console.CloseConsole();
                    }
                }

                else
                {
                    tracer.AddMessage("�����Ȓl�F" + value, Tracer.Level.error);
                }
            }

            else
            {
                tracer.AddMessage("�l�̐����ߏ�ł�", Tracer.Level.error);
            }
        }

        public override void Update(float dt)
        {
            if (dataListList == null) { dataListList = new List<List<float[]>>() { new List<float[]>() }; }
            if (!recording) { return; }

            pastTime += dt;

            var data = new float[RecordSystem.dataSize];
            var pos = Player.Myself.transform.position;
            var rot = PM_Camera.EulerAngle();
            var v = Player.Rb.velocity;

            data[0] = pastTime;
            data[1] = pos.x;
            data[2] = pos.y;
            data[3] = pos.z;
            data[4] = rot.x;
            data[5] = rot.y;
            data[6] = rot.z;
            data[7] = v.x;
            data[8] = v.y;
            data[9] = v.z;
            data[10] = CheckInput(Keyconfig.CheckInput(KeyAction.forward, false));
            data[11] = CheckInput(Keyconfig.CheckInput(KeyAction.backward, false));
            data[12] = CheckInput(Keyconfig.CheckInput(KeyAction.right, false));
            data[13] = CheckInput(Keyconfig.CheckInput(KeyAction.left, false));
            data[14] = CheckInput(Keyconfig.CheckInput(KeyAction.crouch, false));
            data[15] = CheckInput(Keyconfig.CheckInput(KeyAction.jump, true) || Keyconfig.CheckInput(KeyAction.autoJump, false));

            dataListList.Last().Add(data);

            if (pastTime > RecorderCommand.recorderLimitTime)
            {
                ChatMessageManager.SendChatMessage(RecorderCommand.recorderLimitTime.ToString() + "�b���o�߂������߁C���R�[�_�[���~���܂��D");
                recording = false;
            }

            // - inner function
            static float CheckInput(bool value)
            {
                if (value) { return 1.0f; }
                return 0.0f;
            }
        }

        static void RestoreStatus(float[] data)
        {
            var posX = data[1];
            var posY = data[2];
            var posZ = data[3];

            var rotX = data[4];
            var rotY = data[5];
            var rotZ = data[6];

            var vx = data[7];
            var vy = data[8];
            var vz = data[9];

            Player.SetPosition(new Vector3(posX, posY, posZ), new Vector3(rotX, rotY, rotZ));
            Player.Rb.velocity = new Vector3(vx, vy, vz);

            PM_PlaneVector.SetPlaneVector(new Vector2(vx, vy));
        }
    }
}
