using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIPlayerController : MonoBehaviour
{
    [NonSerialized] public GameObject playerOutlineObject;
    [NonSerialized] public MultiplayerEventSystem playerEventSystem;

    private bool hasSelected;

    private void Start()
    {
        UpdateCaretPosition();
    }

    public void Navigate(InputAction.CallbackContext ctx)
    {
        if (hasSelected) return;
        
        UpdateCaretPosition();
    }

    public void Submit(InputAction.CallbackContext ctx)
    {
        if (hasSelected) return;
        
        UpdateCaretPosition();
        
        PlayerInput pInput = GetComponent<PlayerInput>();

        RectTransform st = playerEventSystem.currentSelectedGameObject != null
            ? playerEventSystem.currentSelectedGameObject.GetComponent<RectTransform>()
            : playerEventSystem.firstSelectedGameObject.GetComponent<RectTransform>();

        hasSelected = true;
        st.GetComponent<Selectable>().interactable = false;

        ValidateFlow(pInput.playerIndex, st);
    }

    private void UpdateCaretPosition()
    {
        if (playerOutlineObject == null) return;
        
        RectTransform rt = playerOutlineObject.GetComponent<RectTransform>();
        RectTransform st = playerEventSystem.currentSelectedGameObject != null
            ? playerEventSystem.currentSelectedGameObject.GetComponent<RectTransform>()
            : playerEventSystem.firstSelectedGameObject.GetComponent<RectTransform>();

        if (st.gameObject.GetComponent<Selectable>().IsInteractable())
        {
            rt.anchoredPosition = st.anchoredPosition;
        }
    }

    private void ValidateFlow(int playerIndex, RectTransform rectTransform)
    {
        Card card = rectTransform.GetComponent<Card>();
        if (card != null)
        {
            card.Submit(playerIndex);
            return;
        }

        OptionButton opt = rectTransform.GetComponent<OptionButton>();
        if (opt != null)
        {
            opt.Submit(playerIndex);
            return;
        }

        Image img = rectTransform.GetComponent<Image>();
        PlayerDataManager.Instance.OnCharacterSelected(playerIndex, img.sprite);
        playerEventSystem.enabled = false;
    }
}
