using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveWithProgress : MonoBehaviour
{
    [SerializeField, Header("エネミーオブジェクト")]
    private GameObject _enemy = null;

    [SerializeField, Header("進行度の値の拡大率")]
    private float _progressPerunit = 10f;

    [SerializeField, Header("進行度の差がいくつだったら表示するか")]
    private float _displayEnemyDuration = 100f;

    [SerializeField, Header("進行度管理スクリプト")]
    private ProgressUI _progressUI;

    [SerializeField, Header("エネミーの座標の基準となる場所")]
    private Transform _enemyPosBase = null;

    [SerializeField]
    // エネミーとプレイヤーの進行度の差
    private float _progressGap = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _progressGap = _progressUI.EnemyProgress - _progressUI.PlayerProgress;
        if (_progressGap > _displayEnemyDuration)
        {
            _enemy.SetActive(false);
        }
        else
        {
            _enemy.transform.position = new Vector3(_enemyPosBase.position.x + _progressGap / _progressPerunit, _enemy.transform.position.y, _enemy.transform.position.z);
            _enemy.SetActive(true);
        }
    }
}
