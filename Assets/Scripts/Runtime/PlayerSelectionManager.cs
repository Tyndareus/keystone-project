using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class PlayerSelectionManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager inputManager;
    [SerializeField] private GameObject playerUIInput;
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    [SerializeField] private Transform selectableParent;
    [SerializeField] private Transform idlePlayerSpawn;
    [SerializeField] private UnityEvent onCharacterSelected;

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

        if (selectableParent.childCount > 0)
        {
            mes.firstSelectedGameObject = selectableParent.GetChild(0).gameObject;
        }

        mes.playerRoot = selectableParent.gameObject;
        
        PlayerDataManager.Instance.RegisterPlayer(pInput.playerIndex);

        var module = playerUI.GetComponent<InputSystemUIInputModule>();
        pInput.uiInputModule = module;
        pInput.SwitchCurrentActionMap("UI");

        UIPlayerController uiController = pInput.GetComponent<UIPlayerController>();
        uiController.playerEventSystem = mes;
        uiController.uiRaycaster = graphicRaycaster;

        if (idlePlayerSpawn == null) return;
        
        GameObject idleCharacter = PlayerDataManager.Instance.GetPlayerCharacter(pInput.playerIndex, true);
        GameObject spawnedCharacter = Instantiate(idleCharacter, idlePlayerSpawn);
        spawnedCharacter.transform.localScale /= 10.0f;
    }

    public void SelectCharacter(int playerIndex, int charIndex)
    {
        onCharacterSelected?.Invoke();
        PlayerDataManager.Instance.OnCharacterSelected(playerIndex, charIndex);
    }
}
