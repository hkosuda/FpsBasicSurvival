using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public enum TimerName
    {
        gameTimer,
        movieTimer,
        replayTimer,
    }

    public abstract class Timer
    {
        public TimerName Name { get; protected set; }

        public Timer(TimerName name)
        {
            Name = name;
        }

        public abstract void Pause();

        public abstract void Resume();

        public abstract void Initialize();

        public abstract void Shutdown();

        public abstract void Update(float dt);

        public abstract void LateUpdate();

        public abstract void FixedUpdate(float dt);
    }
}

