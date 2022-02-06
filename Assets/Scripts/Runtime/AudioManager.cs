using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private OptionsData data;
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private List<AudioClip> audioClips;
    [SerializeField] private AudioClip victoryClip;

    private void Start()
    {    
        backgroundMusicSource.Play();
        StartCoroutine(FadeMusicIn());
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void FadeOut()
    {
        StartCoroutine(FadeMusicOut());
    }

    public void OnGameComplete()
    {
        backgroundMusicSource.Stop();
        backgroundMusicSource.loop = false;
        backgroundMusicSource.clip = victoryClip;
        backgroundMusicSource.Play();

        StartCoroutine(FadeOnVictory());
    }

    private void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0 || scene.buildIndex == SceneManager.sceneCount - 1) return;
        
        StopAllCoroutines();
        backgroundMusicSource.loop = true;
        backgroundMusicSource.clip = audioClips[SceneData.currentScene - 1];
        backgroundMusicSource.Play();

        StartCoroutine(FadeMusicIn());
    }

    private IEnumerator FadeMusicIn()
    {
        for (float t = 0.0f; t <= 2.0f; t += Time.deltaTime)
        {
            backgroundMusicSource.volume = Mathf.Lerp(0.0f, data.volumeLevel, t / 1.0f);
            yield return null;
        }

        backgroundMusicSource.volume = data.volumeLevel;
    }

    private IEnumerator FadeMusicOut()
    {
        for (float t = 0.0f; t <= 2.0f; t += Time.deltaTime)
        {
            backgroundMusicSource.volume = Mathf.Lerp(data.volumeLevel, 0.0f, t / 1.0f);
            yield return null;
        }

        backgroundMusicSource.volume = 0;
        backgroundMusicSource.Stop();
    }

    private IEnumerator FadeOnVictory()
    {
        while (backgroundMusicSource.isPlaying) yield return null;

        StartCoroutine(FadeMusicOut());
    }
}
