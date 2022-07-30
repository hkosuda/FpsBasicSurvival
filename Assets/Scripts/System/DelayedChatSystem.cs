using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class DelayedChatSystem : MonoBehaviour
    {
        static int frameCount = 0;
        static float dtSum = 0.0f;
        static float dtAve = 0.02f;

        static List<MessageTime> mtList = new List<MessageTime>();

        private void Awake()
        {
            mtList = new List<MessageTime>();
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
                GameHost.HostStopped += ClearList;
            }

            else
            {
                GameHost.HostStopped -= ClearList;
            }
        }

        static void ClearList(object obj, bool mute)
        {
            mtList = new List<MessageTime>();
        }

        private void Update()
        {
            CalcDt(Time.deltaTime);

            if (mtList != null)
            {
                for(var n = mtList.Count - 1; n > -1; n--)
                {
                    mtList[n].pastTime += dtAve;
                    if (mtList[n].pastTime < mtList[n].delayTime) { continue; }

                    ChatMessageManager.SendChatMessage(mtList[n].message);
                    mtList.RemoveAt(n);
                }
            }
        }

        static void CalcDt(float dt)
        {
            if (TimerSystem.Paused) { return; }

            dtSum += dt;
            frameCount++;

            if (frameCount >= 10)
            {
                dtAve = dtSum / (float)frameCount;

                dtSum = 0.0f;
                frameCount = 0;
            }
        }

        static public void AddMessage(string message, float delayTime)
        {
            if (mtList == null) { mtList = new List<MessageTime>(); }
            mtList.Add(new MessageTime(message, delayTime));
        }

        class MessageTime
        {
            public string message;
            public float delayTime;
            public float pastTime;

            public MessageTime(string message, float delayTime)
            {
                this.message = message;
                this.delayTime = delayTime;

                pastTime = 0.0f;
            }
        }
    }
}

