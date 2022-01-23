using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(menuName = "Keystone/Quiz Data")]
public class QuizData : ScriptableObject
{
    [Serializable]
    public class QuestionData
    {
        public string question;
        public List<OptionData> options;
    }

    [Serializable]
    public class OptionData
    {
        public string text;
        public bool correctAnswer;
    }

    [SerializeField] private List<QuestionData> questions;

    public Queue<QuestionData> questionList;

    public void RandomiseQuestions()
    {
        questionList ??= new Queue<QuestionData>();
        
        questionList.Clear();
        Random rnd = new Random();

        List<QuestionData> tempList = CopyQuestions(questions);

        tempList = tempList.OrderBy(q => rnd.Next()).ToList();

        foreach (var question in tempList)
        {
            question.options = question.options.OrderBy(o => rnd.Next()).ToList();
            questionList.Enqueue(question);
        }
    }

    private List<QuestionData> CopyQuestions(List<QuestionData> questions)
    {
        List<QuestionData> result = new List<QuestionData>();

        for (int i = 0; i < questions.Count; i++)
        {
            result.Add(new QuestionData
            {
                question = questions[i].question,
                options = CopyOptions(questions[i].options)
            });
        }

        return result;
    }

    private List<OptionData> CopyOptions(List<OptionData> options)
    {
        List<OptionData> result = new List<OptionData>();

        for (int i = 0; i < options.Count; i++)
        {
            result.Add(new OptionData
            {
                text = options[i].text,
                correctAnswer = options[i].correctAnswer
            });
        }

        return result;
    }
}