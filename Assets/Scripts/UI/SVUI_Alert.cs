using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class SVUI_Alert : MonoBehaviour
    {
        static GameObject myself;
        static GameObject _alert;

        static List<EnemyBrain> brains;
        static List<GameObject> enemies;

        static List<GameObject> roots;
        static List<RectTransform> rootRects;

        private void Awake()
        {
            myself = gameObject;
            _alert = Resources.Load<GameObject>("UiComponent/Alert");

            brains = new List<EnemyBrain>();
            enemies = new List<GameObject>();

            roots = new List<GameObject>();
            rootRects = new List<RectTransform>();
        }

        void Start()
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
                EnemyBrain.DetectedPlayer += Initialize;
                TimerSystem.Updated += UpdateMethod;
            }

            else
            {
                EnemyBrain.DetectedPlayer -= Initialize;
                TimerSystem.Updated -= UpdateMethod;
            }
        }

        static void UpdateMethod(object obj, float dt)
        {
            var listLength = enemies.Count;

            for (var n = listLength - 1; n > -1; n--)
            {
                if (enemies[n] == null || !brains[n].IsTracking)
                {
                    Destroy(roots[n]);

                    brains.RemoveAt(n);
                    enemies.RemoveAt(n);
                    roots.RemoveAt(n);
                    rootRects.RemoveAt(n);
                }

                else
                {
                    UpdateRotation(enemies[n], rootRects[n]);
                }
            }
        }

        // function
        static void UpdateRotation(GameObject target, RectTransform rootRect)
        {
            if (target == null)
            {
                return;
            }

            var dx = target.transform.position.x - Player.Myself.transform.position.x;
            var dz = target.transform.position.z - Player.Myself.transform.position.z;

            var theta1 = Player.Camera.transform.eulerAngles.y;
            var theta2 = Mathf.Atan2(dx, dz) * Mathf.Rad2Deg;

            var dt = theta2 - theta1;

            rootRect.rotation = Quaternion.Euler(0, 0, -dt);
        }

        static void Initialize(object obj, EnemyBrain brain)
        {
            var alert = GameObject.Instantiate(_alert);
            alert.transform.SetParent(myself.transform);

            alert.transform.localPosition = Vector3.zero;

            var enemy = brain.gameObject;
            var rect = alert.GetComponent<RectTransform>();

            brains.Add(brain);
            enemies.Add(enemy);

            roots.Add(alert);
            rootRects.Add(rect);

            UpdateRotation(enemy, rect);

            // set color
            var img = alert.transform.GetChild(0).gameObject.GetComponent<Image>();
            Color color;

            if (brain.EnemyType == EnemyType.mine)
            {
                color = new Color(1.0f, 1.0f, 0.0f);
            }

            else
            {
                color = new Color(1.0f, 0.0f, 0.0f);
            }

            img.color = color;
        }
    }
}

