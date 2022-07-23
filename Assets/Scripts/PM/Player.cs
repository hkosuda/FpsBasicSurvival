using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Player : MonoBehaviour
    {
        static public EventHandler<float> Moved { get; set; }

        static public readonly float playerRadius = 0.5f;
        static public readonly float centerY = 0.9f;

        static public GameObject Myself { get; private set; }
        static public GameObject Camera { get; private set; }

        static public Rigidbody Rb { get; private set; }
        static public MeshCollider Collider { get; private set; }

        static Vector2 prevPos;
        static bool initialProcessing;

        private void Awake()
        {
            Myself = gameObject;
            Camera = GameObject.FindWithTag("MainCamera");

            Rb = gameObject.GetComponent<Rigidbody>();
            Collider = gameObject.GetComponent<MeshCollider>();
        }

        private void Start()
        {
            prevPos = new Vector2(Myself.transform.position.x, Myself.transform.position.z);
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
                TimerSystem.TimerResumed += ResetPrevPos;
            }

            else
            {
                TimerSystem.Updated -= UpdateMethod;
                TimerSystem.TimerResumed -= ResetPrevPos;
            }
        }

        static void UpdateMethod(object obj, float dt)
        {
            var pos = new Vector2(Myself.transform.position.x, Myself.transform.position.z);
            var delta = (pos - prevPos).magnitude;

            if (!initialProcessing && delta > 0.0f)
            {
                Moved?.Invoke(null, delta);
            }

            prevPos = pos;
            initialProcessing = false;
        }

        static void ResetPrevPos(object obj, bool mute)
        {
            prevPos = new Vector2(Myself.transform.position.x, Myself.transform.position.z);
        }

        static public float PlayerHeight()
        {
            var sizeY = Myself.transform.localScale.y;
            return centerY * sizeY;
        }

        static public void SetPosition(Vector3 position, Vector3 eulerAngle)
        {
            Myself.transform.position = position;
            Camera.transform.position = position + new Vector3(0.0f, CameraPosition.CameraOffset, 0.0f);

            PM_Camera.SetEulerAngles(eulerAngle);
            Rb.velocity = Vector3.zero;
        }
    }
}
