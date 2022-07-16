using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkSound : MonoBehaviour
{
    static AudioSource audioSource;

    static AudioClip shootingClip;

    static AudioClip reloadingClip1st;
    static AudioClip reloadingClip2nd;
    static AudioClip reloadingClip3rd;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        if (shootingClip == null) { shootingClip = LoadClip("ak_shot"); }
        if (reloadingClip1st == null) { reloadingClip1st = LoadClip("ak_mag_off"); }
        if (reloadingClip2nd == null) { reloadingClip2nd = LoadClip("ak_mag_in"); }
        if (reloadingClip3rd == null) { reloadingClip3rd = LoadClip("ak_lever"); }

        // - inner function
        static AudioClip LoadClip(string fileName)
        {
            return Resources.Load<AudioClip>("Audio/ak/" + fileName);
        }
    }

    //
    // used in animation
    public void PlayShootingSound()
    {
        audioSource.PlayOneShot(shootingClip);
    }

    public void PlayReloadingSound1st()
    {
        audioSource.PlayOneShot(reloadingClip1st);
    }

    public void PlayReloadingSound2nd()
    {
        audioSource.PlayOneShot(reloadingClip2nd);
    }

    public void PlayReloadingSound3rd()
    {
        audioSource.PlayOneShot(reloadingClip3rd);
    }
}
