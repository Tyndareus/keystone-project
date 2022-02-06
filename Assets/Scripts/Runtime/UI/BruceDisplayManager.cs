using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BruceDisplayManager : MonoBehaviour
{
    [SerializeField] private List<string> comments;
    [SerializeField] private RectTransform bruceTransform;
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private float minFrequency, maxFrequency;

    private string previousMessage;
    private bool displayed;

    private void Awake()
    {
        displayed = false;
    }
    
    private void Update()
    {
        if (!TimerManager.InPlay) return;
        
        if (displayed) return;
        
        float rand = Random.Range(minFrequency, maxFrequency);

        StartCoroutine(Display(rand));
    }

    private IEnumerator Display(float delay)
    {
        displayed = true;
        for (float t = 0.0f; t <= delay; t += Time.deltaTime) yield return null;

        Vector2 curPos = bruceTransform.anchoredPosition;
        float currentX = curPos.x;
        float targetX = -currentX;

        string comment = comments[Random.Range(0, comments.Count)];
        while (comment == previousMessage)
        {
            comment = comments[Random.Range(0, comments.Count)];
            yield return null;
        }
        
        textBox.text = comment;
        previousMessage = comment;
        
        for (float t = 0.0f; t <= 1.5f; t += Time.deltaTime)
        {
            curPos.x = Mathf.Lerp(currentX, targetX, t / 1.0f);
            bruceTransform.anchoredPosition = curPos;
            yield return null;
        }

        for (float t = 0.0f; t <= 5.0f; t += Time.deltaTime) yield return null;

        StartCoroutine(Hide());
    }

    private IEnumerator Hide()
    {
        Vector2 curPos = bruceTransform.anchoredPosition;
        float currentX = curPos.x;
        float targetX = -currentX;

        for (float t = 0.0f; t <= 1.5f; t += Time.deltaTime)
        {
            curPos.x = Mathf.Lerp(currentX, targetX, t / 1.0f);
            bruceTransform.anchoredPosition = curPos;
            yield return null;
        }

        textBox.text = "";
        displayed = false;
    }
}
