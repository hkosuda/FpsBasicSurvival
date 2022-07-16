using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Player : MonoBehaviour
    {
        static public readonly float playerRadius = 0.5f;
        static public readonly float centerY = 0.9f;

        static public GameObject Myself { get; private set; }

        static public GameObject Camera { get; private set; }

        static public Rigidbody Rb { get; private set; }

        private void Awake()
        {
            Myself = gameObject;
            Camera = GameObject.FindWithTag("MainCamera");

            Rb = gameObject.GetComponent<Rigidbody>();
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
