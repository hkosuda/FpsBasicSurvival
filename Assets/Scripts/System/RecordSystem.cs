using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class RecordSystem : MonoBehaviour
    {
        static public readonly int dataSize = 16;

        static public EventHandler<CachedData> RecordingEnd { get; set; }

        static public CachedData CachedData { get; private set; }

        static List<float[]> dataList;
        static bool recording;
        static float pastTime;

        void Start()
        {
            SetEvent(1);
        }

        void OnDestroy()
        {
            SetEvent(-1);
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                TimerSystem.Updated += UpdateMethod;
                GameSystem.HostSwitched += FFinishRecorder;
            }

            else
            {
                TimerSystem.Updated -= UpdateMethod;
                GameSystem.HostSwitched -= FFinishRecorder;
            }
        }

        static void UpdateMethod(object obj, float dt)
        {
            if (!recording) { return; }
            if (dataList == null) { dataList = new List<float[]>(); }

            pastTime += dt;

            var data = new float[dataSize];
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

            dataList.Add(data);

            if (pastTime > RecorderCommand.recorderLimitTime)
            {
                //ChatMessages.SendChat(limitTime.ToString() + "秒が経過したため，レコーダーを停止します．", ChatMessages.Sender.system);
                FinishRecorder(true);
            }

            // - inner function
            static float CheckInput(bool value)
            {
                if (value) { return 1.0f; }
                return 0.0f;
            }
        }

        static public void BeginRecorder()
        {
            recording = true;

            dataList = new List<float[]>();
            pastTime = 0.0f;
        }

        static public void FinishRecorder(bool cache)
        {
            if (!recording) { return; }
            recording = false;

            if (dataList == null) { dataList = new List<float[]>(); }

            if (cache)
            {
                var mapName = MapSystem.CurrentMap.MapName;
                CachedData = new CachedData(new List<float[]>(dataList), mapName);
            }

            dataList = new List<float[]>();
        }

        static public int DataListSize()
        {
            if (dataList == null) { return 0; }
            return dataList.Count;
        }

        static public void FFinishRecorder(object obj, bool mute)
        {
            FinishRecorder(false);
        }

        static public void TrySave(string fileName, Tracer tracer)
        {
            if (CachedData == null || CachedData.dataList.Count == 0)
            {
                var warning = "キャッシュされたデータが存在しないため，保存に失敗しました";
                tracer.AddMessage(warning, Tracer.Level.warning);

                return;
            }

            RecordDataIO.TrySave(fileName, CachedData, tracer);
        }
    }

    public class CachedData
    {
        public List<float[]> dataList;
        public MapName mapName;

        public CachedData(List<float[]> dataList, MapName mapName)
        {
            if (dataList == null) { dataList = new List<float[]>(); }

            this.dataList = dataList;
            this.mapName = mapName;
        }
    }
}

