using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyGame
{
    public class DebugInfo : MonoBehaviour
    {
        TextMeshProUGUI text;

        void Start()
        {
            text = gameObject.GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            //text.text = PM_Landing.LandingIndicator.ToString();
            //text.text = PM_Landing.DeltaY.ToString("F8");
            text.text = new Vector2(Player.Rb.velocity.x, Player.Rb.velocity.z).magnitude.ToString("F3");
        }
    }
}

