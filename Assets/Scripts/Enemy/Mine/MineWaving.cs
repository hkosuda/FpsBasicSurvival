using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineWaving : MonoBehaviour
{
    BoxCollider boxCollider;
    GameObject body;
    float theta;

    private void Awake()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
        body = gameObject.transform.GetChild(0).gameObject;
    }

    void Start()
    {
        SetPhase();
        SetEvent(1);

        // - inner function
        void SetPhase()
        {
            var id = gameObject.GetComponent<MineBrain>().ID;
            SeedManager.SetSeed(id);
            theta = UnityEngine.Random.Range(0.0f, Mathf.PI);
        }
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            TimerSystem.Updated += UpdateMethod;
        }

        else
        {
            TimerSystem.Updated -= UpdateMethod;
        }
    }

    // Update is called once per frame
    void UpdateMethod(object obj, float dt)
    {
        theta += 2.0f * Mathf.PI * (dt / Floats.Get(Floats.Item.mine_waving_period));
        theta %= 2.0f * Mathf.PI;

        var height = Floats.Get(Floats.Item.mine_height) + Floats.Get(Floats.Item.mine_waving_amp) * Mathf.Sin(theta);

        body.transform.localPosition = new Vector3(0.0f, height, 0.0f);
        boxCollider.center = new Vector3(0.0f, height, 0.0f);
    }
}
