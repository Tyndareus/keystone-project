using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerSelection : Selectable
{
    [SerializeField] private PlayerSelectionManager manager;

    [SerializeField] private int charId;
    [SerializeField] private UnityEvent onSelect;
    
    public void PlayerSelect(int playerIndex)
    {
        onSelect?.Invoke();
        manager.SelectCharacter(playerIndex, charId);
    }
}
