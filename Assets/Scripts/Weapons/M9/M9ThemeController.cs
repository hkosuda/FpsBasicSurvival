using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class M9ThemeController : MonoBehaviour
    {
        static readonly float rotSpeed = 0.2f;

        [SerializeField] Material rubyMaterial;
        [SerializeField] Material sapphireMaterial;
        [SerializeField] Material emeraldMaterial;
        [SerializeField] Material violetMaterial;
        [SerializeField] Material steelMaterial;
        [SerializeField] Material blackMaterial;

        static Dictionary<M9Theme.Theme, Material> matList;

        static GameObject myself;

        static MeshRenderer renderer1;
        static MeshRenderer renderer2;

        static float rotY;

        static bool initialized = false;

        private void Awake()
        {
            myself = gameObject;

            renderer1 = GetRenderer(0);
            renderer2 = GetRenderer(1);

            matList = new Dictionary<M9Theme.Theme, Material>()
            {
                { M9Theme.Theme.ruby, rubyMaterial },
                { M9Theme.Theme.sapphire, sapphireMaterial },
                { M9Theme.Theme.emerald, emeraldMaterial },
                { M9Theme.Theme.violet, violetMaterial },
                { M9Theme.Theme.steel, steelMaterial },
                { M9Theme.Theme.black, blackMaterial },
            };

            // - inner function
            MeshRenderer GetRenderer(int n)
            {
                return gameObject.transform.GetChild(0).GetChild(n).gameObject.GetComponent<MeshRenderer>();
            }
        }

        void Start()
        {
            if (!initialized)
            {
                initialized = true;
                InitializeTheme();
            }

            UpdateRenderer();
            SetEvent(1);

            // - inner function
            static void InitializeTheme()
            {
                var now = DateTime.Now.Millisecond;
                var theme = (M9Theme.Theme)(now % M9Theme.nTheme);

                M9Theme.SwitchMaterial(theme);
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
                WeaponController.ShootingHit += SwitchMaterial;
                TimerSystem.Updated += UpdateMethod;
            }

            else
            {
                WeaponController.ShootingHit -= SwitchMaterial;
                TimerSystem.Updated -= UpdateMethod;
            }
        }

        static void SwitchMaterial(object obj, RaycastHit hit)
        {
            if (hit.collider.gameObject != myself) { return; }

            var currentIdx = (int)M9Theme.CurrentTheme;
            var nextIndex = (currentIdx + 1) % M9Theme.nTheme;

            var nextTheme = (M9Theme.Theme)nextIndex;
            M9Theme.SwitchMaterial(nextTheme);

            UpdateRenderer();
        }

        static void UpdateMethod(object obj, float dt)
        {
            rotY += dt * rotSpeed * 360.0f;
            rotY %= 360.0f;

            myself.transform.eulerAngles = new Vector3(0.0f, rotY, 0.0f);
        }

        static void UpdateRenderer()
        {
            if (renderer1 == null || renderer2 == null) { return; }
            if (matList == null) { return; }

            renderer1.material = matList[M9Theme.CurrentTheme];
            renderer2.material = matList[M9Theme.CurrentTheme];
        }
    }
}

