using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorTurnManager : MonoBehaviour
{
    [SerializeField] private WorldPlayerManager playerManager;
    
    public int whosTurnIsIt;

    private void Start()
    {
        //TODO: Present messages/information/sounds etc before setting player spawn
        playerManager.AllowPlayerToMove(whosTurnIsIt);
    }
}
