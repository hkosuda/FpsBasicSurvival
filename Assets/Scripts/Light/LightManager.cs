using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class LightManager : MonoBehaviour
    {
        [SerializeField] Light sun;

        static Light spotLight;
        static Light sunLight;

        private void Awake()
        {
            spotLight = gameObject.GetComponent<Light>();
            sunLight = sun;

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
                GameSystem.HostSwitched += UpdateEnvironment;
            }

            else
            {
                GameSystem.HostSwitched += UpdateEnvironment;
            }
        }

        static void UpdateEnvironment(object obj, bool mute)
        {
            

            var hostName = GameSystem.CurrentHost.HostName;

            if (hostName == HostName.survival)
            {
                spotLight.gameObject.SetActive(true);
                sunLight.gameObject.SetActive(false);

                RenderSettings.ambientIntensity = 0.0f;
                RenderSettings.reflectionIntensity = 0.5f;
            }

            else
            {
                spotLight.gameObject.SetActive(false);
                sunLight.gameObject.SetActive(true);

                RenderSettings.ambientIntensity = 1.0f;
                RenderSettings.reflectionIntensity = 1.0f;
            }
        }
    }
}

