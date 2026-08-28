using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Raft : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _needWoodAmountText;
    [SerializeField] private int _needWoodAmount = 20;
    [SerializeField] private int _nowWoodAmount = 0;
    [SerializeField] private GameObject _colliderLine;
    [SerializeField] private GameObject _raftBody;
    [SerializeField] private string _sceneName;
    [SerializeField] private float _fadeSpeed;

    private void Start()
    {
        if (StaticGameManager.s_isGameEnd) return;
        ChangeWoodAmountText();
    }

    public int ConsumeWoodAmount(int amount)
    {
        int backAmount = 0;
        if(amount + _nowWoodAmount >= _needWoodAmount)
        {
            backAmount = (amount + _nowWoodAmount) - _needWoodAmount;
            _nowWoodAmount = _needWoodAmount;
        }
        else
        {
            _nowWoodAmount += amount;
        }
        ChangeWoodAmountText();

        if(_nowWoodAmount >= _needWoodAmount)
        {
            ReachedWoodAmount();
        }
        return backAmount;
    }

    private void ReachedWoodAmount()
    {
        _needWoodAmountText.gameObject.SetActive(false);
        _colliderLine.SetActive(false);
        _raftBody.gameObject.SetActive(true);

        StaticGameManager.s_isGameEnd = true;
        StaticGameManager.s_isWin = true;
        FadeManager.Instance.LoadScene(_sceneName, _fadeSpeed);
    }

    private void ChangeWoodAmountText()
    {
        _needWoodAmountText.text = $"Wood : {_nowWoodAmount} / {_needWoodAmount}";
    }
}
