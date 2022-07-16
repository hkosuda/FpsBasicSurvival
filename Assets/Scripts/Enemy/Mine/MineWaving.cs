using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class MineWaving : MonoBehaviour
    {
        static readonly float mineHeight = 0.8f;
        static readonly float wavingAmp = 0.3f;
        static readonly float wavingSpeed = 0.5f;

        BoxCollider boxCollider;
        GameObject body;
        float theta;

        private void Awake()
        {
            boxCollider = gameObject.GetComponent<BoxCollider>();
            body = gameObject.transform.GetChild(0).gameObject;
        }

        void Start()
        {
            SetPhase();
            SetEvent(1);

            // - inner function
            void SetPhase()
            {
                var id = gameObject.GetComponent<MineBrain>().ID;
                SeedSystem.SetSeed(id);
                theta = UnityEngine.Random.Range(0.0f, Mathf.PI);
            }
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                TimerSystem.Updated += UpdateMethod;
            }

            else
            {
                TimerSystem.Updated -= UpdateMethod;
            }
        }

        // Update is called once per frame
        void UpdateMethod(object obj, float dt)
        {
            theta += 2.0f * Mathf.PI * dt * wavingSpeed;
            theta %= 2.0f * Mathf.PI;

            var height = mineHeight + wavingAmp * Mathf.Sin(theta);

            body.transform.localPosition = new Vector3(0.0f, height, 0.0f);
            boxCollider.center = new Vector3(0.0f, height, 0.0f);
        }
    }
}

