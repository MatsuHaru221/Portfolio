using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class ScoreManager : MonoBehaviour
{   
    [SerializeField] private TextMeshProUGUI _playerTotalScoreText;
    [SerializeField] private TextMeshProUGUI _enemyTotalScoreText;
    [SerializeField] public bool _isP1Won = true;

    private int _playerTotalScore;
    private int _enemyTotalScore;
    private List<int> _playerScoreList = new List<int>();
    private List<int> _enemyScoreList = new List<int>();

    private void Awake()
    {
        // _isP1Won = StaticGameData.GetIsWin;

        // デバッグ用
        // _playerScoreList.AddRange(new int[] { 100, 200, 500 });
        // _enemyScoreList.AddRange(new int[] { 200, 500, 500 });

        // 本番用
         _playerScoreList = StaticGameData.GetPlayerScore;
         _enemyScoreList = StaticGameData.GetEnemyScore;

        ScoreCalculate().Forget();
    }

    /// <summary>
    /// プレイヤーとエネミーのスコアのそれぞれの合算
    /// また、プレイヤーの勝敗決定
    /// </summary>
    private async UniTask ScoreCalculate() 
    {
        for(int i = 0;  i < _playerScoreList.Count; i++)
        {
            _playerTotalScore += _playerScoreList[i]; 
        }

        for(int i = 0; i < _enemyScoreList.Count; i++)
        {
            _enemyTotalScore += _enemyScoreList[i];
        }
        // プレイヤーのトータルスコアが敵のトータルスコアより大きかったらプレイヤーの勝利
         _isP1Won = (_playerTotalScore > _enemyTotalScore);
        await UniTask.WhenAll();
    }
}
