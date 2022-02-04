using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimerManager : MonoBehaviour
{
    public static bool InPlay;

    [SerializeField] private float countdownTimer;
    [SerializeField] private float gameTimer;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private FadeManager fadeManager;

    [SerializeField] private UnityEvent onGameStart;
    
    private TimeSpan timer;

    private void Start()
    {
        timer = TimeSpan.FromSeconds(gameTimer);
        timerText.text = $"Time: {timer.Minutes:D2}:{timer.Seconds:D2}:{timer.Milliseconds:D2}";
        fadeManager.FadeIn();
    }

    public void OnFadeIn() => StartCoroutine(BeginCountdown());

    private void Update()
    {
        if (!InPlay) return;
        
        timer -= TimeSpan.FromSeconds(Time.deltaTime);

        if (timer.TotalSeconds <= 0)
        {
            InPlay = false;
            fadeManager.FadeOut();
        }

        timerText.text = $"Time: {timer.Minutes:D2}:{timer.Seconds:D2}:{timer.Milliseconds:D2}";
    }

    private IEnumerator BeginCountdown()
    {
        for (float t = 0.0f; t <= countdownTimer; t += Time.deltaTime)
        {
            //TODO: Display countdown timer thing
            yield return null;
        }

        InPlay = true;
        onGameStart?.Invoke();
    }

    public void ReduceTime(float time)
    {
        if (!InPlay) return;

        timer -= TimeSpan.FromSeconds(time);
    }

    public void OnFadeOut() => LevelManager.Instance.LoadNextScene();
}
