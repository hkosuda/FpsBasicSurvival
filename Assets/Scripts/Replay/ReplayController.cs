using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class ReplayController : MonoBehaviour
    {
        private void Start()
        {
            var button = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Button>();
            button.onClick.AddListener(CloseController);
        }

        private void OnDestroy()
        {
            ReplaySystem.FinishReplay();
        }

        void CloseController()
        {
            Destroy(gameObject);
        }
    }
}

