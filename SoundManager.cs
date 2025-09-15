using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField]
    private Slider BGMSlider;
    [SerializeField]
    private Slider SFXSlider;

    [Header("Background Music Clips")]
    public AudioClip dayTime1;
    public AudioClip dayTime2;
    public AudioClip night;

    [Header("UI Sound Effect Clips")]
    public AudioClip click;
    public AudioClip up1;
    public AudioClip up2;
    public AudioClip logo;
    public AudioClip error;
    public AudioClip star;
    public AudioClip starNot;
    public AudioClip item;

    [SerializeField]
    private AudioSource bgmSource;
    [SerializeField]
    private AudioSource uiSfxSource;

    [SerializeField]
    private List<AudioSource> catSource = new List<AudioSource>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
            uiSfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
            foreach (var catSource in catSource)
            {
                catSource.volume = PlayerPrefs.GetFloat("CatVolume", 0.5f);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayDayBGM()
    {
        int ran1 = Random.Range(0, 4);
        if (ran1 == 0)
        {
            int ran2 = Random.Range(0, 2);
            bgmSource.clip = ran2 == 0 ? dayTime1 : dayTime2;
            bgmSource.Play();
        }
        else
        {
            return;
        }

    }

    public void PlayNightBGM()
    {
        Debug.Log("PlayNightBGM called");
        int ran = Random.Range(0, 4);
        if (ran == 0)
        {
            bgmSource.clip = night;
            bgmSource.Play();
        }
        else
        {
            return;
        }
    }

    public void PlaySFX(string SFX)
    {
        switch (SFX)
        {
            case "click":
                uiSfxSource.PlayOneShot(click);
                break;
            case "up1":
                uiSfxSource.PlayOneShot(up1);
                break;
            case "up2":
                uiSfxSource.PlayOneShot(up2);
                break;
            case "error":
                uiSfxSource.PlayOneShot(error);
                break;
            case "logo":
                uiSfxSource.PlayOneShot(logo);
                break;
            case "star":
                uiSfxSource.PlayOneShot(star);
                break;
            case "starNot":
                uiSfxSource.PlayOneShot(starNot);
                break;
            case "item":
                uiSfxSource.PlayOneShot(item);
                break;
        }
    }
    
    

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        uiSfxSource.volume = volume;
        foreach (var cats in catSource)
        {
            cats.volume = volume;
        }
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
