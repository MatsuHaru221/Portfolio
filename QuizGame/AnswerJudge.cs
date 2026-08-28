using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerJudge : MonoBehaviour
{
    [SerializeField, Header("正誤の表示テキスト")]
    private TextMeshProUGUI _crossCircle;

    [SerializeField, Header("子オブジェクトの問題文取得")]
    private TextMeshProUGUI _selfQuestionOption;

    [SerializeField, Header("他ボタンのアタッチ")]
    private List<Button> _answerButton = new List<Button>();

    [SerializeField, Header("Nextボタンのアタッチ")]
    private GameObject _nextButton;

    public void Judge()
    {
        _crossCircle.gameObject.SetActive(true);

        if(_selfQuestionOption.text == GameManager.questionArray[GameManager.currentQuestion, 5])
        {
            _crossCircle.text = "○";
            _crossCircle.color = Color.red;
        }
        else
        {
            _crossCircle.text = "X";
            _crossCircle.color = Color.blue;
        }

        for (int i = 0; i < _answerButton.Count; i++)
        {
            _answerButton[i].interactable = false;
        }

        _nextButton.SetActive(true);
    }
}
