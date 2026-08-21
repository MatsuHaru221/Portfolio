using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using System;

public class StandByManager : MonoBehaviour
{
    [SerializeField] private bool _isDuel;

    [SerializeField, Header("プレイヤー1のコイン")]
    private List<GameObject> _playerCoins = new List<GameObject>();
    [SerializeField, Header("プレイヤー1のレディー状態")]
    private bool _isPlayerReady = false;
    [SerializeField, Header("プレイヤー1の最終的なコインの位置")]
    private List<GameObject> _playerEndCoinPos = new List<GameObject>();
    [SerializeField] FadeSceneLoder _fadeSceneLoder;

    [SerializeField, Header("プレイヤー2のコイン")]
    private List<GameObject> _enemyCoins = new List<GameObject>();
    [SerializeField, Header("プレイヤー2のレディー状態")]
    private bool _isEnemyReady = false;
    [SerializeField, Header("プレイヤー2の最終的なコインの位置")]
    private List<GameObject> _enemyEndCoinPos = new List<GameObject>();
    [SerializeField, Header("遷移先のシーン名")]
    private string _sceneName;

    [SerializeField, Header("コインの移動速度")]
    private float _coinMoveSpeed = 1;
    [SerializeField] private GameObject _p1ClickToStart;
    [SerializeField] private GameObject _p2ClickToStart;

    [SerializeField] float _delayTime;

    private bool _doingPlayerAnim = false;
    private bool _doingEnemyAnim = false;

    private void Start()
    {
        if(!_isDuel)_isEnemyReady = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
                _isPlayerReady = true;
                _p1ClickToStart.SetActive(false);
        }

        if(_isPlayerReady && !_doingPlayerAnim)
        {
            PlayerCoinAnimation();
        }

        if (Input.GetMouseButtonDown(1))
        {
                _isEnemyReady = true;
                _p2ClickToStart.SetActive(false);
        }

        if (_isEnemyReady && !_doingEnemyAnim)
        {
            EnemyCoinAnimation();
        }


        if(_isPlayerReady && _doingPlayerAnim && _isEnemyReady && _doingEnemyAnim)
        {
            Load().Forget();
        }
    }

    private async UniTask Load()
    {
        var Token = this.GetCancellationTokenOnDestroy();
        await UniTask.Delay(TimeSpan.FromSeconds(_delayTime), cancellationToken: Token);
        _fadeSceneLoder.CallFadeAndLoad(_sceneName);

    }


    private void PlayerCoinAnimation()
    {
        if (_isPlayerReady) { _doingPlayerAnim = true; }
        SoundManager.Instance.PlaySE(3);
        for (int i = 0; i < _playerCoins.Count; i++) 
        {
            _playerCoins[i].transform.DOMove(new Vector3(_playerEndCoinPos[i].transform.position.x,
                _playerEndCoinPos[i].transform.position.y, _playerEndCoinPos[i].transform.position.z), _coinMoveSpeed).SetEase(Ease.InOutQuad);
        }
    }


    private void EnemyCoinAnimation()
    {
        if (_isEnemyReady) { _doingEnemyAnim = true; }
        SoundManager.Instance.PlaySE(3);
        for (int i = 0; i < _enemyCoins.Count; i++)
        {
            _enemyCoins[i].transform.DOMove(new Vector3(_enemyEndCoinPos[i].transform.position.x,
                _enemyEndCoinPos[i].transform.position.y, _enemyEndCoinPos[i].transform.position.z), _coinMoveSpeed).SetEase(Ease.InOutQuad);
        }
    }

}
