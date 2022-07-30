using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class SvDifficultyButton : MonoBehaviour
    {
        static readonly Dictionary<Difficulty, Color> buttonColor = new Dictionary<Difficulty, Color>()
        {
            { Difficulty.ez, new Color(0.1f, 0.4f, 0.1f, 1.0f) },
            { Difficulty.normal, new Color(0.4f, 0.4f, 0.1f, 1.0f) },
            { Difficulty.hard, new Color(0.4f, 0.25f, 0.1f, 1.0f) },
            { Difficulty.inferno, new Color(0.4f, 0.1f, 0.1f, 1.0f) },
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
                var current = (int)SvParams.CurrentDifficulty;
                var next = (Difficulty)((current + 1) % 4);

                SvParams.SwitchDifficulty(next);
                UpdateContent();
            }
        }

        static void UpdateContent()
        {
            var diff = SvParams.CurrentDifficulty;

            buttonText.text = "ìÔà’ìxÅF" + diff.ToString().ToLower();
            buttonBody.GetComponent<MeshRenderer>().material.SetColor("_MainColor", buttonColor[diff]);
        }
    }
}

