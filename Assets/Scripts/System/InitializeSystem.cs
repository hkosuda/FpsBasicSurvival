using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class InitializeSystem : MonoBehaviour
    {
        private void Awake()
        {
            BulletLine.Initialize();
            ImpactBox.Initialize();
        }

        private void OnDestroy()
        {
            BulletLine.Shutdown();
            ImpactBox.Shutdown();
        }
    }
}

