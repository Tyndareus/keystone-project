using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static bool InPlay;

    [SerializeField] private float countdownTimer;
    [SerializeField] private float gameTimer;
    
    
    private float timer;

    private void Start()
    {
        StartCoroutine(BeginCountdown());
    }

    private void Update()
    {
        if (InPlay)
        {
            timer += Time.deltaTime;

            if (timer >= gameTimer)
            {
                Debug.Log("Game Finished");
                InPlay = false;
            }
        }
    }

    private IEnumerator BeginCountdown()
    {
        for (float t = 0.0f; t <= countdownTimer; t += Time.deltaTime)
        {
            //TODO: Display countdown timer thing
            yield return null;
        }

        InPlay = true;
        FindObjectOfType<ObjectSpawner>().BeginSpawning();
    }
}
