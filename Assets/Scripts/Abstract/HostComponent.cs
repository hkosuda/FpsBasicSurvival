using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public abstract class HostComponent
    {
        public virtual void Initialize() { }
        public virtual void Shutdown() { }
        public virtual void Begin() { }
        public virtual void Stop() { }
    }
}

