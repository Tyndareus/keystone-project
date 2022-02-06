using UnityEngine;

[CreateAssetMenu(menuName = "Keystone/Player Data")]
public class PlayerData : ScriptableObject
{
    public GameObject character;
    public GameObject idleCharacter;
    public int points;
}
