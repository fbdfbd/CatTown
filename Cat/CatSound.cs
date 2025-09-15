using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSound : MonoBehaviour
{
    [Header("Cat Sound Effect Clips")]
    [SerializeField]
    private AudioClip steps;
    [SerializeField]
    private List<AudioClip> meow = new List<AudioClip>();
    [SerializeField]
    private AudioSource catSource;


    public void PlayCatStepSFX(bool isPlay)
    {
        catSource.clip = steps;
        catSource.loop = isPlay;
        if (isPlay) catSource.Play();
        else catSource.Stop();
    }

    public void PlayCatMeowSFX()
    {
        int ran = Random.Range(0, 3);
        catSource.PlayOneShot(meow[ran]);
    }
}
