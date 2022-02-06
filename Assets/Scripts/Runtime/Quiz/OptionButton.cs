using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionButton : Selectable
{
    public QuizManager quizManager;
    private QuizData.OptionData opt;
    private int optionIndex;

    private TMP_Text optionText;

    protected override void Start()
    {
        optionText = GetComponentInChildren<TMP_Text>();
        optionText.text = opt.text;
    }
    
    public void UpdateOption(QuizData.OptionData data, int index)
    {
        opt = data;
        optionIndex = index;

        if (optionText == null) return;
        
        optionText.text = data.text;
    }

    public void Submit(int playerIndex)
    {
        quizManager.OnOptionSelected(playerIndex, optionIndex);
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
