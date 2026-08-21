using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using System.Collections.Concurrent;
using System.Transactions;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField] private bool _isPlayer1Score = true; //プレイヤーのスコアかどうか判断する変数
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private float _scoreDuration = 2;
    [SerializeField] private Image _winTextImage;
    [SerializeField] private List<Sprite> _winTextImages = new List<Sprite>();
    [SerializeField] private Image _winCharaImage;
    [SerializeField] private List<Sprite> _winCharaSprites = new List<Sprite>();
    [SerializeField] private int _handNameIndex = 0;


    void Start()
    {
        // ここで StaticGameData から _handNameIndex に勝った役のIndexの最大値を入れる
        if (_isPlayer1Score)
        {
            _handNameIndex = StaticGameData.s_wonPlayerHand.Max();
        }
        else
        {
            _handNameIndex = StaticGameData.s_wonEnemyHand.Max();
        }

        if (_isPlayer1Score)
        {
            // プレイヤーの勝利判定のテキスト
            if (StaticGameData.GetIsWin is true)
                _winTextImage.sprite = _winTextImages[0];
            else
                _winTextImage.sprite = _winTextImages[1];
        }
        else
        {
            // プレイヤーの勝利判定のテキスト
            if (StaticGameData.GetIsWin is false)
                _winTextImage.sprite = _winTextImages[0];
            else
                _winTextImage.sprite = _winTextImages[1];
        }
        // Debug.Log(_handNameIndex);
        SetCharacterImage(_handNameIndex);

        // プレイヤーのスコアの和表示
        List<int> _scoreList;
        if(_isPlayer1Score is true)
            _scoreList = StaticGameData.GetPlayerScore;
        else 
            _scoreList = StaticGameData.GetEnemyScore;

        ScoreAnimation(0,_scoreList[0] + _scoreList[1] + _scoreList[2]);
        // _scoreText.text = $"{_scoreList[0] + _scoreList[1] + _scoreList[2]}";
    }

    private void ScoreAnimation(int score, int to)
    {
        int currentValue = score;

        DOTween.To(() => currentValue, x => {
            currentValue = x;
            _scoreText.text = currentValue.ToString("F0"); // カンマ区切り
        }, to, _scoreDuration).SetEase(Ease.OutQuad);
    }

    private void SetCharacterImage(int handNameIndex)
    {
        if(handNameIndex == 0)
        {
            _winCharaImage.gameObject.GetComponent<Image>().enabled = false;
        }
        else
        {
            // Debug.Log(_handNameIndex);
            _winCharaImage.sprite = _winCharaSprites[handNameIndex - 1];
        }
    }
}
