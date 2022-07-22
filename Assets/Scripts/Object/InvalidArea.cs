using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [RequireComponent(typeof(BoxCollider))]
    public class InvalidArea : MonoBehaviour
    {
        static public EventHandler<Vector3> CourseOut { get; set; }

        public bool removeMesh = true;

        private void Awake()
        {
            var collider = gameObject.GetComponent<BoxCollider>();
            collider.isTrigger = true;

            if (removeMesh)
            {
                Destroy(gameObject.GetComponent<MeshRenderer>());
                Destroy(gameObject.GetComponent<MeshFilter>());
            }

            // ignore laycast
            gameObject.layer = 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (TimerSystem.Paused) { return; }

            var position = Player.Myself.transform.position;
            CourseOut?.Invoke(null, position);
        }
    }
}

