using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class DamageEffect : MonoBehaviour
    {
        static readonly float maxTime = 0.8f;
        static readonly float maxWidth = 100.0f;

        static GameObject image;
        static Material material;

        static float effectTime;

        private void Awake()
        {
            image = gameObject.transform.GetChild(0).gameObject;
            material = image.GetComponent<Image>().material;
        }

        private void Start()
        {
            SetEvent(1);
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        static void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                SV_Status.PlayerDamageTaken += BeginEffect;
                TimerSystem.Updated += UpdateMethod;
            }

            else
            {
                SV_Status.PlayerDamageTaken -= BeginEffect;
                TimerSystem.Updated -= UpdateMethod;
            }
        }

        static void BeginEffect(object obj, int[] damage)
        {
            var damageRate = Calcf.Clip(0.0f, 1.0f, Calcf.SafetyDiv(damage[0], MineDamage(), 1.0f));
            damageRate = Mathf.Pow(damageRate, 0.5f);

            effectTime = maxTime * damageRate;

            // - inner function
            static int MineDamage()
            {
                var def = SvParams.Get(SvParam.mine_damage);
                var inc = SvParams.Get(SvParam.mine_damage_increase);

                return Mathf.RoundToInt(def * (1.0f + inc * SV_Round.RoundNumber));
            }
        }

        static void UpdateMethod(object obj, float dt)
        {
            effectTime -= dt;

            if (effectTime < 0.0f)
            {
                effectTime = -1.0f;
                image.SetActive(false);
            }

            else
            {
                var timeRate = effectTime / maxTime;

                var width = Calcf.Clip(1.0f, maxWidth, maxWidth * timeRate);
                var alpha = timeRate;

                image.SetActive(true);
                material.SetFloat("_Width", width);
                material.SetFloat("_Alpha", alpha);
            }
        }
    }
}

