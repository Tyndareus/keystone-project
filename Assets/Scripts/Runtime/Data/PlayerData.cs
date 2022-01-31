using UnityEngine;

[CreateAssetMenu(menuName = "Keystone/Player Data")]
public class PlayerData : ScriptableObject
{
    public int playerIndex;
    public Color color;
    public Sprite characterSprite;
    public int points;
}
