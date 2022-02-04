using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSelection : Selectable
{
    [SerializeField] private PlayerSelectionManager manager;
    
    public void PlayerSelect(int playerIndex, Vector3 scale)
    {
        manager.SelectCharacter(playerIndex, transform.GetSiblingIndex(), scale);
    }
}
