using UnityEngine;

[CreateAssetMenu(menuName = "Keystone/Player Data")]
public class PlayerData : ScriptableObject
{
    public int playerIndex;
    public GameObject character;
    public GameObject idleCharacter;
    public int points;
    public Vector3 characterScale;
}
