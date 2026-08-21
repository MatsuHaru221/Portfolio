using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : SingletonMonoBehaviour<MainSceneManager>
{
    [SerializeField, Header("SpeedManager")]
    private SpeedManager _speedManager;

    [SerializeField, Header("クリアシーンの名前")]
    private string _clearSceneName;

    [SerializeField, Header("失敗シーンの名前")]
    private string _failedSceneName;

    private bool _isFinished = false;

    protected override bool dontDestroyOnLoad => false;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBGM(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// プレイヤーおよびエネミーがゴールしているかどうかのチェック
    /// </summary>
    /// <param name="playerGoal">プレイヤーがゴールしているかどうか</param>
    /// <param name="enemygoal">エネミーがゴールしているかどうか</param>
    public void IsGoalCheck(bool playerGoal, bool enemygoal)
    {
        if (_isFinished) return;

        if (!enemygoal && !playerGoal) return;

        _speedManager._playerSpeed = 0f;
        _speedManager._enemySpeed = 0f;
        if (enemygoal && playerGoal)
        {
            SoundManager.Instance.StopBGM();
            // 引き分けの処理
            Debug.Log("引き分け");
        }
        else if (!enemygoal && playerGoal)
        {
            SoundManager.Instance.StopBGM();
            //プレイヤーがゴール
            SoundManager.Instance.PlaySE(10);
            FadeManager.Instance.CallScene(_clearSceneName);
            _isFinished = true;
        }
        else if (enemygoal && !playerGoal)
        {
            SoundManager.Instance.StopBGM();
            // エネミーがゴール
            SoundManager.Instance.PlaySE(11);
            FadeManager.Instance.CallScene(_failedSceneName);
            _isFinished = true;
        }
    }
}
