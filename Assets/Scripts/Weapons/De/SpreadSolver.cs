using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SpreadSolver
    {
        static readonly int seed = 3000;

        static public Vector3 CalcSpread(SpreadParam spreadParam)
        {
            var radRotX = -Player.Camera.transform.eulerAngles.x * Mathf.Deg2Rad;
            var radRotY = Player.Camera.transform.eulerAngles.y * Mathf.Deg2Rad;

            var v = new Vector2(Player.Rb.velocity.x, Player.Rb.velocity.z).magnitude;
            var vRate = Calcf.SafetyDiv(v, Params.pm_max_speed_on_ground, 0.0f);

            var isJumping = (PM_Landing.LandingIndicator <= 0) || PM_Jumping.JumpingBegin;

            return _CalcSpread(radRotX, radRotY, vRate, isJumping, spreadParam);
        }

        static public Vector3 _CalcSpread(float radRotX, float radRotY, float vRate, bool isJumping, SpreadParam spreadParam)
        {
            InitState(spreadParam);

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

            // - inner function
            static void InitState(SpreadParam spreadParam)
            {
                if (spreadParam.spreadPattern == null)
                {
                    UnityEngine.Random.InitState(seed + DateTime.Now.Millisecond);
                }

                else
                {
                    UnityEngine.Random.InitState(seed + spreadParam.shootingCounter);
                }
            }

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
                var qr_spread = GetEllipseRandomSpread(spreadParam.h_random, spreadParam.v_random, rate, spreadParam, true);

                q += GetOffset(spreadParam) + Mathf.Abs(qr_spread[0]) * FixedPattern(spreadParam);
                r += qr_spread[1];

                spreadParam.prevRandomQ = q;
                spreadParam.prevPotential = spreadParam.potential;

                return new float[3] { p, q, r };

                // - inner function
                static float GetOffset(SpreadParam spreadParam)
                {
                    if (spreadParam.spreadPattern == null)
                    {
                        return 0.0f;
                    }

                    else
                    {
                        var offsetQ = spreadParam.prevRandomQ * Calcf.SafetyDiv(spreadParam.potential, spreadParam.prevPotential, 0.0f);
                        var limit = 2 * spreadParam.h_random;

                        if (offsetQ > limit) { offsetQ = limit; }
                        if (offsetQ < -limit) { offsetQ = -limit; }

                        return offsetQ;
                    }
                }
            }

            static float[] CalcJumpingRunningSpread(float[] pqr_vector, float vRate, bool isJumping, SpreadParam spreadParam)
            {
                UnityEngine.Random.InitState(DateTime.Now.Millisecond);

                var p = pqr_vector[0];
                var q = pqr_vector[1];
                var r = pqr_vector[2];

                if (vRate > 1.0f) { vRate = 1.0f; }
                vRate = Mathf.Pow(vRate, spreadParam.runningExpo);

                // jumping
                var jRate = 0.0f;
                if (isJumping) { jRate = 1.0f; }

                var rate = vRate + jRate;

                var qr_spread = GetEllipseRandomSpread(spreadParam.h_running, spreadParam.v_running, rate, spreadParam, false);

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
            static float[] GetEllipseRandomSpread(float h_max, float v_max, float rate, SpreadParam spreadParam, bool pattern)
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

            // - inner function
            static float FixedPattern(SpreadParam spreadParam)
            {
                if (spreadParam.spreadPattern == null)
                {
                    return RandomDirection();
                }

                else
                {
                    return CalcPattern(spreadParam.spreadPattern, spreadParam.shootingCounter);
                }

                // - - - inner function
                static float CalcPattern(List<float> pattern, int count)
                {
                    if (count < pattern.Count)
                    {
                        return pattern[count];
                    }

                    else
                    {
                        var over = count - pattern.Count;
                        var quo = Calcf.QuoRem(over, 8)[0];

                        var dir = -pattern.Last();

                        for (var n = 0; n < quo; n++)
                        {
                            dir *= -1.0f;
                        }

                        return dir;
                    }
                }
            }
        }
    }

    public class SpreadParam
    {
        public int shootingCounter = 0;

        public float potential;
        public float interval;

        public float prevRandomQ;
        public float prevPotential;

        public readonly float maxPotential;
        public readonly float potentialIncrease;
        public readonly float shootingInterval;
        public readonly float resetTime;

        public readonly float lifting;
        public readonly float h_random;
        public readonly float v_random;
        public readonly float h_running;
        public readonly float v_running;

        public readonly float liftingExpo;
        public readonly float randomExpo;
        public readonly float runningExpo;

        public readonly List<float> spreadPattern;

        public SpreadParam(float maxPotential, float potentialIncrease, float shootingInterval, float resetTime,
            float lifting, float h_random, float v_random, float h_running, float v_running,
            float liftingExpo, float randomExpo, float runningExpo, List<float> spreadPattern = null)
        {
            this.maxPotential = maxPotential;
            this.potentialIncrease = potentialIncrease;
            this.shootingInterval = shootingInterval;
            this.resetTime = resetTime;

            this.lifting = lifting;
            this.h_random = h_random;
            this.v_random = v_random;
            this.h_running = h_running;
            this.v_running = v_running;

            this.liftingExpo = liftingExpo;
            this.randomExpo = randomExpo;
            this.runningExpo = runningExpo;

            this.spreadPattern = spreadPattern;
        }

        public void IncreasePotential()
        {
            interval = 0.0f;

            potential += shootingInterval + potentialIncrease;
            shootingCounter++;

            if (potential > maxPotential) 
            {
                potential = maxPotential;
            }
        }

        public void DecreasePotential(float dt)
        {
            interval += dt;
            potential -= dt;

            if (potential < 0.0f)
            {
                potential = 0.0f;
                shootingCounter = 0;
            }

            if (interval > resetTime)
            {
                shootingCounter = 0;
            }
        }
    }
}

