using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class TimerSystem : MonoBehaviour
    {
        static public EventHandler<bool> TimerPaused { get; set; }
        static public EventHandler<bool> TimerResumed { get; set; }

        static public EventHandler<float> Updated { get; set; }
        static public EventHandler<bool> LateUpdated { get; set; }
        static public EventHandler<float> FixedUpdated { get; set; }

        static public bool Paused { get; protected set; }

        private void Start()
        {
            Paused = false;
        }

        static public void Pause()
        {
            var prev = Paused;

            Paused = true;
            Time.timeScale = 0.0f;

            if (prev != Paused) { TimerPaused?.Invoke(null, false); }
        }

        static public void Resume()
        {
            var prev = Paused;

            Paused = false;
            Time.timeScale = 1.0f;

            if (prev != Paused) { TimerResumed?.Invoke(null, false); }
        }

        private void Update()
        {
            if (Paused) { return; }
            Updated?.Invoke(null, Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (Paused) { return; }
            LateUpdated?.Invoke(null, false);
        }

        private void FixedUpdate()
        {
            if (Paused) { return; }
            FixedUpdated?.Invoke(null, Time.fixedDeltaTime);
        }
    }
}

