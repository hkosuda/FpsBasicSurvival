using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMain : EnemyMain
{
    private void Awake()
    {
        var _hp = Floats.Get(Floats.Item.turret_hp_default);
        var rate = Floats.Get(Floats.Item.turret_hp_increase_rate);

        HP = _hp * (1.0f + rate * SV_RoundAdmin.RoundNumber);

        var interactive = gameObject.GetComponent<InteractiveObject>();
        interactive.SetOnShotReaction(OnShot);

        EnemyType = EnemyType.turret;
    }

    void OnShot()
    {
        var damageRate = Utility.SafetyDivision((float)SV_StatusAdmin.CurrentDamageRate, (float)SV_StatusAdmin.DefaultDamageRate, 1.0f);
        var damage = Floats.Get(Floats.Item.ak_damage) * damageRate;

        HP -= damage;
        EnemyDamageTaken(null, this);

        if (HP <= 0.0f)
        {
            KilledBy = Killer.player;
            EnemyDestroyed?.Invoke(this, this);
            Destroy(gameObject);
        }
    }
}
