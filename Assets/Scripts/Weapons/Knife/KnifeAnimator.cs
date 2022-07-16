using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeAnimator : MonoBehaviour
{
    static Animator animator;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

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
            KnifeController.Shot += BeginShootingAnimation;
        }

        else
        {
            KnifeController.Shot -= BeginShootingAnimation;
        }
    }

    void Update()
    {
        
    }

    static public void BeginChangingAnimation()
    {
        animator.SetTrigger("Change");
    }

    static public void BeginShootingAnimation(object obj, RaycastHit hit)
    {
        animator.SetTrigger("Shot");
    }
}
