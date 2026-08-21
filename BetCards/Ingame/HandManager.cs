using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

public class HandManager : MonoBehaviour
{
    [Foldout("手札の配置用")]
    [SerializeField, Label("手札を表示する円の大きさ")]
    private float _handCardRadius = 1;
    [Foldout("手札の配置用")]
    [SerializeField, Label("手札を表示する角度の最小値")]
    private float _handCardMinAngle = 60;
    [Foldout("手札の配置用")]
    [SerializeField, Label("手札を表示する角度の最大値")]
    private float _handCardMaxAngle = 120;

    //DeckManagerから配られるカードの位置
    [SerializeField] Transform _stagingPoint;
    public Vector3 StargingPosition => _stagingPoint.position;

    //手札を配置する時の基準となる中心座標
    [SerializeField] Transform _handCenter;
    public Vector3 HandCenterPositon => _handCenter.position;

    // 手札のカードデータ保存用
    //private List<CardData> _handCardDataLists = new List<CardData>();
    // 手札のカード保存用
    private List<Card> _handCardLists = new List<Card>();
    public List<Card> HandCardLists => _handCardLists;

    //プレイヤーを判別するためのID
    private int _playerId;
    public int PlayerId { get { return _playerId; } set { _playerId = value; } }

    // 手札の枚数
    private int _cardAmount = 0;
    public int CardAmount => _cardAmount;

    //現在の手札の最大枚数を入れる変数
    private int _maxHandCardCount;
    public int MaxHandCardCount => _maxHandCardCount;

    // 何番目のカードが選ばれているか
    private int _selectCardIndex;
    //選ばれるCard
    private Card _selectCard;

    //ジョーカーがあるかどうか 持ってる時はtrue 持ってない時はfalse
    private bool _isHaveAJoker = false;

    // エネミー用かの判別
    [HideInInspector] public bool _isEnemy = false;

    void Start()
    {
        //カードの総数を設定
        _maxHandCardCount = MainManager.Instance.CardsPerRound;

        //プレイフェーズが始まった時のイベントを登録
        MainManager.Instance.RegisterStartPlayPhaseEvent(JokerCheck);
    }

    /// <summary>
    /// 手札にカード情報を与える、追加する関数
    /// </summary>
    /// <param name="cardData">追加するカードの情報,CardData</param>
    /// <param name="card">追加するカード</param>
    public void AddCardData(CardData cardData, Card card)
    {
        //手札の枚数を増やす
        _cardAmount++;
        // ここで _handCardDataLists に手札のカード情報を入れる
        //_handCardDataLists.Add(cardData);
        // ここで _handCardLists に手札のカードを入れる
        _handCardLists.Add(card);
    }

    /// <summary>
    /// カード選択時の処理
    /// </summary>
    /// <param name="index">選ぶカードのindex</param>
    public void SelectCard(int index)
    {
        //ジョーカーを持っている場合はそれ以外を選択できないようにする
        if(_isHaveAJoker is true) return;

        //選択したカード以外を選択されていない状態にする
        for(int i = 0; i < _handCardLists.Count; i++)
        {
            if(_handCardLists[i] != _handCardLists[index])
                _handCardLists[i].UnSelected();
        }

        _selectCard = _handCardLists[index];
        _selectCard.Selected();
        _selectCardIndex = index;
    }

    /// <summary>
    /// カードの情報を送る処理
    /// </summary>
    public void SendCard()
    {
        if (_selectCard == null || _handCardLists.Count < _selectCardIndex) return;

        // ここで場にカードのデータを送る _handCardDataListsの中の _selectCardIndex を指定すればいいはず
        MainManager.Instance.RequestPlayCard(this, _handCardLists[_selectCardIndex]);
        
        //ジョーカーフラグの解除
        _isHaveAJoker = false;

        //手札を少なくする処理
        DecreaseCardAmount();
    }

    /// <summary>
    /// 手札の枚数を減らす処理
    /// </summary>
    private void DecreaseCardAmount()
    {
        //カードのデータを削除する
        _handCardLists.RemoveAt(_selectCardIndex);

        //選んだカードとIndexをリセット
        _selectCard = null;
        _selectCardIndex = -1;

        //現在のカードの枚数を減らす
        _cardAmount--;
        //手札の最大枚数を減らす
        _maxHandCardCount--;

        //カードの位置を変更する
        CardPositionChanger();
    } 

    /// <summary>
    /// カードの枚数によって変わるカードのポジションセット
    /// </summary>
    /// <param name="index">何番目のカードか</param>
    private Vector3 SetPositionByIndex(int index)
    {
        if (_maxHandCardCount <= 0)
        {
            Debug.LogError("オブジェクト数が0以下です");
            return transform.position;
        }

        // indexを範囲内に収めるためclamp
        //index = Mathf.Clamp(index, 0, _maxHandCardCount - 1);
        float totalAngle = _handCardMaxAngle - _handCardMinAngle;
        //Debug.Log(totalAngle);

        float angle;
        // 1枚だけなら中央角度
        if (_maxHandCardCount == 1)
        {
            angle = (_handCardMinAngle + _handCardMaxAngle) / 2f;
        }
        // 均等割りの角度
        else
        {
            float step = totalAngle / (_maxHandCardCount + 1);
            angle = _handCardMinAngle + step * index;
            //Debug.Log($"angle = {angle}, step = {step}");
        }

        // 座標計算
        float rad = Mathf.Deg2Rad * angle;
        float x = Mathf.Cos(rad) * _handCardRadius;
        float y = Mathf.Sin(rad) * _handCardRadius;

        // 自身の位置を中心にした座標を返す
        return _handCenter.position + new Vector3(x, y, _cardAmount);
    }

    /// <summary>
    /// 手札カードのポジションを返す関数です
    /// </summary>
    /// <returns>Vector3型のカードポジションを返します</returns> <summary>
    public Vector3 GetPosistionByIndex()
    {
        return SetPositionByIndex(_cardAmount);
    }
    
    /// <summary>
    /// カードの枚数が少なくなった時に手札を詰める処理
    /// </summary>
    private void CardPositionChanger()
    {
        for(int i = 0; i < _cardAmount; i++)
        {
            Vector3 newPosition = SetPositionByIndex(i + 1);
            _handCardLists[i].UnSelected();
            _handCardLists[i].MoveToHandSlotAsync(newPosition);
        }
    }

    public void Reset()
    {
        _maxHandCardCount = MainManager.Instance.CardsPerRound;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)_handCenter.position, _handCardRadius);
    }

    public void SearchMyHandCard(Card card)
    {
        for(int i = 0; i < _handCardLists.Count; i++)
        {
            if(_handCardLists[i] == card)
            {
                SelectCard(i);
                break;
            }
        }
    }

    //手札のカード全てを何も選択されていない状態(光っている状態)にする処理
    public void FadeInAllCard()
    {
        for(int i = 0; i < _handCardLists.Count; i++)
        {
            _handCardLists[i].ColorFadeIn();
        }
    }

    //手札にジョーカーがあるかをチェックし、あった場合はジョーカー以外のカードを選択できないようにする
    public void JokerCheck()
    {
        for(int i = 0; i < _handCardLists.Count; i++)
        {
            if(_handCardLists[i].CurrentMark == Mark.Joker)
            {
                _isHaveAJoker = true;
                _selectCard = _handCardLists[i];
                _selectCard.Selected();
                _selectCardIndex = i;

                for(int j = 0; j < _handCardLists.Count; j++)
                {
                    if(_handCardLists[j] != _selectCard)
                        _handCardLists[j].UnSelected();
                }

                break;
            }
        }
    }
}
