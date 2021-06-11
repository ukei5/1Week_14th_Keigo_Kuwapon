﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChangeButton : MonoBehaviour
{
    public void OnClick(string sceneName)
    {
        AudioManager.instance.PlaySE(AudioManager.instance.click);
        SceneManager.LoadScene(sceneName);
    }
}
