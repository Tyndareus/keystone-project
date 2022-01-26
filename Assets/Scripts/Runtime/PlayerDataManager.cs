using System.Linq;
using UnityEngine;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    [Range(1, 4)] public int maxPlayerCount;
    public int playerCount;
    [SerializeField] private PlayerData playerDataTemplate;
    
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

    public void OnCharacterSelected(int index, Sprite sprite)
    {
        players[index].characterSprite = sprite;
        
        /*if (players.Count(p => p.characterSprite != null) >=
            maxPlayerCount)
        {*/
            LevelManager.Instance.LoadNextScene();
        //}
    }

    public PlayerData SelectPlayer(int index) => players[index];
    public Sprite GetPlayerSprite(int index) => players[index].characterSprite;
    public int GetPlayerScore(int index) => players[index].points;
    public void UpdatePlayerScore(int index, int score) => players[index].points = score;
}
