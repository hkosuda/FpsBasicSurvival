using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class FieldItem : MonoBehaviour
    {
        static readonly float period = 5.0f;

        static public EventHandler<Item> PlayerGotItem { get; set; }

        Item item;
        float pastTime;

        private void Start()
        {
            gameObject.GetComponent<SphereCollider>().isTrigger = true;
            gameObject.layer = Const.itemLayer;

            SetEvent(1);
        }

        private void OnDestroy()
        {
            SetEvent(-1);
        }

        void SetEvent(int indicator)
        {
            if (indicator > 0)
            {
                FocusSystem.Touched += OnTouched;
                TimerSystem.Updated += UpdateMethod;
            }

            else
            {
                FocusSystem.Touched -= OnTouched;
                TimerSystem.Updated -= UpdateMethod;
            }
        }

        protected void Init(Item item)
        {
            this.item = item;
        }

        void OnTouched(object obj, GameObject _gameObject)
        {
            if (_gameObject != gameObject) { return; }

            if (ItemMethod())
            {
                PlayerGotItem?.Invoke(this, item);
                Destroy(gameObject);
            }
        }

        private void UpdateMethod(object obj, float dt)
        {
            pastTime += Time.deltaTime;
            if (pastTime > period) { pastTime -= period; }

            var rotY = 360.0f * pastTime / period;
            gameObject.transform.eulerAngles = new Vector3(0.0f, rotY, 0.0f);
        }

        protected abstract bool ItemMethod();

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != Const.playerLayer) { return; }

            if (ItemMethod())
            {
                PlayerGotItem?.Invoke(this, item);
                Destroy(gameObject);
            }
        }
    }
}

