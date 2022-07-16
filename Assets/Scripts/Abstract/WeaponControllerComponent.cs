using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public abstract class WeaponControllerComponent
    {
        public virtual void Initialize()
        {
            return;
        }

        public virtual void Shutdown()
        {
            return;
        }

        public virtual void Activate()
        {
            return;
        }

        public virtual void Inactivate()
        {
            return;
        }

        public virtual void Update(float dt)
        {
            return;
        }

        public virtual void LateUpdate()
        {
            return;
        }

        public virtual void FixedUpdate(float dt)
        {
            return;
        }
    }
}

