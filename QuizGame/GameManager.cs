using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField, Header("問題数を表示するテキストを選択")]
    private TextMeshProUGUI _questionNum;

    [SerializeField, Header("問題を表示するテキストを選択")]
    private TextMeshProUGUI _questionTMP;

    [SerializeField, Header("解答に表示するテキストを選択")]
    private List<TextMeshProUGUI> _questionList = new List<TextMeshProUGUI>();

    [SerializeField, Header("選択肢のボタンを選択")]
    private List<Button> _answerButton = new List<Button>();

    [SerializeField, Header("選択肢の正誤判定テキストを選択")]
    private List<TextMeshProUGUI> _answerJudgeText = new List<TextMeshProUGUI>();

    [SerializeField, Header("Nextボタンの選択")]
    private GameObject _nextButton;

    public static string[,] questionArray = new string[8, 6]
    {
        { "what is the highest mountain in the world?", "A:Mt.Fuji", "B:Everest", "C:K2", "D:Mt.RAINIER", "B:Everest"},
        { "Where is the capital of Japan?", "A:Osaka", "B:Nagoya", "C:Tokyo", "D:Kyoto", "C:Tokyo"},
        { "How many continents are there on Earth?", "A:5", "B:6", "C:7", "D:8", "C:7"},
        { "What is the largest planet in our solar system?", "A: Jupiter", "B: Earth", "C: Mars", "D: Venus", "A: Jupiter"},
        {"What is the color of the sky on a clear day?", "A: Blue", "B: Red", "C: Green", "D: Yellow", "A: Blue" },
        {"What is the capital of France?", "A: Paris", "B: Berlin", "C: London", "D: Madrid", "A: Paris" },
        {"What is the chemical symbol for water?", "A: H2O", "B: CO2", "C: O2", "D: NaCl", "A: H2O" },
        {"What is the currency of the United States?", "A: Dollar", "B: Euro", "C: Yen", "D: Pound", "A: Dollar" }
    };

    public static int currentQuestion = 0;

    private void Start()
    {
        MakeQuestionText();
    }

    private void MakeQuestionText()
    {
        _questionTMP.text = questionArray[currentQuestion, 0];
        for (int i = 0; i < questionArray.Length; i++)
        {
            _questionList[i].text = questionArray[currentQuestion, i + 1];
        }
    }

    public void NextQuestion()
    {
        _nextButton.SetActive(false);
        _questionNum.text = $"Question { currentQuestion + 2}";
        for (int i = 0; i < _answerButton.Count; i++)
        {
            _answerButton[i].interactable = true;
        }
        for(int i = 0; i < _answerJudgeText.Count; i++)
        {
            _answerJudgeText[i].gameObject.SetActive(false);
        }
        currentQuestion++;
        MakeQuestionText();
    }
}
