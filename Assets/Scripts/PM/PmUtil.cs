using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class PmUtil
    {
        // constants
        static public readonly float _maxSpeedOnTheGround = 7.7f;
        static public readonly float _maxSpeedInTheAir = 1.4f;
        static public readonly float _accelOnTheGround = 65.0f;
        static public readonly float _accelInTheAir = 5000.0f;
        static public readonly float _draggingAccel = 45.0f;

        static public readonly float _crouchingMaxSpeed = 3.5f;
        static public readonly float _crouchingAccel = 60.0f;
        static public readonly float _crouchingDraggingAccel = 55.0f;

        static public Vector2 AddVector { get; private set; }
        static public Vector2 NextVector { get; private set; }

        static public Vector2 CalcVector(Vector2 inputVector, Vector2 currentVector, float dt, bool onground, bool crouching)
        {
            // load settings 
            var draggingAccel = _draggingAccel;
            var maxSpeed = _maxSpeedOnTheGround;
            var accel = _accelOnTheGround;

            if (onground && crouching)
            {
                maxSpeed = _crouchingMaxSpeed;
                accel = _crouchingAccel;
                draggingAccel = _crouchingDraggingAccel;
            }

            if (!onground)
            {
                draggingAccel = 0.0f;
                maxSpeed = _maxSpeedInTheAir;
                accel = _accelInTheAir;
            }

            var normalizedInputVector = inputVector.normalized;

            var magnitudeOfFriction = Clip(currentVector.magnitude, 0.0f, draggingAccel * dt);

            var frictionVector = currentVector.normalized * (-magnitudeOfFriction);

            var playerVector_fric = currentVector + frictionVector;

            var magnitudeOfProjection = Vector2.Dot(playerVector_fric, normalizedInputVector);

            var magnitudeOfAddVector = Clip(maxSpeed - magnitudeOfProjection, 0.0f, accel * dt);

            var addVector = normalizedInputVector * magnitudeOfAddVector;

            var nextPlayerVector = playerVector_fric + addVector;

            AddVector = addVector;
            NextVector = nextPlayerVector;

            return nextPlayerVector;

            // - inner function
            static float Clip(float val, float minVal, float maxVal)
            {
                if (val < minVal) { return minVal; }
                if (val > maxVal) { return maxVal; }
                return val;
            }
        }
    }
}