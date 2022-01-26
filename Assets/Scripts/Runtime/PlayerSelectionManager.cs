using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerSelectionManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager inputManager;
    [SerializeField] private GameObject playerUIInput;
    [FormerlySerializedAs("characterParent")] [SerializeField] private Transform selectableParent;
    [SerializeField] private GameObject playerCaret;
    
    [SerializeField] private Transform playerCaretParent;

    private void Start()
    {
        for (int i = 0; i < PlayerDataManager.Instance.maxPlayerCount; i++)
        {
            //Don't want to create more players than physically controllable
            if (i >= InputSystem.devices.Count) continue;
            
            inputManager.JoinPlayer();
        }
    }

    public void OnPlayerJoined(PlayerInput pInput)
    {
        GameObject playerUI = Instantiate(playerUIInput);
        MultiplayerEventSystem mes = playerUI.GetComponent<MultiplayerEventSystem>();
        mes.firstSelectedGameObject = selectableParent.GetChild(0).gameObject;
        mes.playerRoot = selectableParent.gameObject;
        
        PlayerDataManager.Instance.RegisterPlayer(pInput.playerIndex);

        var module = playerUI.GetComponent<InputSystemUIInputModule>();
        pInput.uiInputModule = module;
        pInput.SwitchCurrentActionMap("UI");

        GameObject caret = Instantiate(playerCaret, playerCaretParent);
        
        //TEMP
        switch (pInput.playerIndex)
        {
            case 0: caret.GetComponent<Image>().color = Color.red; break;
            case 1: caret.GetComponent<Image>().color = Color.cyan; break;
            case 2: caret.GetComponent<Image>().color = Color.green; break;
            case 3: caret.GetComponent<Image>().color = Color.yellow; break;
        }

        UIPlayerController uiController = pInput.GetComponent<UIPlayerController>();
        uiController.playerOutlineObject = caret;
        uiController.playerEventSystem = mes;
    }
}
