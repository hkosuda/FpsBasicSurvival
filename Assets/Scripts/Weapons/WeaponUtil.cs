using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class WeaponUtil
{
    public class SpreadParam
    {
        // seed
        public int basicSeed;
        public int randomSeed;

        // previous q value
        public float q_previous;

        // status
        public bool isJumping;

        // rate
        public float potentialRate;
        public float velocityRate;

        // spread rate
        public float spreadRate;
        public float minSpreadRate;

        // each elements spread rate
        public float liftingSpreadRate;
        public float randomSpreadRate;
        public float runningSpreadRate;
        public float jumpingSpreadRate;

        // intensity
        public float liftingSpreadIntensity;
        public float h_randomSpreadIntensity;
        public float v_randomSpreadIntensity;
        public float h_runningSpreadIntensity;
        public float v_runningSpreadIntensity;

        // exponentials
        public float liftingSpreadExpo;
        public float randomSpreadExpo;

        // random spread horizontal notation
        public List<int> randomSpreadHorizontalPattern;

        public SpreadParam(Weapon weapon, float q_previous, float potentialRate, float velocityRate, int randomSeed, bool isJumping)
        {
            this.q_previous = q_previous;

            this.potentialRate = potentialRate;
            this.velocityRate = velocityRate;

            this.randomSeed = randomSeed;
            this.isJumping = isJumping;

            if (weapon == Weapon.akm)
            {
                basicSeed = Ints.Get(Ints.Item.ak_spread_seed);

                spreadRate = Floats.Get(Floats.Item.ak_spread_rate);
                liftingSpreadRate = Floats.Get(Floats.Item.ak_lifting_spread_rate);
                randomSpreadRate = Floats.Get(Floats.Item.ak_random_spread_rate);
                runningSpreadRate = Floats.Get(Floats.Item.ak_running_spread_rate);

                liftingSpreadExpo = Floats.Get(Floats.Item.ak_lifting_spread_expo);
                randomSpreadExpo = Floats.Get(Floats.Item.ak_random_spread_expo);

                liftingSpreadIntensity = Floats.Get(Floats.Item.ak_lifting_intensity);

                h_randomSpreadIntensity = Floats.Get(Floats.Item.ak_random_h_intensity);
                v_randomSpreadIntensity = Floats.Get(Floats.Item.ak_random_v_intensity);

                h_runningSpreadIntensity = Floats.Get(Floats.Item.ak_running_h_intensity);
                v_runningSpreadIntensity = Floats.Get(Floats.Item.ak_running_v_intensity);

                minSpreadRate = Floats.Get(Floats.Item.ak_min_spread_rate);

                randomSpreadHorizontalPattern = new List<int>() { 1, 3, 6, 10, 15, 21, 28 };

                if (PlayerController.IsCrouching)
                {
                    spreadRate *= Floats.Get(Floats.Item.ak_crouching_spread_rate);
                }

                return;
            }

            if (weapon == Weapon.deagle)
            {
                basicSeed = Ints.Get(Ints.Item.de_spread_seed);

                spreadRate = Floats.Get(Floats.Item.de_spread_rate);
                liftingSpreadRate = Floats.Get(Floats.Item.de_lifting_spread_rate);
                randomSpreadRate = Floats.Get(Floats.Item.de_random_spread_rate);
                runningSpreadRate = Floats.Get(Floats.Item.de_running_spread_rate);

                liftingSpreadExpo = Floats.Get(Floats.Item.de_lifting_spread_expo);
                randomSpreadExpo = Floats.Get(Floats.Item.de_random_spread_expo);

                liftingSpreadIntensity = Floats.Get(Floats.Item.de_lifting_intensity);

                h_randomSpreadIntensity = Floats.Get(Floats.Item.de_random_h_intensity);
                v_randomSpreadIntensity = Floats.Get(Floats.Item.de_random_v_intensity);

                h_runningSpreadIntensity = Floats.Get(Floats.Item.de_running_h_intensity);
                v_runningSpreadIntensity = Floats.Get(Floats.Item.de_running_v_intensity);

                minSpreadRate = Floats.Get(Floats.Item.de_min_spread_rate);

                randomSpreadHorizontalPattern = new List<int>();

                if (PlayerController.IsCrouching)
                {
                    spreadRate *= Floats.Get(Floats.Item.de_crouching_spread_rate);
                }

                return;
            }
        }
    }

    static public float[] GetPQR(SpreadParam param)
    {
        var q = param.q_previous * param.potentialRate;

        float[] pqr_vector = new float[3] { 10.0f, q, 0.0f };

        SeedManager.SetSeed(param.basicSeed, param.randomSeed);

        pqr_vector = CalcLifting(pqr_vector, param);
        pqr_vector = CalcRandomSpread(pqr_vector, param);
        pqr_vector = CalcJumpingRunningSpread(pqr_vector, param);

        return pqr_vector;

        // - inner functions
        static float[] CalcLifting(float[] pqr_vector, SpreadParam param)
        {
            var p = pqr_vector[0];
            var q = pqr_vector[1];
            var r = pqr_vector[2];

            var lifting = param.liftingSpreadIntensity * Mathf.Pow(param.potentialRate, param.liftingSpreadExpo) * param.spreadRate * param.liftingSpreadRate;

            r += lifting;

            return new float[3] { p, q, r };
        }

        static float[] CalcRandomSpread(float[] pqr_vector, SpreadParam param)
        {
            var p = pqr_vector[0];
            var q = pqr_vector[1];
            var r = pqr_vector[2];

            var rate = Mathf.Pow(param.potentialRate, param.randomSpreadExpo) * param.spreadRate * param.randomSpreadRate;

            var h_max = param.h_randomSpreadIntensity;
            var v_max = param.v_randomSpreadIntensity;

            var qr_spread = GetEllipseRandomSpread(param, h_max, v_max, rate);

            // add spread
            q += qr_spread[0];
            r += qr_spread[1];

            return new float[3] { p, q, r };
        }

        // - inner function
        static float[] CalcJumpingRunningSpread(float[] pqr_vector, SpreadParam param)
        {
            var p = pqr_vector[0];
            var q = pqr_vector[1];
            var r = pqr_vector[2];

            var rate = GetRate(param);

            var h_max = param.h_runningSpreadIntensity;
            var v_max = param.v_runningSpreadIntensity;

            var qr_spread = GetEllipseRunningSpread(h_max, v_max, rate);

            // <add spread>
            q += qr_spread[0];
            r += qr_spread[1];
            // </add spread>

            return new float[3] { p, q, r };

            // - - inner function
            static float GetRate(SpreadParam param)
            {
                float v_rate;
                float j_rate;

                // calc running spread rate
                v_rate = Utility.Clip(param.velocityRate, 0.0f, 1.0f);
                v_rate *= param.runningSpreadRate;

                // jumping spread ratio
                if (param.isJumping)
                {
                    j_rate = param.jumpingSpreadRate;
                }

                else
                {
                    j_rate = 0.0f;
                }

                return (j_rate + v_rate) * param.spreadRate;
            }
        }

        // - inner function
        static float[] GetEllipseRunningSpread(float h_max, float v_max, float rate)
        {
            SeedManager.SetSeed(-1);

            h_max *= rate;
            v_max *= rate;
            if (h_max == 0.0f) { return new float[2] { 0.0f, 0.0f }; }

            var qq = UnityEngine.Random.Range(-h_max, h_max);

            var rr_max = v_max * Mathf.Sqrt(1.0f - Mathf.Pow(qq / h_max, 2.0f));
            var rr = UnityEngine.Random.Range(-rr_max, rr_max);

            return new float[2] { qq, rr };
        }

        // - inner function
        static float[] GetEllipseRandomSpread(SpreadParam param, float h_max, float v_max, float rate)
        {
            h_max *= rate;
            v_max *= rate;
            if (h_max == 0.0f) { return new float[2] { 0.0f, 0.0f }; }

            var notation = GetNotation(param);

            float qq;

            if (notation > 0)
            {
                qq = UnityEngine.Random.Range(0, h_max);
            }

            else
            {
                qq = UnityEngine.Random.Range(-h_max, 0);
            }

            var rr_max = v_max * Mathf.Sqrt(1.0f - Mathf.Pow(qq / h_max, 2.0f));
            var rr = UnityEngine.Random.Range(-rr_max, rr_max);

            return new float[2] { qq, rr };

            // - inner function
            static float GetNotation(SpreadParam param)
            {
                // settings

                var interval = 10;

                // processing
                var subnotation = 1.0f;

                if (param.randomSpreadHorizontalPattern == null || param.randomSpreadHorizontalPattern.Count == 0)
                {
                    Debug.Log("Random");
                    SeedManager.SetSeed(-1);
                    var val = UnityEngine.Random.Range(-1.0f, 1.0f);
                    if (val > 0) { return 1.0f; }
                    return -1.0f;
                }

                if (param.randomSpreadHorizontalPattern.Count % 2 == 1)
                {
                    subnotation = -1.0f;
                }

                if (param.randomSeed > param.randomSpreadHorizontalPattern.Last())
                {
                    var rem = (param.randomSeed - param.randomSpreadHorizontalPattern.Last()) % (2 * interval);

                    if (rem < interval)
                    {
                        return subnotation;
                    }

                    else
                    {
                        return -subnotation;
                    }
                }

                for (var n = 0; n < param.randomSpreadHorizontalPattern.Count; n++)
                {
                    if (param.randomSpreadHorizontalPattern[n] >= param.randomSeed)
                    {
                        if (n % 2 == 0)
                        {
                            return 1.0f;
                        }

                        else
                        {
                            return -1.0f;
                        }
                    }
                }

                return 1.0f;
            }
        }
    }

    static public RaycastHit Shot(float[] pqr_vector, Transform cameraTransform)
    {
        var rotX = -cameraTransform.rotation.eulerAngles.x * Mathf.Deg2Rad;
        var rotY = cameraTransform.rotation.eulerAngles.y * Mathf.Deg2Rad;

        var zxy = PQR2ZXY(pqr_vector, rotX, rotY);

        var direction = new Vector3(zxy[1], zxy[2], zxy[0]);

        Physics.Raycast(origin: cameraTransform.position, direction: direction, out RaycastHit hit);

        return hit;

        // - inner function
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
    }

    static public Vector3 PQR2Vec3(float[] pqr, Transform cameraTransform)
    {
        var rotX = -cameraTransform.rotation.eulerAngles.x * Mathf.Deg2Rad;
        var rotY = cameraTransform.rotation.eulerAngles.y * Mathf.Deg2Rad;

        var p = pqr[0];
        var q = pqr[1];
        var r = pqr[2];

        var sX = Mathf.Sin(rotX);
        var cX = Mathf.Cos(rotX);
        var sY = Mathf.Sin(rotY);
        var cY = Mathf.Cos(rotY);

        var z = p * cX * cY - q * sY - r * sX * cY;
        var x = p * cX * sY + q * cY - r * sX * sY;
        var y = p * sX + r * cX;

        return new Vector3(x, y, z);
    }
}
