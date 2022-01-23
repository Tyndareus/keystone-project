using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionButton : Selectable, IPointerClickHandler, ISubmitHandler
{
    public QuizManager quizManager;
    private string option;
    private int optionIndex;

    private TMP_Text optionText;

    protected override void Start()
    {
        optionText = GetComponentInChildren<TMP_Text>();

        optionText.text = option;
    }

    public void UpdateStateValues(Transition state, ColorBlock color, SpriteState sprite)
    {
        transition = state;
        colors = color;
        spriteState = sprite;
    }

    public void UpdateOption(string opt, int id)
    {
        option = opt;
        optionIndex = id;

        if (optionText == null) return;
        
        optionText.text = option;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        quizManager.OnOptionSelected(optionIndex);

        DoStateTransition(SelectionState.Pressed, false);
        StartCoroutine(OnFinishSubmit());
    }

    public void OnSubmit(BaseEventData eventData)
    {
        quizManager.OnOptionSelected(optionIndex);
        DoStateTransition(SelectionState.Pressed, false);
        StartCoroutine(OnFinishSubmit());
    }

    private IEnumerator OnFinishSubmit()
    {
        var fadeTime = colors.fadeDuration;
        var elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        
        DoStateTransition(currentSelectionState, false);
    }
}
