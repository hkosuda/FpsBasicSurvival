using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Ghost : MonoBehaviour
    {
        // public
        static public float[] InterpolatedData { get; private set; }

        // params
        static List<float[]> dataList;
        static MapName mapName;

        static float pastTime;

        // objects
        static GameObject _ghost;
        static GameObject _ghostLine;

        static GameObject ghost;
        static GameObject ghostBox;

        static GameObject ghostLineObject;
        static LineRenderer ghostLine;

        static Vector3 prevLinePos;

        private void Awake()
        {
            ghostBox = gameObject.transform.GetChild(0).gameObject;
        }

        void Start()
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
                TimerSystem.Updated += UpdateMethod;
                TimerSystem.TimerPaused += OnTimerPaused;
            }

            else
            {
                TimerSystem.Updated -= UpdateMethod;
                TimerSystem.TimerPaused -= OnTimerPaused;
            }
        }

        static void UpdateMethod(object obj, float dt)
        {
            if (dataList == null || dataList.Count == 0) { SetVisibility(false); return; }
            if (mapName != MapSystem.CurrentMap.MapName) { SetVisibility(false); return; }

            pastTime += dt; Debug.Log(pastTime);
            if (pastTime > dataList.Last()[0]) { Repeat(); return; }

            InterpolatedData = ReplayUtil.Interpolate(pastTime, dataList);

            SetVisibility(true);
            UpdateTransform();
            UpdateLine();
        }

        static void UpdateTransform()
        {
            ghost.transform.position = Vec3(InterpolatedData);
            ghost.transform.eulerAngles = new Vector3(0.0f, InterpolatedData[5], 0.0f);
        }

        static void UpdateLine()
        {
            var pos = Vec3(InterpolatedData, -Player.centerY);

            if ((prevLinePos - pos).magnitude > 10.0f)
            {
                InitializeLine(pos);
            }

            else
            {
                ghostLine.positionCount++;
                ghostLine.SetPosition(ghostLine.positionCount - 1, pos);
            }

            prevLinePos = pos;
        }

        static public void BeginReplay(List<float[]> _dataList, MapName _mapName)
        {
            EndReplay();

            if (_ghost == null) { _ghost = Resources.Load<GameObject>("Ghost/Ghost"); }
            if (_ghostLine == null) { _ghostLine = Resources.Load<GameObject>("Ghost/GhostLine"); }

            if (GameSystem.Root == null) { return; }

            dataList = _dataList;
            mapName = _mapName;

            pastTime = 0.0f;

            if (dataList == null || dataList.Count == 0) { return; }

            ghost = GameHost.Instantiate(_ghost);
            ghost.transform.position = Vec3(dataList[0]);

            ghostLineObject = GameHost.Instantiate(_ghostLine);
            ghostLine = ghostLineObject.transform.GetChild(0).gameObject.GetComponent<LineRenderer>();

            InitializeLine(Vec3(dataList[0], -Player.centerY));
        }

        static public void EndReplay()
        {
            if (ghost == null) { return; }

            Destroy(ghost);
            Destroy(ghostLineObject);
        }

        static void Repeat()
        {
            if (dataList != null && dataList.Count > 0)
            {
                ghostLine.positionCount = 1;
                ghostLine.SetPosition(0, Vec3(dataList[0], -Player.centerY));
            }

            else
            {
                ghostLine.positionCount = 1;
                ghostLine.SetPosition(0, Vector3.zero);
            }

            pastTime = 0.0f;
        }

        static void OnTimerPaused(object obj, bool mute)
        {
            SetVisibility(false);
        }

        static public Vector3 Position()
        {
            if (ghost == null) { return new Vector3(); }
            return ghost.transform.position;
        }

        //
        // utilities
        static Vector3 Vec3(float[] data, float dy = 0.0f)
        {
            return new Vector3(data[1], data[2] + dy, data[3]);
        }

        static void SetVisibility(bool visibility)
        {
            if (TimerSystem.Paused) { visibility = false; }

            if (ghostBox != null) { ghostBox.SetActive(visibility); }
            if (ghostLineObject != null) { ghostLineObject.SetActive(visibility); }
        }

        static void InitializeLine(Vector3 pos)
        {
            ghostLine.positionCount = 1;
            ghostLine.SetPosition(0, pos);

            prevLinePos = pos;
        }
    }
}

