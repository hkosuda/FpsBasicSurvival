using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{

    public class AkController : WeaponController
    {
        static public float Potential { get; private set; }
        static public float MaxPotential { get; private set; }
        static public float FiringCooldownRemain { get; private set; }

        static public int AmmoInMag { get; set; } = 30;
        static public int AmmoInBag { get; set; } = 300;

        static public int MaxAmmoInMag { get; set; } = 30;
        static public int MaxAmmoInBag { get; set; } = 300;

        static public int SpreadRandomSeed { get; private set; }

        static float q_previous;

        private void Awake()
        {
            SetEvent(1);

            FiringCooldownRemain = 0.0f;
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                TimerSystem.Updated += UpdateMethod;
                AkAnimator.ReloadingEnd += Reload;
            }

            else
            {
                TimerSystem.Updated -= UpdateMethod;
                AkAnimator.ReloadingEnd -= Reload;
            }
        }

        static void UpdateMethod(object obj, float dt)
        {
            // update potentials
            Potential -= dt;
            if (Potential < 0.0f) { Potential = 0.0f; SpreadRandomSeed = 0; }

            // update firing cooldown
            FiringCooldownRemain = Calcf.Clip(FiringCooldownRemain - dt, 0.0f, Params.ak_firing_interval);

            //
            // response to input

            if (Keyconfig.CheckInput(Keyconfig.KeyAction.shot, true))
            {
                if (CheckEmpty()) { return; }
            }

            if (Keyconfig.CheckInput(Keyconfig.KeyAction.shot, false))
            {
                FullAuto();
                return;
            }

            if (Keyconfig.CheckInput(Keyconfig.KeyAction.reload, true))
            {
                if (CheckReloading()) { return; }
            }
        }

        static void FullAuto()
        {
            if (!WeaponManager.Active) { return; }
            if (!CheckShootability()) { return; }

            var potentialRate = Calcf.Clip(Calcf.SafetyDiv(Potential, MaxPotential, 0.0f), 0.0f, 1.0f);
            var velocityRate = PM_PlaneVector.PlaneVector.magnitude / Params.pm_max_speed_on_the_gound;

            var param = GetParams(potentialRate, velocityRate);
            var pqr_vector = WeaponUtil.GetPQR(param);

            q_previous = pqr_vector[1];
            Shot?.Invoke(null, WeaponUtil.PQR2Vec3(pqr_vector, Player.Camera.transform));

            var hit = WeaponUtil.Shot(pqr_vector, PlayerViewController.Self.transform);

            IncrementPotential();
            UpdateStatus();

            Shot_Hit?.Invoke(null, hit);
            Shot_Potential?.Invoke(null, potentialRate);

            if (hit.collider != null)
            {
                ShootingHit?.Invoke(null, hit.collider.gameObject);
            }


            //
            // - inner function
            static bool CheckShootability()
            {
                if (FiringCooldownRemain > 0.0f) { return false; }
                if (AmmoInMag < 1) { return false; }
                return true;
            }

            //
            // - iner function
            static WeaponUtil.SpreadParam GetParams(float potentialRate, float velocityRate)
            {
                return new WeaponUtil.SpreadParam(Weapon.akm, q_previous, potentialRate, velocityRate, SpreadRandomSeed, PlayerController.IsJumping);
            }

            //
            // - inner function
            static void IncrementPotential()
            {
                MaxPotential = Floats.Get(Floats.Item.ak_potential_cooldown_time);

                var incrementRate = Floats.Get(Floats.Item.ak_potential_increment_rate);
                var firingInterval = Floats.Get(Floats.Item.ak_firing_interval);

                var increment = MaxPotential * incrementRate;

                Potential = Utility.Clip(Potential + firingInterval + increment, 0.0f, MaxPotential + firingInterval);
            }

            //
            // - inner function
            static void UpdateStatus()
            {
                FiringCooldownRemain = Floats.Get(Floats.Item.ak_firing_interval);
                AmmoInMag--;
                SpreadRandomSeed++;
            }
        }

        static bool CheckEmpty()
        {
            if (!WeaponManager.Active) { return false; }

            if (AmmoInMag <= 0)
            {
                Empty?.Invoke(null, false);
                return true;
            }

            return false;
        }

        static bool CheckReloading()
        {
            if (!WeaponManager.Active) { return false; }
            if (AmmoInBag == 0) { return false; }

            var max_ammo_in_mag = MaxAmmoInMag;

            if (AmmoInMag < max_ammo_in_mag)
            {
                ReloadingBegin?.Invoke(null, false);
                return true;
            }

            return false;
        }

        static void Reload(object obj, bool mute)
        {
            var max_ammo_in_mag = MaxAmmoInMag;

            if (AmmoInMag >= max_ammo_in_mag)
            {
                return;
            }

            var requireAmmo = max_ammo_in_mag - AmmoInMag;

            if (requireAmmo <= AmmoInBag)
            {
                AmmoInBag -= requireAmmo;
                AmmoInMag += requireAmmo;
            }

            else
            {
                AmmoInMag += AmmoInBag;
                AmmoInBag = 0;
            }
        }

        static public bool ReplonishAmmo(int additional)
        {
            var totalMaxAmmo = MaxAmmoInMag + MaxAmmoInBag;
            var totalAmmo = AmmoInMag + AmmoInBag;

            if (totalAmmo >= totalMaxAmmo) { return false; }

            var nextTotalAmmo = totalAmmo + additional;

            if (nextTotalAmmo < MaxAmmoInMag)
            {
                AmmoInMag = nextTotalAmmo;
                AmmoInBag = 0;
                return true;
            }

            else
            {
                AmmoInMag = MaxAmmoInMag;
            }

            var nextTotalAmmoRemain = nextTotalAmmo - MaxAmmoInMag;

            if (nextTotalAmmoRemain < MaxAmmoInBag)
            {
                AmmoInBag = nextTotalAmmoRemain;
            }

            else
            {
                AmmoInBag = MaxAmmoInBag;
            }

            return true;
        }
    }
}

