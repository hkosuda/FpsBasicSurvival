using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class PM_Jumping : Controller
    {
        // constants
        static readonly int jumpingFrameBuffer = 10;

        // valiables
        static public int JumpingFrameBufferRemain { get; private set; }
        static public bool JumpingBegin { get; private set; }
        static public bool AutoJump { get; set; }

        public override void Initialize()
        {
            SetEvent(1);
        }

        public override void Shutdown()
        {
            SetEvent(-1);
        }

        static void SetEvent(int indicator)
        {

        }

        public override void Update(float dt)
        {
            //if (Keyconfig.CheckInput(Keyconfig.KeyAction.autoJump, true)) { AutoJump = !AutoJump; }

            if (PM_Landing.LandingIndicator < 0)
            {
                JumpingFrameBufferRemain--;
                if (JumpingFrameBufferRemain < 0) { JumpingFrameBufferRemain = 0; }

                return;
            }

            if (JumpingFrameBufferRemain <= 0)
            {
                //if (Keyconfig.CheckInput(Keyconfig.KeyAction.jump, true) || AutoJump)
                if (Keyconfig.CheckInput(KeyAction.jump, true) || Keyconfig.CheckInput(KeyAction.autoJump, false))
                {
                    JumpingFrameBufferRemain = jumpingFrameBuffer;
                    //JumpingBegin = true;

                    var v = Player.Rb.velocity;
                    Player.Rb.velocity = new Vector3(v.x, Params.pm_jumping_velocity, v.z);
                }
            }
        }

        public override void FixedUpdate(float dt)
        {
            if (PM_Landing.LandingIndicator < 0)
            {
                JumpingFrameBufferRemain--;
                if (JumpingFrameBufferRemain < 0) { JumpingFrameBufferRemain = 0; }

                return;
            }

            if (JumpingFrameBufferRemain <= 0)
            {
                //if (Keyconfig.CheckInput(Keyconfig.KeyAction.jump, true) || AutoJump)
                if (Keyconfig.CheckInput(KeyAction.jump, true) || Keyconfig.CheckInput(KeyAction.autoJump, false))
                {
                    JumpingFrameBufferRemain = jumpingFrameBuffer;
                    //JumpingBegin = true;

                    var v = Player.Rb.velocity;
                    Player.Rb.velocity = new Vector3(v.x, Params.pm_jumping_velocity, v.z);
                }
            }
        }

        public override void LateUpdate()
        {
            JumpingBegin = false;
        }

        static void InactivateAutoJumpOnCourseOut(object obj, Vector3 position)
        {
            InactivateAutoJump();
        }

        static public void InactivateAutoJump()
        {
            AutoJump = false;
        }
    }
}

