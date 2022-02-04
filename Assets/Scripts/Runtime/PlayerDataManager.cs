using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    [Range(1, 4)] public int maxPlayerCount;
    public int playerCount;
    [SerializeField] private PlayerData playerDataTemplate;

    [SerializeField] private List<GameObject> characters;
    [SerializeField] private List<GameObject> idleCharacters;

    private PlayerData[] players;

    public override void Awake()
    {
        base.Awake();
        players = new PlayerData[maxPlayerCount];
    }

    public void RegisterPlayer(int index)
    {
        if (players[index] != null) return;
        
        players[index] = Instantiate(playerDataTemplate);
        players[index].playerIndex = index;
        playerCount++;
    }

    public void OnCharacterSelected(int index, int charIndex, Vector3 assumedScale)
    {
        players[index].character = characters[charIndex];
        players[index].idleCharacter = idleCharacters[charIndex];
        players[index].characterScale = assumedScale / 100f;

        /*if (players.Count(p => p.character != null) >=
            maxPlayerCount)
        {
            LevelManager.Instance.LoadNextScene();
        }*/
    }

    public PlayerData SelectPlayer(int index) => players[index];

    public GameObject GetPlayerCharacter(int index, bool useIdle = false) =>
        useIdle ? players[index].idleCharacter : players[index].character;

    public int GetPlayerScore(int index) => players[index].points;
    public void UpdatePlayerScore(int index, int score) => players[index].points = score;
}
