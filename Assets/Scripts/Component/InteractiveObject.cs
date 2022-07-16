using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class InteractiveObject : MonoBehaviour
    {
        public delegate void OnShotReaction();
        public delegate void OnFocusedReaction();
        public delegate void OnDefocusedReaction();
        public delegate void OnTouchedReaction();

        OnShotReaction onShotReaction;
        OnFocusedReaction onFocusedReaction;
        OnDefocusedReaction onDefocusedReaction;
        OnTouchedReaction onTouchedReaction;

        // Start is called before the first frame update
        void Start()
        {
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
                WeaponController.ShootingHit += ReactToShot;
            }

            else
            {
                WeaponController.ShootingHit -= ReactToShot;
            }
        }

        void ReactToShot(object obj, GameObject _gameObject)
        {
            if (_gameObject != gameObject) { return; }

            onShotReaction?.Invoke();
        }

        void ReactToTouch(object obj, GameObject _gameObject)
        {
            if (_gameObject != gameObject) { return; }

            onTouchedReaction?.Invoke();
        }

        void ReactToFocus(object obj, GameObject _gameObject)
        {
            if (_gameObject != gameObject) { return; }

            onFocusedReaction?.Invoke();
        }

        void ReactToDefocus(object obj, GameObject _gameObject)
        {
            if (_gameObject != gameObject) { return; }

            onDefocusedReaction?.Invoke();
        }

        //
        // set reactions
        public void SetOnShotReaction(OnShotReaction onShotReaction)
        {
            this.onShotReaction = onShotReaction;
        }

        public void SetOnFocusedReaction(OnFocusedReaction onFocusedReaction)
        {
            this.onFocusedReaction = onFocusedReaction;
        }

        public void SetOnDefocusedReaction(OnDefocusedReaction onDefocusedReaction)
        {
            this.onDefocusedReaction = onDefocusedReaction;
        }

        public void SetOnTouchecReaction(OnTouchedReaction onTouchedReaction)
        {
            this.onTouchedReaction = onTouchedReaction;
        }
    }
}

