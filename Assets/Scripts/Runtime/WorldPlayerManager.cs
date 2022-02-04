using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldPlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager inputManager;
    [SerializeField] private Transform characterSpawn;

    private List<PlayerInput> currentPlayers;
    private static readonly int Selected = Animator.StringToHash("Selected");

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
        PlayerData pd = PlayerDataManager.Instance.SelectPlayer(pInput.playerIndex);

        GameObject player = Instantiate(pd.character, pInput.transform, false);
        player.transform.position = new Vector3(0.0f, -0.5f, 0.0f);
        player.transform.localScale = pd.characterScale;

        pInput.transform.SetParent(characterSpawn, false);

        currentPlayers.Add(pInput);
        
        player.GetComponentInChildren<Animator>().SetBool(Selected, true);
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
