using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private FadeManager fadeManager, altFade;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        StartCoroutine(DelayIntroScreen());
    }

    public void OnPlayClick()
    {
        FindObjectOfType<AudioManager>().FadeOut();
        fadeManager.FadeOut();
    }

    public void OnQuitClick()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnFadeOut()
    {
        LevelManager.Instance.LoadNextScene();
    }

    private IEnumerator DelayIntroScreen()
    {
        for (float t = 0.0f; t <= 3.0f; t += Time.deltaTime) yield return null;
        
        altFade.FadeIn();
    }
}