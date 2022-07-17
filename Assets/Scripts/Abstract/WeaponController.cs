using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public abstract class WeaponController
    {
        static public EventHandler<RaycastHit> ShootingHit { get; set; }
        static public EventHandler<Vector3> Shot { get; set; }

        static public EventHandler<bool> Empty { get; set; }
        static public EventHandler<bool> ReloadingBegin { get; set; }

        public Weapon Weapon { get; protected set; }

        protected List<WeaponControllerComponent> controllerList;

        public WeaponController(Weapon weapon)
        {
            Weapon = weapon;
        }

        public virtual void Initialize()
        {
            if (controllerList != null)
            {
                foreach(var controller in controllerList)
                {
                    controller.Initialize();
                }
            }
        }

        public virtual void Shutdown()
        {
            if (controllerList != null)
            {
                foreach (var controller in controllerList)
                {
                    controller.Shutdown();
                }
            }
        }

        public virtual void Activate()
        {
            if (controllerList != null)
            {
                foreach (var controller in controllerList)
                {
                    controller.Activate();
                }
            }
        }

        public virtual void Inactivate()
        {
            if (controllerList != null)
            {
                foreach (var controller in controllerList)
                {
                    controller.Inactivate();
                }
            }
        }

        public virtual void Update(float dt)
        {
            if (controllerList != null)
            {
                foreach (var controller in controllerList)
                {
                    controller.Update(dt);
                }
            }
        }

        public virtual void FixedUpdate(float dt)
        {
            if (controllerList != null)
            {
                foreach (var controller in controllerList)
                {
                    controller.FixedUpdate(dt);
                }
            }
        }
    }
}

