using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class M9_Availability : WeaponControllerComponent
    {
        static public EventHandler<bool> InspectBegin { get; set; }

        static public readonly float preparingTime = 0.05f;
        static public readonly float animationTime = 5.58f;

        static float preparingTimeRemain;
        static float animationTimeRemain;

        public override void Activate()
        {
            preparingTimeRemain = preparingTime;
            animationTimeRemain = 0.0f;
        }

        public override void Inactivate()
        {

        }

        public override void Update(float dt)
        {
            preparingTimeRemain -= dt;
            if (preparingTimeRemain < 0.0f) { preparingTimeRemain = -1.0f; }

            animationTimeRemain -= dt;
            if (animationTimeRemain < 0.0f) { animationTimeRemain = -1.0f; }

            
            if (Keyconfig.CheckInput(KeyAction.shot, false))
            {
                if (preparingTimeRemain < 0.0f && animationTimeRemain < 0.0f)
                {
                    animationTimeRemain = animationTime;
                    InspectBegin?.Invoke(null, false);
                }
            }
        }
    }
}