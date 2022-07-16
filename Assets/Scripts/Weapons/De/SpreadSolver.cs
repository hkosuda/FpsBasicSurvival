using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SpreadSolver
    {
        static public Vector3 CalcSpread(SpreadParam spreadParam)
        {
            var radRotX = Player.Camera.transform.eulerAngles.x;
            var radRotY = Player.Camera.transform.eulerAngles.y;

            var v = new Vector2(Player.Rb.velocity.x, Player.Rb.velocity.y).magnitude;
            var vRate = Calcf.SafetyDiv(v, Params.pm_max_speed_on_the_gound, 0.0f);

            var isJumping = (PM_Landing.LandingIndicator <= 0) || PM_Jumping.JumpingBegin;

            return _CalcSpread(radRotX, radRotY, vRate, isJumping, spreadParam);
        }

        static public Vector3 _CalcSpread(float radRotX, float radRotY, float vRate, bool isJumping, SpreadParam spreadParam)
        {
            var potentialRate = spreadParam.potential / spreadParam.maxPotential;
            if (potentialRate > 1.0f) { potentialRate = 1.0f; }

            var pqr_vector = new float[3] { 10.0f, 0.0f, 0.0f };

            pqr_vector = CalcLifting(pqr_vector, potentialRate, spreadParam);
            pqr_vector = CalcRandomSpread(pqr_vector, potentialRate, spreadParam);
            pqr_vector = CalcJumpingRunningSpread(pqr_vector, vRate, isJumping, spreadParam);

            var zxy_vector = PQR2ZXY(pqr_vector, radRotX, radRotY);

            var z = zxy_vector[0];
            var x = zxy_vector[1];
            var y = zxy_vector[2];

            return new Vector3(x, y, z);

            // - inner functions
            static float[] CalcLifting(float[] pqr_vector, float potentialRatio, SpreadParam spreadParam)
            {
                var p = pqr_vector[0];
                var q = pqr_vector[1];
                var r = pqr_vector[2];

                r += spreadParam.lifting * Mathf.Pow(potentialRatio, spreadParam.liftingExpo);

                return new float[3] { p, q, r };
            }

            // - inner function
            static float[] CalcRandomSpread(float[] pqr_vector, float potentialRatio, SpreadParam spreadParam)
            {
                var p = pqr_vector[0];
                var q = pqr_vector[1];
                var r = pqr_vector[2];

                var rate = Mathf.Pow(potentialRatio, spreadParam.randomExpo);
                var qr_spread = GetEllipseRandomSpread(spreadParam.h_random, spreadParam.v_random, 0.0f, rate);

                q += Mathf.Abs(qr_spread[0]) * RandomDirection();
                r += qr_spread[1];

                return new float[3] { p, q, r };


            }

            static float[] CalcJumpingRunningSpread(float[] pqr_vector, float vRate, bool isJumping, SpreadParam spreadParam)
            {
                var p = pqr_vector[0];
                var q = pqr_vector[1];
                var r = pqr_vector[2];

                // running
                var maxSpeed = Params.pm_max_speed_on_the_gound;
                var velocity = new Vector2(Player.Rb.velocity.x, Player.Rb.velocity.z).magnitude;

                if (vRate > 1.0f) { vRate = 1.0f; }
                vRate = Mathf.Pow(vRate, spreadParam.runningExpo);

                // jumping
                var jRate = 0.0f;
                if (isJumping) { jRate = 1.0f; }

                var rate = vRate + jRate;

                var qr_spread = GetEllipseRandomSpread(spreadParam.h_jumping, spreadParam.v_jumping, 0.0f, rate);

                q += qr_spread[0] * RandomDirection();
                r += qr_spread[1];

                return new float[3] { p, q, r };
            }

            // function
            static float[] PQR2ZXY(float[] pqr_vector, float radrotX, float radrotY)
            {
                var p = pqr_vector[0];
                var q = pqr_vector[1];
                var r = pqr_vector[2];

                var sX = Mathf.Sin(radrotX);
                var cX = Mathf.Cos(radrotX);
                var sY = Mathf.Sin(radrotY);
                var cY = Mathf.Cos(radrotY);

                var z = p * cX * cY - q * sY - r * sX * cY;
                var x = p * cX * sY + q * cY - r * sX * sY;
                var y = p * sX + r * cX;

                return new float[3] { z, x, y };
            }

            // - inner function
            static float[] GetEllipseRandomSpread(float h_max, float v_max, float v_min, float rate)
            {
                h_max *= rate;
                v_max *= rate;
                if (h_max == 0.0f) { return new float[2] { 0.0f, 0.0f }; }

                var qq = UnityEngine.Random.Range(-h_max, h_max);
                var rr_max = v_max * Mathf.Sqrt(1.0f - Mathf.Pow(qq / h_max, 2.0f));
                var rr = UnityEngine.Random.Range(-rr_max, rr_max);

                return new float[2] { qq, rr };
            }

            // - inner function
            static float RandomDirection()
            {
                var value = UnityEngine.Random.Range(0.0f, 1.0f);

                if (value > 0.5f) { return 1.0f; }
                return -1.0f;
            }
        }
    }

    public class SpreadParam
    {
        public int shootingCounter = 0;

        public float potential;

        public readonly float maxPotential;
        public readonly float potentialIncrease;
        public readonly float shootingInterval;

        public readonly float lifting;
        public readonly float h_random;
        public readonly float v_random;
        public readonly float h_jumping;
        public readonly float v_jumping;

        public readonly float liftingExpo;
        public readonly float randomExpo;
        public readonly float runningExpo;

        public readonly List<int> spreadPattern;

        public SpreadParam(float maxPotential, float potentialIncrease, float shootingInterval,
            float lifting, float h_random, float v_random, float h_jumping, float v_jumping,
            float liftingExpo, float randomExpo, float runningExpo, List<int> spreadPattern = null)
        {
            this.lifting = lifting;
            this.h_random = h_random;
            this.v_random = v_random;
            this.h_jumping = h_jumping;
            this.v_jumping = v_jumping;

            this.liftingExpo = liftingExpo;
            this.randomExpo = randomExpo;
            this.runningExpo = runningExpo;

            this.spreadPattern = spreadPattern;
        }

        public void IncreasePotential()
        {
            potential += shootingInterval + potentialIncrease;
            if (potential > maxPotential) { potential = maxPotential; }
        }

        public void DecreasePotential(float dt)
        {
            potential -= dt;
            if (potential < 0.0f) { potential = 0.0f; }
        }
    }
}

