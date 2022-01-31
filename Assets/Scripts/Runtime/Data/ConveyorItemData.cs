using UnityEngine;

[CreateAssetMenu(menuName = "Keystone/Conveyor Item")]
public class ConveyorItemData : ScriptableObject
{
    public Sprite sprite;
    public CollisionType collision;
}

public enum CollisionType
{
    Circle,
    Square
}