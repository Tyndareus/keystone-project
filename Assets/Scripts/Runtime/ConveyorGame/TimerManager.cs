using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TimerManager : MonoBehaviour
{
    public static bool InPlay;

    [SerializeField] private float countdownTimer;
    [SerializeField] private float gameTimer;
    [SerializeField] private Animator timerAnimator;
    [SerializeField] private FadeManager secondaryFade;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private UnityEvent onGameStart, onGameFinished, onTimerScored;
    [SerializeField] private AudioSource alertSound;
    

    private TimeSpan timer;
    private float animTimer, timeSincePrevention;
    private bool coroutineActive, canScore, alerted;

    private static readonly int animTime = Animator.StringToHash("time");

    private void Start()
    {
        canScore = true;
        timer = TimeSpan.FromSeconds(gameTimer);
    }

    public void OnFadeIn() => StartCoroutine(BeginCountdown());

    private void Update()
    {
        if (!InPlay) return;

        timer -= TimeSpan.FromSeconds(Time.deltaTime);

        if (timer.TotalSeconds < 10 && !alerted)
        {
            alerted = true;
            alertSound.Play();
        }

        if (canScore)
        {
            timeSincePrevention += Time.deltaTime;
        }

        if (!coroutineActive)
        {
            animTimer += Time.deltaTime;
        }

        if (animTimer >= 1 && (int)(animTimer % 10) == 0 && !coroutineActive)
        {
            coroutineActive = true;
            StartCoroutine(FlipHourglass());
        }
        else if (!coroutineActive)
        {
            timerAnimator.SetFloat(animTime, animTimer / 10f);
        }

        if (canScore && (int)timeSincePrevention > 0 && (int)(timeSincePrevention % 5) == 0)
        {
            canScore = false;
            onTimerScored?.Invoke();
            scoreManager.TryScore(false);
            StartCoroutine(ScoreCooldown());
        }

        if (timer.TotalSeconds <= 0 && InPlay)
        {
            InPlay = false;
            StartCoroutine(GoForFade());
        }
    }

    private IEnumerator BeginCountdown()
    {
        for (float t = 0.0f; t <= countdownTimer; t += Time.deltaTime)
        {
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

    public void PreventScore()
    {
        timeSincePrevention = 0;
        canScore = false;
        StartCoroutine(ScoreCooldown());
        scoreManager.PreventScore();
    }

    private IEnumerator FlipHourglass()
    {
        RectTransform timerTransform = timerAnimator.GetComponent<RectTransform>();
        Vector3 currentRotation = timerTransform.eulerAngles;
        float currentZ = currentRotation.z;

        for (float t = 0.0f; t <= 1.0f; t += Time.deltaTime)
        {
            currentRotation.z = Mathf.Lerp(currentZ, currentZ + 180f, t / 0.9694f);
            timerTransform.eulerAngles = currentRotation;
            yield return null;
        }

        currentRotation.z = 0f;
        timerTransform.eulerAngles = currentRotation;
        timerAnimator.SetFloat(animTime, 0.0f);
        animTimer = 0;

        coroutineActive = false;
    }

    private IEnumerator GoForFade()
    {
        onGameFinished?.Invoke();
        FindObjectOfType<AudioManager>().OnGameComplete();

        for (float t = 0.0f; t <= 5.0f; t += Time.deltaTime) yield return null;

        secondaryFade.FadeOut();
    }

    private IEnumerator ScoreCooldown()
    {
        for (float t = 0.0f; t <= 2.0f; t += Time.deltaTime) yield return null;

        canScore = true;
    }
}