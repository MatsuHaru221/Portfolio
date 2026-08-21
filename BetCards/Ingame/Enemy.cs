using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] HandManager _handManager;
    BetManager _betManager;

    //NPCが考えてる風に見えるような待ち時間
    [SerializeField] float _thinkTimeSeconds = 1f;

    //カードを出そうとしているかどうか
    private bool _isThinking = false;

    //勝利数とベット数を予想したかどうか
    private bool _isSettled = false;

    private void Start()
    {
        //_handManager = new HandManager();
        _handManager._isEnemy = true;
        _betManager = BetManager.Instance;


        //リセットイベントの処理を登録
        MainManager.Instance.RegisterStartBetPhaseEvent(HandleGameReset);
    }

    void Update()
    {
        //自身のターンか監視
        if(MainManager.Instance.IsMyTurn(_handManager.PlayerId) is true && _isThinking is false)
        {
            ThinkAndPlayCardAsync().Forget();
        }

        if(MainManager.Instance.CurrentPhase == Phase.Bet && _isSettled is false)
        {
            DecideAndPlaceBet();
        }
    }

    // /// <summary>
    // /// この関数が呼ばれたらランダムなカードを選んで出します
    // /// </summary>
    // public void PlayCard()
    // {
    //     _handManager.SelectCard(Random.Range(1, _handManager.CardAmount + 1));
    //     _handManager.SendCard();
    // }

    async UniTask ThinkAndPlayCardAsync()
    {
        //カードを出そうと考えているフラグを立てる
        _isThinking = true;

        // ちょっと考えてるっぽい待ち時間
        await UniTask.Delay(TimeSpan.FromSeconds(_thinkTimeSeconds));

        // 出すカードを決める（カードを選ぶアルゴリズム）
        //ジョーカーがあったら一番最初に出す 無かったらランダム
        bool isHaveJoker = false; //ジョーカーを持っていたかどうかのフラグ
        for(int i = 0; i < _handManager.HandCardLists.Count; i++)
        {
            if(_handManager.HandCardLists[i].CurrentMark == Mark.Joker)
            {
                //ジョーカーを持っているフラグを立ててジョーカーを選択する
                isHaveJoker = true;
                _handManager.SelectCard(i);
                break;
            }
        }

        //ジョーカーがなかったらランダムに選ぶ
        if(isHaveJoker is false)
            _handManager.SelectCard(UnityEngine.Random.Range(0, _handManager.CardAmount));

        //選んだカードを出す
        _handManager.SendCard();

        //フラグを下す
        _isThinking = false;
    }

    public void DecideAndPlaceBet()
    {
        //決めたというフラグを立てる
        _isSettled = true;

        //ここで予測
        //暫定的に今は無し
        //反映
        _betManager.SetWinAndBet(_handManager.PlayerId, 1, 100);
    }

    //リセット時の処理
    private void HandleGameReset()
    {
        //予想を決めたフラグを下す
        _isSettled = false;
    }
}
