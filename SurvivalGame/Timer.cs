using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private int _maxTimeLimit = 21;
    [SerializeField] private int _nowTime = 12;
    [SerializeField] private float _countTimeSpeed;
    [SerializeField] private float _countUpTime;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private string _sceneName;
    [SerializeField] private float _fadeSpeed;
    public float elapsedTime = 0;

    private void Update()
    {
        if (StaticGameManager.s_isGameEnd) return;
        elapsedTime += Time.deltaTime * _countTimeSpeed;
        if(elapsedTime > _countUpTime)
        {
            elapsedTime = 0;
            _nowTime++;
        }
        if(elapsedTime < 9f)
        {
            _timerText.text = $"Time {_nowTime}:0{elapsedTime.ToString("F0")}";
        }
        else
        {
            _timerText.text = $"Time {_nowTime}:{elapsedTime.ToString("F0")}";
        }

        if(_nowTime >= _maxTimeLimit)
        {
            StaticGameManager.s_isGameEnd = true;
            StaticGameManager.s_isWin = false;
            FadeManager.Instance.LoadScene(_sceneName, _fadeSpeed);
        }
    }

}
