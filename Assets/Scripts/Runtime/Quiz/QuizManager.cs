using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private QuizData quizSelection;

    [SerializeField] private TMP_Text question;
    [SerializeField] private GameObject optionPrefab;
    [SerializeField] private Transform optionContainer;
    
    private QuizData.QuestionData currentQuestion;

    private void Start()
    {
        quizSelection.RandomiseQuestions();
        currentQuestion = quizSelection.questionList.Dequeue();

        UpdateDisplay();
    }

    public void OnOptionSelected(int option)
    {
        if (currentQuestion == null) return;
        
        if (currentQuestion.options[option].correctAnswer)
        {
            optionContainer.GetChild(option).GetComponent<Image>().color = Color.green;

            currentQuestion = null;

            StartCoroutine(SelectNextQuestion());
        }
        else
        {
            optionContainer.GetChild(option).GetComponent<Image>().color = Color.red;
        }
    }

    private void UpdateDisplay()
    {
        question.text = currentQuestion.question;

        if (currentQuestion.options.Count > optionContainer.childCount ||
            currentQuestion.options.Count < optionContainer.childCount)
        {
            for (int i = 0; i < currentQuestion.options.Count; i++)
            {
                GameObject option = Instantiate(optionPrefab, optionContainer);
                OptionButton optButton = option.AddComponent<OptionButton>();
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
            gameObject.SetActive(false);
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
}
