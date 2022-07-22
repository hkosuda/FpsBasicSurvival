using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class M9Theme : MonoBehaviour
    {
        static public int nTheme = 4;

        public enum Theme
        {
            ruby = 0, sapphire = 1, emerald = 2, violet = 3
        }

        [SerializeField] Material rubyMaterial;
        [SerializeField] Material sapphireMaterial;
        [SerializeField] Material emeraldMaterial;
        [SerializeField] Material violetMaterial;

        static public Theme CurrentTheme { get; private set; }

        static Dictionary<Theme, Material> matList = new Dictionary<Theme, Material>();

        static MeshRenderer renderer1;
        static MeshRenderer renderer2;

        private void Awake()
        {
            renderer1 = gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
            renderer2 = gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();

            matList = new Dictionary<Theme, Material>()
            {
                { Theme.ruby, rubyMaterial },
                { Theme.sapphire, sapphireMaterial },
                { Theme.emerald, emeraldMaterial },
                { Theme.violet, violetMaterial },
            };
        }

        void Start()
        {
            UpdateMaterial();
        }

        static public void SwitchMaterial(Theme theme)
        {
            CurrentTheme = theme;
            UpdateMaterial();
        }

        static void UpdateMaterial()
        {
            if (renderer1 == null || renderer2 == null) { return; }
            if (matList == null) { return; }

            renderer1.material = matList[CurrentTheme];
            renderer2.material = matList[CurrentTheme];
        }
    }
}

