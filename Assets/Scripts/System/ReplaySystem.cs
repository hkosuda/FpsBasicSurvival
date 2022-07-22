using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class ReplaySystem : MonoBehaviour
    {
        static public EventHandler<float[]> Updated { get; set; }
        static public EventHandler<bool> ReplayBegin { get; set; }
        static public float PastTime { get; private set; }
        static public float[] InterpolatedData { get; private set; }

        static public float EndTime { get; private set; }

        static List<float[]> dataList;

        static PlayerStatus playerStatus;

        static GameObject _controller;
        static GameObject controller;

        private void Awake()
        {
            _controller = Resources.Load<GameObject>("UI/ReplayController");
        }

        private void Start()
        {
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
                ReplayTimer.Updated += UpdateMethod;
            }

            else
            {
                ReplayTimer.Updated -= UpdateMethod;
            }
        }

        static void UpdateMethod(object obj, float dt)
        {
            if (dataList == null) { Debug.Log("NULL"); return; }

            PastTime += dt;
            if (PastTime > EndTime) { PastTime = EndTime; }

            SetPosition();
        }

        static public bool TryBeginReplay(CachedData cachedData, Tracer tracer)
        {
            if (cachedData.mapName != MapSystem.CurrentMap.MapName)
            {
                var error = "現在のマップと異なるため，再生できません．";
                tracer.AddMessage(error, Tracer.Level.error);

                return false;
            }

            if (cachedData.dataList == null || cachedData.dataList.Count == 0)
            {
                var error = "値のデータが取得できません．";
                tracer.AddMessage(error, Tracer.Level.error);

                return false;
            }

            if (controller == null)
            {
                controller = Instantiate(_controller);
            }

            Debug.Log(cachedData.dataList.Count);

            dataList = cachedData.dataList;
            playerStatus = new PlayerStatus();

            PastTime = 0.0f;
            EndTime = dataList.Last()[0];
            Debug.Log(EndTime);
            dataList = cachedData.dataList;

            SetPosition();
            ReplayBegin?.Invoke(null, true);

            return true;
        }

        static public void FinishReplay()
        {
            if (controller != null)
            {
                Destroy(controller);
            }

            if (playerStatus == null)
            {
                GameSystem.SwitchHost(HostName.survival);
            }

            else
            {
                playerStatus.RollBack();
            }
            
            playerStatus = null;
            dataList = null;
        }

        static public void Backward()
        {
            PastTime -= 2.0f;
            if (PastTime < 0.0f) { PastTime = 0.0f; }

            SetPosition();
        }

        static public void Forward()
        {
            PastTime += 2.0f;
            if (PastTime > EndTime) { PastTime = EndTime; }

            SetPosition();
        }

        static public void ToTheStart()
        {
            PastTime = 0.0f;
            SetPosition();
        }

        static public void ToTheEnd()
        {
            PastTime = EndTime;
            SetPosition();
        }

        static public void SetTime(float pastTime)
        {
            PastTime = pastTime;
            SetPosition();
        }

        static void SetPosition()
        {
            if (dataList == null) { return; }

            InterpolatedData = ReplayUtil.Interpolate(PastTime, dataList);
            var d = InterpolatedData;

            var x = d[1];
            var y = d[2];
            var z = d[3];

            var position = new Vector3(x, y, z);
            Debug.Log(position.ToString() + ", " + PastTime.ToString());

            var rx = d[4];
            var ry = d[5];
            var rz = d[6];

            var eulerAngle = new Vector3(rx, ry, rz);
            Player.SetPosition(position, eulerAngle);
            Debug.Log("Set Position");
            Updated?.Invoke(null, InterpolatedData);
        }
    }

    public class PlayerStatus
    {
        public Vector3 position;
        public Vector3 eulerAngle;
        public Vector3 velocity;

        public PlayerStatus()
        {
            position = Player.Myself.transform.position;
            eulerAngle = Player.Camera.transform.eulerAngles;
            velocity = Player.Rb.velocity;
        }

        public void RollBack()
        {

        }
    }
}

