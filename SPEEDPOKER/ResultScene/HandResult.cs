using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class HandResult : MonoBehaviour
{
    [SerializeField, Header("上がったときの手札のリスト")]
    private List<Image> _handCardList = new List<Image>();

    [SerializeField, Header("上がった役の名前")]
    private TextMeshProUGUI _handNameText;

    [SerializeField, Header("上がった役のスコア表示")]
    private RectTransform _handScoreTextContainer;
    [SerializeField] private GameObject _handScorePrefab;
    [SerializeField] private float _handScoreInterval = 0.1f;
    [SerializeField] private float _slideDuration = 0.5f;
    [SerializeField] private float XOffset;

    [SerializeField, Header("表示する必要があるかどうか")]
    private bool _isShow = true;

    [SerializeField, Header("このUIが上から何番目か")]
    private int _listNum = 1;

    [SerializeField] private bool _isP1 = true;

    [SerializeField]private float _XOffsetStart;

    private void Awake()
    {
        CheckDisplayOrNot();
    }

    private void Start()
    {
        int ScorePoint = 0;
        int HandName = 0;
        if (_isP1)
        {
            ScorePoint = StaticGameData.GetPlayerScore[_listNum - 1];
            HandName = StaticGameData.s_wonPlayerHand[_listNum - 1];
        }
        else
        {
            ScorePoint = StaticGameData.GetEnemyScore[_listNum - 1];
            HandName = StaticGameData.s_wonEnemyHand[_listNum - 1];
        }

        ChangeHandCards(ScorePoint, HandName);
    }

    private void Update()
    {
        this.gameObject.SetActive(_isShow);
    }

    /// <summary>
    /// 上がったときの手札の反映
    /// </summary>
    /// <param name="handPoints">上がった役のスコア</param>
    /// <param name="handNameIndex">上がった役のIndex</param>
    private void ChangeHandCards(int handPoints, int handNameIndex)
    {
        // 役の名前
        string handName = HandName(handNameIndex);
        _handNameText.text = handName;
        ScoreTextAnimation(handPoints).Forget();
        // _handScoreText.text = $"{handPoints}";
    }

    /// <summary>
    /// スコアテキストのアニメーション
    /// </summary>
    /// <param name="point">アニメーションさせるスコアの値</param>
    private async UniTaskVoid ScoreTextAnimation(int point)
    {
        string s = point.ToString();

        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];

            var textObj = Instantiate(_handScorePrefab, _handScoreTextContainer, false);
            TextMeshProUGUI textMesh = textObj.GetComponent<TextMeshProUGUI>();
            textMesh.text = c.ToString();

            RectTransform rt = textObj.GetComponent<RectTransform>();

            // 文字の幅を取得
            float charWidth = textMesh.preferredWidth;
            
            // startXの値が大きいほど右からスタート
            float startX = _XOffsetStart + charWidth;
            rt.anchoredPosition = new Vector2(startX, 0f);

            // スライドイン
            float targetX = _XOffsetStart;  // 次の文字の基準位置
            rt.DOAnchorPosX(targetX, _slideDuration).SetEase(Ease.OutCubic);

            // 次の文字の位置を更新
            _XOffsetStart += charWidth;

            await UniTask.Delay(System.TimeSpan.FromSeconds(_handScoreInterval));
        }
    }

    private void CheckDisplayOrNot()
    {
        if (StaticGameData.s_wonPlayerHand[_listNum - 1] == 0)
        {
            _isShow = false;
        }
        else
        {
            _isShow = true;
        }
    }

    /// <summary>
    /// 上がった役の名前を取得
    /// </summary>
    /// <param name="handIndex">上がった役のIndex</param>
    /// <returns>上がった役の名前の string </returns>
    private string HandName(int handIndex)
    {
        string handName;
        switch (handIndex)
        {
            case 0:
                handName = "None";
                break;
            case 1:
                handName = "OnePair";
                break;
            case 2:
                handName = "TwoPair";
                break;
            case 3:
                handName = "ThreeCard";
                break;
            case 4:
                handName = "Straight";
                break;
            case 5:
                handName = "Flush";
                break;
            case 6:
                handName = "FullHouse";
                break;
            case 7:
                handName =  "StraightFlush";
                break;
            case 8:
                handName = "RoyalStraightFlush";
                break;
            default:
                handName = "Unknown";
                break;
        }
        return handName;
    }
}
