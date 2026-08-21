using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] public float _playerSpeed;
    [SerializeField] public float _enemySpeed;

    [SerializeField] private float _decelerationDuration;
    [SerializeField] private float _returnDuration;
    [SerializeField] private float _recoverSpeed;
    [SerializeField] private float _stackingMinSpeed;

    [SerializeField] private float _cardMinSpeed;
    [SerializeField] private float _slowPerSecSpeed;
    [SerializeField] private TextMeshProUGUI _speedText;

    private float defaultPlayerSpeed;
    private bool isStackingSlow = false;

    private void Start()
    {
        defaultPlayerSpeed = _playerSpeed;
    }

    public IEnumerator ObjectStacking() //  障害物に当たった時の減速と加速
    {
        // Debug.Log("coroutine");
        float elapsedTime = 0f;
        isStackingSlow = true;

        while(elapsedTime < _decelerationDuration)
        {
            _playerSpeed = Mathf.Lerp(defaultPlayerSpeed, _stackingMinSpeed, elapsedTime / _decelerationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _playerSpeed = _stackingMinSpeed;
        elapsedTime = 0f;

        // カードを使用時にぶつかった時の速度回復
        if (StaticManager.GetIsHandlingCard)
        {
            while (elapsedTime < _returnDuration)
            {
                _playerSpeed = Mathf.Lerp(_stackingMinSpeed, _cardMinSpeed, elapsedTime / _returnDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _playerSpeed = _cardMinSpeed;
        }
        else
        {
            while(elapsedTime < _returnDuration)
            {
                _playerSpeed = Mathf.Lerp(_stackingMinSpeed, defaultPlayerSpeed, elapsedTime / _returnDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _playerSpeed = defaultPlayerSpeed;
        }
        isStackingSlow = false;
    }

    // Update is called once per frame
    void Update()
    {
        // カード所持時の減速
        if (StaticManager.GetIsHandlingCard && !isStackingSlow)
        {
            if (_playerSpeed > _cardMinSpeed)
            {
                _playerSpeed -= _slowPerSecSpeed * Time.deltaTime;
            }
            else
            {
                _playerSpeed = _cardMinSpeed;
            }
        }
        // スピードの回復
        else if(_playerSpeed <= defaultPlayerSpeed && !StaticManager.GetIsHandlingCard)
        {
            _playerSpeed += _recoverSpeed * Time.deltaTime;
        }
        else if (!isStackingSlow && !StaticManager.GetIsHandlingCard)
        {
            _playerSpeed = defaultPlayerSpeed;
        }

        _speedText.text = $"Speed : {_playerSpeed.ToString("F0")}";
    }
}
