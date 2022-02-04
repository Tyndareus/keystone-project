using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private QuizData quizSelection;

    [SerializeField] private TMP_Text question;
    [SerializeField] private GameObject optionPrefab;
    [SerializeField] private Transform optionContainer;
    [SerializeField] private FadeManager fadeManager;

    private QuizData.QuestionData currentQuestion;

    private QuizData.OptionData[] playerSelection;

    private void Start()
    {
        playerSelection = new QuizData.OptionData[PlayerDataManager.Instance.playerCount];
        quizSelection.RandomiseQuestions();
        currentQuestion = quizSelection.questionList.Dequeue();
        UpdateDisplay();
    }

    public void OnFadeIn()
    {
        StartCoroutine(EnableInput());
    }

    public void OnOptionSelected(int player, int option)
    {
        if (currentQuestion == null) return;
        
        playerSelection[player] = currentQuestion.options[option];

        if (playerSelection.Count(p => p != null) >= PlayerDataManager.Instance.playerCount)
        {
            ValidateOptions();
            currentQuestion = null;
            StartCoroutine(SelectNextQuestion());
        }
    }

    private void ValidateOptions()
    {
        for(int i = 0; i < playerSelection.Length; i++)
        {
            if (playerSelection[i].correctAnswer)
            {
                //TODO: Outline portrait?
                Debug.Log($"Player {i + 1} is correct");
            }

            playerSelection[i] = null;
        }
    }

    private void UpdateDisplay()
    {
        question.text = currentQuestion.question;

        //Adds new options
        if (currentQuestion.options.Count > optionContainer.childCount ||
            currentQuestion.options.Count < optionContainer.childCount)
        {
            for (int i = optionContainer.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(optionContainer.GetChild(i));
            }

            for (int i = 0; i < currentQuestion.options.Count; i++)
            {
                GameObject option = Instantiate(optionPrefab, optionContainer);
                OptionButton optButton = option.GetComponent<OptionButton>();
                optButton.UpdateOption(currentQuestion.options[i].text, i);
                optButton.quizManager = this;
            }
        }
        else
        {
            for (int i = 0; i < currentQuestion.options.Count; i++)
            {
                Transform child = optionContainer.GetChild(i);
                child.GetComponent<Image>().color = Color.white;
                OptionButton optButton = child.GetComponent<OptionButton>();
                optButton.UpdateOption(currentQuestion.options[i].text, i);
                optButton.quizManager = this;
            }
        }

        var mes = FindObjectsOfType<MultiplayerEventSystem>();
        foreach (var m in mes)
        {
            m.firstSelectedGameObject = optionContainer.GetChild(0).gameObject;
        }
    }

    private IEnumerator SelectNextQuestion()
    {
        ToggleOptionInteractivity(false);
        
        for (float t = 0.0f; t <= 2.0f; t += Time.deltaTime)
        {
            yield return null;
        }

        if (quizSelection.questionList.Count > 0)
        {
            currentQuestion = quizSelection.questionList.Dequeue();

            ToggleOptionInteractivity(true);
            UpdateDisplay();
        }
        else
        {
            fadeManager.FadeOut();
        }
    }

    private void ToggleOptionInteractivity(bool value)
    {
        if (optionContainer.childCount <= 0) return;

        for (int i = 0; i < optionContainer.childCount; i++)
        {
            OptionButton opt = optionContainer.GetChild(i).GetComponent<OptionButton>();

            opt.interactable = value;
        }
    }

    private IEnumerator EnableInput()
    {
        for (float t = 0.0f; t <= 1.5f; t += Time.deltaTime) yield return null;
        
        foreach (var uiPlayer in FindObjectsOfType<UIPlayerController>())
        {
            uiPlayer.EnableInput();
        }
    }

    public void OnFadeOut()
    {
        LevelManager.Instance.LoadNextScene();
    }
}
