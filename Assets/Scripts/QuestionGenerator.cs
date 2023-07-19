using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class QuestionGenerator : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI questionText;
    [SerializeField] private string questionBankPath = "Assets/bank.json";
    private string questionBankJson;
    private Dictionary<string, List<string>> questionBank;


    public enum QuestionType
    {
        Personality,
        Interests,
        Social,
        Habits,
        Career,
    }

    private void Start()
    {
        if (questionBankPath != string.Empty)
        {
            questionBankJson = File.ReadAllText(questionBankPath);
        }
        else {
            Debug.LogError("No file path specified: question bank");
        }

        questionBank = LoadQuestions();

    }

    private Dictionary<string, List<string>> LoadQuestions()
    {
        Dictionary<string, List<string>> QuestionBank = new Dictionary<string, List<string>>();

        // Create a class to match the JSON structure
        QuestionBank bank = JsonUtility.FromJson<QuestionBank>(questionBankJson);


        foreach (Category category in bank.categories)
        {
            List<string> tempQuestions = new List<string>();
            foreach (string question in category.questions)
            {
                tempQuestions.Add(question);
            }
            QuestionBank.Add(category.name, tempQuestions);
        }
        // Convert the object to a dictionary

        return QuestionBank;
    }

    public Question RandomQuestions() {
        List<string> _categories = new List<string>();
        foreach (var key in questionBank.Keys) {
            _categories.Add(key);
        }

        // pick a random category
        string category = _categories[Random.Range(0, _categories.Count)];
        var questionList = questionBank[category];
        string randomQuestion = questionList[Random.Range(0, questionList.Count)];
        return new Question(category, randomQuestion);
    }

    public void Test() {
        questionText.text = RandomQuestions().question;
    }
}

[System.Serializable]
public class Question {

    public Question(string category, string question)
    {
        this.category = category;
        this.question = question;
    }
    public string category;
    public string question;
}

[System.Serializable]
public class Category
{
    public string name;
    public List<string> questions;
}

[System.Serializable]
public class QuestionBank
{
    public List<Category> categories;
}
