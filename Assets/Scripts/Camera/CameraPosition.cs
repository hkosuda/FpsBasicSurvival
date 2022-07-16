using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class CameraPosition : MonoBehaviour
    {
        static public float CameraOffset { get; private set; } = 0.45f;

        void LateUpdate()
        {
            gameObject.transform.position = Player.Rb.position + new Vector3(0.0f, CameraOffset, 0.0f);
        }
    }
}

