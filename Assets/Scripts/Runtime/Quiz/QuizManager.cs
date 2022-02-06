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
    [SerializeField] private CanvasGroup questionGroup;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private AudioSource resultAudio;

    private QuizData.QuestionData currentQuestion;

    private QuizData.OptionData[] playerSelection;

    private void Start()
    {
        playerSelection = new QuizData.OptionData[PlayerDataManager.Instance.playerCount];
        quizSelection.RandomiseQuestions();
        currentQuestion = quizSelection.questionList.Dequeue();
        UpdateDisplay();
    }

    public void OnGameStart()
    {
        StartCoroutine(FadeInQuestions());
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
        if (playerSelection[0].correctAnswer)
        {
            scoreManager.TryScore(true);
        }
        else
        {
            resultAudio.Play();
        }

        playerSelection[0] = null;

        for (int i = 0; i < currentQuestion.options.Count; i++)
        {
            Transform child = optionContainer.GetChild(i);
            OptionButton btn = child.GetComponentInChildren<OptionButton>();
            btn.targetGraphic.color = currentQuestion.options[i].correctAnswer ? Color.green : Color.red;
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
                optButton.UpdateOption(currentQuestion.options[i], i);
                optButton.quizManager = this;
            }
        }
        else
        {
            for (int i = 0; i < currentQuestion.options.Count; i++)
            {
                Transform child = optionContainer.GetChild(i);
                OptionButton optButton = child.GetComponent<OptionButton>();
                optButton.targetGraphic.color = Color.white;
                optButton.UpdateOption(currentQuestion.options[i], i);
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

        for (int i = 0; i < optionContainer.childCount; i++)
        {
            optionContainer.GetChild(i).GetComponentInChildren<OptionButton>().targetGraphic.color = Color.white;
        }

        if (quizSelection.questionList.Count > 0)
        {
            currentQuestion = quizSelection.questionList.Dequeue();

            ToggleOptionInteractivity(true);
            UpdateDisplay();
        }
        else
        {
            StartCoroutine(GoForFade());
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

    private IEnumerator GoForFade()
    {
        FindObjectOfType<AudioManager>().OnGameComplete();
        
        for (float t = 0.0f; t <= 5.0f; t += Time.deltaTime) yield return null;

        fadeManager.FadeOut();
    }

    private IEnumerator FadeInQuestions()
    {
        for (float t = 0.0f; t <= 1.25f; t += Time.deltaTime)
        {
            questionGroup.alpha = Mathf.Lerp(0.0f, 1.0f, 1 / 1.0f);
            yield return null;
        }

        questionGroup.interactable = true;
        questionGroup.blocksRaycasts = true;
    }
}
