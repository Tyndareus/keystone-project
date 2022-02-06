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

    private static readonly int Selected = Animator.StringToHash("Selected");

    private Selectable previouslySelected;

    private void Start()
    {
        
    }

    public void Navigate(InputAction.CallbackContext ctx)
    {
        
    }

    public void MouseNavigation(InputAction.CallbackContext ctx)
    {
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
    }

    public void Submit(InputAction.CallbackContext ctx)
    {
        if (ctx.action.name == "Click" && ctx.performed)
        {
            if(!ValidateClick()) return;
        }
        
        PlayerInput pInput = GetComponent<PlayerInput>();

        if (playerEventSystem == null) return;
        if (playerEventSystem.currentSelectedGameObject == null) return;

        RectTransform st = playerEventSystem.currentSelectedGameObject.GetComponent<RectTransform>();

        ValidateFlow(pInput.playerIndex, st);
    }

    private void ValidateFlow(int playerIndex, RectTransform rectTransform)
    {
        Selectable selectable = rectTransform.GetComponentInChildren<Selectable>();
        switch (selectable)
        {
            case Card c:
                c.Submit(playerIndex);
                return;
            case OptionButton ob:
                ob.Submit(playerIndex);
                return;
            case PlayerSelection ps:
                if (previouslySelected != null)
                {
                    previouslySelected.interactable = true;
                }
                
                ps.interactable = false;
                ps.PlayerSelect(playerIndex);
                
                previouslySelected = selectable;
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
}
