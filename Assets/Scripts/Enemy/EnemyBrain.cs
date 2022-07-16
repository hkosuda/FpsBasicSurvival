using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class EnemyBrain : MonoBehaviour
    {
        static public EventHandler<EnemyBrain> DetectedPlayer { get; set; }

        public bool IsTracking { get; protected set; }
        public EnemyType EnemyType { get; protected set; }
    }
}

