using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    AudioSource aud;
    public AudioClip sceneChange = default;
    public AudioClip danceTrue = default;
    public AudioClip danceFalse = default;
    public AudioClip click = default;
    public AudioClip win = default;
    public AudioClip lose = default;
    public AudioClip encount = default;
    private void Awake()
    {
        aud = GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void PlayBGM(AudioClip audioClip)
    {
        GameObject.Find("BGMManager").GetComponent<AudioSource>().Stop();
        GameObject.Find("BGMManager").GetComponent<AudioSource>().PlayOneShot(audioClip);
    }
    public void PlaySE(AudioClip audioClip)
    {
        aud.PlayOneShot(audioClip);
    }
}
