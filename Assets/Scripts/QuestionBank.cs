// using UnityEngine;
// using System.Collections.Generic;
// using System.IO;
// using UnityEngine.UI;
// using System.Text.Json;

// public class QuestionBank : MonoBehaviour
// {
//     public string questionText;
//     public string categoryText;

//     private List<Category> categories;

//     void Start()
//     {
//         LoadQuestionsFromJSON();
//         GenerateRandomQuestion();
//     }

//     void LoadQuestionsFromJSON()
//     {
//         string path = Path.Combine(Application.streamingAssetsPath, "questions.json");

//         if (File.Exists(path))
//         {
//             string jsonContent = File.ReadAllText(path);
//             categories = JsonSerializer.Deserialize<List<Category>>(jsonContent);
//         }
//         else
//         {
//             Debug.LogError("Questions JSON file not found at path: " + path);
//         }
//     }

//     void GenerateRandomQuestion()
//     {
//         if (categories != null && categories.Count > 0)
//         {
//             int randomCategoryIndex = Random.Range(0, categories.Count);
//             Category randomCategory = categories[randomCategoryIndex];

//             int randomQuestionIndex = Random.Range(0, randomCategory.questions.Count);
//             string randomQuestion = randomCategory.questions[randomQuestionIndex];

//             // questionText.text = randomQuestion;
//             // categoryText.text = randomCategory.name;
//             Debug.Log(randomQuestion);
//             Debug.Log(randomCategory.name);
//         }
//         else
//         {
//             Debug.LogWarning("No categories loaded. Make sure the JSON file is properly formatted and contains data.");
//         }
//     }
// }

// [System.Serializable]
// public class Category
// {
//     public string name;
//     public List<string> questions;
// }
