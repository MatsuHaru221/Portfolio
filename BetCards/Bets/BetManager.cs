using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BetData
{
    //予想した勝利数
    public int WinAmount = 0;
    //賭けた金額
    public int BetAmount = 0;
}

public class BetManager : MonoBehaviour
{
    public static BetManager Instance;

    //ベットしたデータを入れておく変数
    private List<BetData> _betDataLists = new List<BetData>();
    public List<BetData> BetDataLists => _betDataLists;

    //それぞれのプレイヤーの所持金
    private List<int> _playerChipLists = new List<int>();
    public List<int> PlayerChipLists => _playerChipLists;
    //それぞれのプレイヤーの勝利数
    private List<int> _playerWinCountLists = new List<int>();
    public List<int> PlayerWinCountLists => _playerWinCountLists;

    //プレイヤーの初期所持金
    [SerializeField] int _initialPlayerChips = 1000;

    //掛け金と勝利数を決めたプレイヤーの数
    private int _settledPlayerCount = 0;
    public int SettledPlayerCount => _settledPlayerCount;

    //すべてのプレイヤーが掛け金と勝利数を決め終わったかどうか
    private List<bool> _playerSettledList = new List<bool>();
    public List<bool>  PlayerSettledList => _playerSettledList;

    void Awake()
    {
        //すでにインスタンスがあり、かつそのインスタンスが自分でない場合に自壊する処理
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        //自分自身をインスタンスにする
        Instance = this;
    }

    void OnDestroy()
    {
        //自分自身がインスタンス化されていて、破壊された時にInstanceを解放
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        //初期所持金の設定
        for(int i = 0; i < 3; i++)
        {
            _playerChipLists.Add(_initialPlayerChips);
            _playerWinCountLists.Add(0);
            _betDataLists.Add(new BetData());
            _playerSettledList.Add(false);
        }

        //Debug.Log(_playerChipLists.Count);
    }

    public void Reset()
    {
        _settledPlayerCount = 0;
        _betDataLists = new List<BetData>();
        _playerWinCountLists = new List<int>();
        _playerSettledList = new List<bool>();

        for(int i = 0; i < 3; i++)
        {
            _playerWinCountLists.Add(0);
            _betDataLists.Add(new BetData());
            _playerSettledList.Add(false);
        }
    }

    public void SetWinAndBet(int playerId, int winNum, int betNum)
    {
        BetData betData = new BetData
        {
            WinAmount = winNum,
            BetAmount = betNum
        };

        _betDataLists[playerId - 1] = betData;
        _playerSettledList[playerId - 1] = true;

        Debug.Log($"{MainManager.Instance.CurrentPhase}, {_playerSettledList[0]}, {_playerSettledList[1]}, {_playerSettledList[2]}");
    }

    //指定されたプレイヤーの勝利数を増やす
    public void AddPlayerWinCount(int playerId)
    {
        _playerWinCountLists[playerId - 1] += 1;
    }

    public void SettlePredictedBets()
    {
        //賭けられたチップの合計
        int totalChipsBet = 0;
        for(int i = 0; i < 3; i++)
            totalChipsBet += _betDataLists[i].BetAmount;
        
        for(int i = 0; i < 3; i++)
        {
            //予想が合っていたプレイヤーに場の合計チップをそのまま足す
            if(_betDataLists[i].WinAmount == _playerWinCountLists[i])
            {
                _playerChipLists[i] += totalChipsBet;
            }
            //予想が外れてたら賭けた分をそのまま引く
            else
            {
                _playerChipLists[i] -= _betDataLists[i].BetAmount;
            }
        }

    }
}
