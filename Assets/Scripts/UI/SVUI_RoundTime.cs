using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SVUI_RoundTime : MonoBehaviour
{
    TextMeshProUGUI roundText;
    TextMeshProUGUI timerText;

    float currentTime;
    int currentRound;

    private void Awake()
    {
        roundText = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        timerText = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
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
            IgSetting.Updated += UpdateText;
        }

        else
        {
            IgSetting.Updated -= UpdateText;
        }
    }

    void Update()
    {
        if (currentTime == SV_TimeAdmin.ActiveTime && currentRound == SV_RoundAdmin.RoundNumber) { return; }

        currentTime = SV_TimeAdmin.ActiveTime;
        currentRound = SV_RoundAdmin.RoundNumber;

        UpdateText(null, Ints.Item.sv_clear_round);
    }

    void UpdateText(object obj, Ints.Item item)
    {
        if (item != Ints.Item.sv_clear_round) { return; }

        roundText.text = "Round " + SV_RoundAdmin.RoundNumber.ToString() + " / " + Ints.Get(Ints.Item.sv_clear_round).ToString();
        timerText.text = Utility.GetTimeText(SV_TimeAdmin.ActiveTime, true);
    }
}
