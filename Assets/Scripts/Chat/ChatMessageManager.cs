using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class ChatMessageManager : MonoBehaviour
    {
        static readonly int maxChatMessages = 15;

        static public EventHandler<bool> ChatUpdated { get; set; }

        static GameObject _chatMessage;
        static List<ChatMessage> chatMessageList;

        static GameObject scroll;
        static GameObject simple;

        static GameObject scrollContent;

        static public bool Opened { get; private set; }

        private void Awake()
        {
            scroll = gameObject.transform.GetChild(0).gameObject;
            simple = gameObject.transform.GetChild(1).gameObject;

            scrollContent = scroll.transform.GetChild(0).GetChild(0).gameObject;
            Close(null, false);
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
                CommandReceiver.RequestEnd += Echo;

                TimerSystem.TimerPaused += Open;
                TimerSystem.TimerResumed += Close;
            }

            else
            {
                CommandReceiver.RequestEnd -= Echo;

                TimerSystem.TimerPaused -= Open;
                TimerSystem.TimerResumed -= Close;
            }
        }

        static void Echo(object obj, Tracer tracer)
        {
            if (chatMessageList == null) { chatMessageList = new List<ChatMessage>(); }
            if (_chatMessage == null) { _chatMessage = Resources.Load<GameObject>("UiComponent/ChatMessage"); }
            if (tracer.option == Tracer.Option.none || tracer.option == Tracer.Option.mute) { return; }

            SendChatMessage(tracer.FullText());
        }

        static public void SendChatMessage(string message)
        {
            if (chatMessageList == null) { chatMessageList = new List<ChatMessage>(); }
            if (_chatMessage == null) { _chatMessage = Resources.Load<GameObject>("UiComponent/ChatMessage"); }

            var chatMessage = Instantiate(_chatMessage);
            chatMessage.GetComponent<TextMeshProUGUI>().text = message;

            if (Opened)
            {
                chatMessage.transform.SetParent(scrollContent.transform);
            }

            else
            {
                chatMessage.transform.SetParent(simple.transform);
            }

            chatMessageList.Add(new ChatMessage(chatMessage));

            if (chatMessageList.Count > maxChatMessages)
            {
                while (true)
                {
                    if (chatMessageList.Count <= maxChatMessages) { break; }

                    Destroy(chatMessageList[0].gameObject);
                    chatMessageList.RemoveAt(0);
                }
            }

            ChatUpdated?.Invoke(null, false);
        }

        private void Update()
        {
            if (chatMessageList != null)
            {
                for(var n = chatMessageList.Count - 1; n > -1; n--)
                {
                    var chat = chatMessageList[n];
                    chat.Update(Time.deltaTime);

                    if (chat.pastTime > ChatMessage.pastTimeLimit && !Opened)
                    {
                        chat.gameObject.SetActive(false);
                    }
                }
            }
        }

        static public void Open(object obj, bool mute)
        {
            Opened = true;

            scroll.SetActive(true);
            simple.SetActive(false);

            if (chatMessageList != null)
            {
                foreach (var chat in chatMessageList)
                {
                    chat.gameObject.SetActive(true);
                    chat.gameObject.transform.SetParent(scrollContent.transform);
                }
            }
        }

        static public void Close(object obj, bool mute)
        {
            Opened = false;

            scroll.SetActive(false);
            simple.SetActive(true);

            if (chatMessageList != null)
            {
                foreach(var chat in chatMessageList)
                {
                    if (chat.pastTime > ChatMessage.pastTimeLimit) 
                    {
                        chat.gameObject.SetActive(false); 
                    }

                    else
                    {
                        chat.gameObject.SetActive(true);
                    }

                    chat.gameObject.transform.SetParent(simple.transform);
                }
            }
        }

        class ChatMessage
        {
            static public readonly float pastTimeLimit = 3.0f;

            public GameObject gameObject;
            public float pastTime;

            public ChatMessage(GameObject gameObject)
            {
                this.gameObject = gameObject;
                pastTime = 0.0f;
            }

            public void Update(float dt)
            {
                pastTime += dt;
            }
        }
    }
}

