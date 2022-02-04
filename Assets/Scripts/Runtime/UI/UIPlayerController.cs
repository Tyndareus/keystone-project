using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIPlayerController : MonoBehaviour
{
    [NonSerialized] public MultiplayerEventSystem playerEventSystem;
    [NonSerialized] public GraphicRaycaster uiRaycaster;

    private bool hasSelected;
    private static readonly int Selected = Animator.StringToHash("Selected");

    private GameObject previouslySelected;

    private void Start()
    {
        UpdateCaretPosition();
    }

    public void Navigate(InputAction.CallbackContext ctx)
    {
        if (hasSelected) return;
        
        UpdateCaretPosition();
    }

    public void MouseNavigation(InputAction.CallbackContext ctx)
    {
        if (hasSelected) return;
        
        PointerEventData ped = new PointerEventData(playerEventSystem)
        {
            position = ctx.ReadValue<Vector2>()
        };

        List<RaycastResult> results = new List<RaycastResult>();
        GameObject hitResult = null;

        if (uiRaycaster == null) return;
        
        uiRaycaster.Raycast(ped, results);
        foreach (var result in results)
        {
            var selectable = result.gameObject.GetComponentInChildren<Selectable>();
            if (selectable != null)
            {
                hitResult = result.gameObject;
            }
        }

        if (hitResult != null)
        {
            playerEventSystem.SetSelectedGameObject(hitResult);
        }
        else
        {
            if (previouslySelected != null)
            {
                Animator previousAnim = previouslySelected.GetComponentInChildren<Animator>();
                if (previousAnim != null)
                {
                    previousAnim.SetBool(Selected, false);
                }
            }
            
            playerEventSystem.SetSelectedGameObject(null);
        }

        UpdateCaretPosition();
    }

    public void Submit(InputAction.CallbackContext ctx)
    {
        if (hasSelected) return;

        if (ctx.action.name == "Click" &&
            ctx.performed)
        {
            if(!ValidateClick()) return;
        }
        
        UpdateCaretPosition();
        
        PlayerInput pInput = GetComponent<PlayerInput>();

        if (playerEventSystem == null) return;
        if (playerEventSystem.currentSelectedGameObject == null) return;

        RectTransform st = playerEventSystem.currentSelectedGameObject.GetComponent<RectTransform>();

        hasSelected = true;
        st.GetComponent<Selectable>().interactable = false;

        ValidateFlow(pInput.playerIndex, st);
    }

    private void UpdateCaretPosition()
    {
        if (playerEventSystem == null) return;

        if (playerEventSystem.currentSelectedGameObject == null) return;

        RectTransform st = playerEventSystem.currentSelectedGameObject.GetComponent<RectTransform>();
        
        if (previouslySelected != null)
        {
            Animator previousAnim = previouslySelected.GetComponentInChildren<Animator>();
            if (previousAnim != null)
            {
                previousAnim.SetBool(Selected, false);
            }
        }
        
        Animator currentAnim = st.gameObject.GetComponentInChildren<Animator>();
        if (currentAnim != null)
        {
            currentAnim.SetBool(Selected, true);
        }
        
        previouslySelected = st.gameObject;
    }

    private void ValidateFlow(int playerIndex, RectTransform rectTransform)
    {
        Selectable selectable = rectTransform.GetComponentInChildren<Selectable>();
        switch (selectable)
        {
            case Card c:
                c.Submit(playerIndex);
                hasSelected = false;
                return;
            case OptionButton ob:
                ob.Submit(playerIndex);
                hasSelected = false;
                return;
            case PlayerSelection ps:
                Animator anim = rectTransform.GetComponentInChildren<Animator>();
                ps.PlayerSelect(playerIndex, anim.transform.localScale);
                playerEventSystem.enabled = false;
                break;
        }
    }

    private bool ValidateClick()
    {
        //There won't be more than one mouse... surely
        PointerEventData ped = new PointerEventData(playerEventSystem)
        {
            position = Mouse.current.position.ReadValue()
        };

        List<RaycastResult> results = new List<RaycastResult>();
        uiRaycaster.Raycast(ped, results);
        return results.Any(result => result.gameObject == playerEventSystem.currentSelectedGameObject);
    }

    public void DisableInput() => hasSelected = true;
    public void EnableInput() => hasSelected = false;
}
