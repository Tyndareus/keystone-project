using System;
using UnityEngine;

public class ConveyorBeltSpeedManager : MonoBehaviour
{
    [SerializeField] private Animator conveyorBeltAnimator;
    [SerializeField] private SurfaceEffector2D surfaceEffector;
    [SerializeField] private float offset;
    
    private static readonly int Play = Animator.StringToHash("Play");

    public void OnGameStart() => conveyorBeltAnimator.SetTrigger(Play);
    
    private void Start()
    {
        conveyorBeltAnimator.speed = Mathf.Abs(surfaceEffector.speed) - offset;
    }

    public void OnScore()
    {
        surfaceEffector.speed -= 0.2f;
        conveyorBeltAnimator.speed = Mathf.Abs(surfaceEffector.speed) - offset;
    }
}
