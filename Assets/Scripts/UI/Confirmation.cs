using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class Confirmation : MonoBehaviour
    {
        public delegate void Action();

        string message;
        Action agree;
        Action disagree;

        private void Start()
        {
            var body = gameObject.transform.GetChild(0).GetChild(0).GetChild(0);

            // confirm panel message.
            var messageText = body.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            messageText.text = message;

            // confirm panel disagree
            var disagreeButton = body.GetChild(1).gameObject.GetComponent<Button>();
            disagreeButton.onClick.AddListener(DisagreeButtonProcessing);

            // confirm panel agree.
            var agreeButton = body.GetChild(2).gameObject.GetComponent<Button>();
            agreeButton.onClick.AddListener(AgreeButtonProcessing);
        }

        protected void SetConfirmationAction(string message, Action agree, Action disagree)
        {
            this.message = message;
            this.agree = agree;
            this.disagree = disagree;
        }

        void AgreeButtonProcessing()
        {
            agree?.Invoke();
            Destroy(gameObject);
        }

        void DisagreeButtonProcessing()
        {
            disagree?.Invoke();
            gameObject.SetActive(false);
        }

        static GameObject _confirmation;

        static public void BeginConfirmation(string message, Action action_yes, Action action_no)
        {
            if (_confirmation == null) { _confirmation = Resources.Load<GameObject>("UI/Confirmation"); }
            if (GameObject.FindWithTag("Confirmation") != null) { return; }

            var confirmation = GameHost.Instantiate(_confirmation);

            var confirmationClass = confirmation.GetComponent<Confirmation>();
            confirmationClass.SetConfirmationAction(message, action_yes, action_no);
        }
    }
}

