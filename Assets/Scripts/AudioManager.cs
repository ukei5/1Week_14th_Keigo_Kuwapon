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
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                VolumeChange(true);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                VolumeChange(false);
            }
        }
    }

    void VolumeChange(bool isUp)
    {
        if (isUp == true)
        {
            aud.volume += 0.1f;
        }
        else
        {
            aud.volume -= 0.1f;
        }
    }
    public void PlaySE(AudioClip audioClip)
    {
        aud.PlayOneShot(audioClip);
    }
}
