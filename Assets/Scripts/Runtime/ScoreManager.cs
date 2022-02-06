using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreDisplay;
    [SerializeField] private AudioSource scoreSound;

    public bool quizScored;
    
    private bool preventScore, scoreCooldown;
    private Coroutine scoreCoroutine;

    private void Awake()
    {
        scoreDisplay.text = $"{PlayerDataManager.Instance.GetPlayerScore(0)}";
    }

    public void TryScore(bool fromQuiz, bool ignoreCooldown = false)
    {
        if (!fromQuiz && quizScored) return;

        if (!ignoreCooldown)
        {
            if (preventScore) return;

            if (scoreCooldown) return;
        }

        scoreSound.Play();
        scoreCooldown = true;
        PlayerDataManager.Instance.UpdatePlayerScore(0, 1);

        if (!ignoreCooldown)
        {
            StartCoroutine(ScoreCooldown());
        }
    }

    public void PreventScore()
    {
        preventScore = true;

        if (scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine);
            scoreCoroutine = null;
        }
        
        scoreCoroutine = StartCoroutine(CheckCooldown());
    }

    private void Update()
    {
        if (!TimerManager.InPlay) return;
        
        scoreDisplay.text = $"{PlayerDataManager.Instance.GetPlayerScore(0)}";
    }

    private IEnumerator CheckCooldown()
    {
        for (float t = 0.0f; t <= 1.25f; t += Time.deltaTime) yield return null;
        preventScore = false;
    }

    private IEnumerator ScoreCooldown()
    {
        for (float t = 0.0f; t <= 1.5f; t += Time.deltaTime) yield return null;
        scoreCooldown = false;
    }
}
