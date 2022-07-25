using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class SvSeedButton : MonoBehaviour
    {
        static readonly Dictionary<bool, Color> buttonColor = new Dictionary<bool, Color>()
        {
            { true, new Color(0.5f, 0.5f, 0.5f, 1.0f) },
            { false, new Color(0.4f, 0.6f, 0.7f, 1.0f) }
        };

        static GameObject buttonBody;
        static TextMeshProUGUI buttonText;

        private void Awake()
        {
            buttonBody = gameObject.transform.GetChild(0).gameObject;
            buttonText = gameObject.transform.GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            UpdateContent();
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
                WeaponController.ShootingHit += SwitchDifficulty;
            }

            else
            {
                WeaponController.ShootingHit -= SwitchDifficulty;
            }
        }

        static void SwitchDifficulty(object obj, RaycastHit hit)
        {
            if (hit.collider.gameObject == buttonBody)
            {
                SV_Seed.SwitchSeedMode(!SV_Seed.FixedSeed);
                UpdateContent();
            }
        }

        static void UpdateContent()
        {
            if (SV_Seed.FixedSeed)
            {
                buttonText.text = "シード：固定";
                buttonBody.GetComponent<MeshRenderer>().material.SetColor("_MainColor", buttonColor[SV_Seed.FixedSeed]);
            }

            else
            {
                buttonText.text = "シード：ランダム";
                buttonBody.GetComponent<MeshRenderer>().material.SetColor("_MainColor", buttonColor[SV_Seed.FixedSeed]);
            }
        }
    }
}

