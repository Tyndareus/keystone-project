using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldPlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager inputManager;
    [SerializeField] private Transform characterSpawn;
    [SerializeField] private LayerMask safeLayer;

    private List<PlayerInput> currentPlayers;

    private void Awake()
    {
        currentPlayers = new List<PlayerInput>();
    }

    private void Start()
    {
        for (int i = 0; i < PlayerDataManager.Instance.maxPlayerCount; i++)
        {
            if (i >= InputSystem.devices.Count) continue;

            inputManager.JoinPlayer();
        }
    }

    public void OnPlayerJoined(PlayerInput pInput)
    {
        SpriteRenderer sr = pInput.gameObject.GetComponent<SpriteRenderer>();
        PlayerData pd = PlayerDataManager.Instance.SelectPlayer(pInput.playerIndex);

        sr.sprite = pd.characterSprite;
        pInput.transform.SetParent(characterSpawn, false);

        currentPlayers.Add(pInput);
    }

    public void AllowPlayerToMove(int player)
    {
        foreach (var p in currentPlayers)
        {
            p.DeactivateInput();
        }
        
        currentPlayers[player].ActivateInput();
    }
}
