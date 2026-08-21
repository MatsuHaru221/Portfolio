using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI: MonoBehaviour
{
    [SerializeField, Header("プレイヤーの進行度表示スライダー")] Slider _progressSlider;
    [SerializeField, Header("エネミーの進行度表示スライダー")] Slider _progressSliderEnemy;
    [SerializeField, Header("SpeedManager")] SpeedManager _speedManager;
    [SerializeField, Header("エネミーのデフォルトの進行度")] private float _enemyDefLength;
    [SerializeField, Header("ゴールの値")] float _goalLength;
    

    // プレイヤーの現在の進行度
    private float _progressLength;
    public float PlayerProgress { get { return _progressLength; } }

    // エネミーの現在の進行度
    private float _progressLengthEnemy = 0f;
    public float EnemyProgress {  get { return _progressLengthEnemy; } }


    // Start is called before the first frame update
    void Start()
    {
        _progressLengthEnemy = _enemyDefLength;
        _progressSliderEnemy.value = _progressLengthEnemy / _goalLength;
    }

    // Update is called once per frame
    void Update()
    {
        _progressLength += _speedManager._playerSpeed * Time.deltaTime;
        _progressLengthEnemy += _speedManager._enemySpeed * Time.deltaTime;
        _progressSlider.value = _progressLength / _goalLength;
        _progressSliderEnemy.value = _progressLengthEnemy / _goalLength;

        MainSceneManager.Instance.IsGoalCheck(_progressLength >= _progressLengthEnemy, _progressLengthEnemy >= _goalLength);
    }
}
