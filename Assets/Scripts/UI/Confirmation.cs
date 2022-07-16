using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Confirmation : MonoBehaviour
{
    public delegate void Action();

    string message;
    Action agree;
    Action disagree;

    private void Start()
    {
        var innerPanel = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);

        // confirm panel message.
        var messageText = innerPanel.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        messageText.text = message;

        // confirm panel agree.
        var agreeButton = innerPanel.GetChild(1).gameObject.GetComponent<Button>();
        agreeButton.onClick.AddListener(AgreeButtonProcessing);

        // confirm panel disagree
        var disagreeButton = innerPanel.GetChild(2).gameObject.GetComponent<Button>();
        disagreeButton.onClick.AddListener(DisagreeButtonProcessing);
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

        var confirmation = GameObject.Instantiate(_confirmation);

        var panel_confirmation = confirmation.GetComponent<Confirmation>();
        panel_confirmation.SetConfirmationAction(message, action_yes, action_no);

        if (GameHost.World != null)
        {
            panel_confirmation.transform.SetParent(GameHost.World.transform);
        }
    }
}
