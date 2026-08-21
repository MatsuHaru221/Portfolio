using TMPro;
using UnityEngine.UI;
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class BetPanelManager : MonoBehaviour
{
    // [Header("仮設定用項目")]
    // [SerializeField, Label("最初の所持金（仮）")] private int _initialMoney = 1000;

    // [Header("お金関係")]
    // [SerializeField, Label("現在の所持金")] private int _moneyAmount;
    [SerializeField] GameObject _betPanel;
    [SerializeField, Label("最低ベット額")] private int _minBetAmount = 100;

    [Header("Betの選択時に出てくる数字のテキスト")]
    [SerializeField] private TextMeshProUGUI _winAmountText;
    [SerializeField] private List<TextMeshProUGUI> _betAmountTexts = new List<TextMeshProUGUI>();
    [SerializeField] private TextMeshProUGUI _playerChipText;
    [Header("テキストアニメーション")]
    [SerializeField] private float _textShakeRange = 5f;
    [SerializeField] private float _textAnimationSpeed = 0.2f;
    [SerializeField] private int _textShakeCount = 2;

    [Header("マネージャー系")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private BetManager _betManager;

    private List<int> _betDigits = new List<int>{0,1,0,0};   // ベットを１桁ずつに分けて入れるリスト
    private int _selectedWinAmount = 0;     // 勝利予想数
    private int _selectedBetAmount = 100;   // ベット額

    private bool _isAdvance = false;
    private bool _isAdvance2 = false;
    private bool _isAdvance3 = false;
    private bool _isPostponement = false;
    private bool _isPostponement2 = false;
    private bool _isPostponement3 = false;

    private void Start()
    {
        //_moneyAmount = _initialMoney;
        _selectedWinAmount = 0;
        _selectedBetAmount = 100;

        //イベントの登録
        MainManager.Instance.RegisterStartBetPhaseEvent(ActiveBetPanle);
    }

    /// <summary>
    /// ベット金額を1桁ずつリストに収納
    /// </summary>
    private void BetToString()
    {
        int bet = _selectedBetAmount;
        int x = 10;
        for(int i = 3; i >= 0; i--)
        {
            // Debug.Log($"計算用のbetは現在{bet}");
            // Debug.Log($"割る用のXは現在{x}");
            // Debug.Log($"計算用のiは現在{i}");
            // Debug.Log($"iの値は{i}");
            _betDigits[i] = bet % x;
            // Debug.Log($"betDigitとiの関係:Iが{i}の時BetDigitは{_betDigits[i]}");
            bet /= x;
            
            // char digitChar = bet[i];
            // _betDigits[i] = int.Parse(digitChar.ToString());
            // Debug.Log($"BetDigitsは{_betDigits[i]}");
        }
    }

    public void ActiveBetPanle()
    {
        ChangeText();
        _betPanel.SetActive(true);
    }

    public void InactiveBetPanel()
    {
        _betPanel.SetActive(false);
    }

    /// <summary>
    /// 勝利予想を増やす処理
    /// </summary>
    public void IncreaseWinAmount()
    {
        if (_selectedWinAmount >= MainManager.Instance.CardsPerRound) return;

        _selectedWinAmount++;
        ChangeText();
        TextAnimation(10).Forget();
    }

    /// <summary>
    /// 勝利予想を減らす処理
    /// </summary>
    public void DecreaseWinAmount()
    {
        if (_selectedWinAmount <= 0) return;

        _selectedWinAmount--;
        ChangeText();
        TextAnimation(10).Forget();
    }

    /// <summary>
    /// ベット額を増やす処理
    /// </summary>
    public void IncreaseBetAmount(int sortNum)
    {
        switch (sortNum)
        {
            case 0:
                if(_selectedBetAmount + 1000 > _betManager.PlayerChipLists[_playerController.PlayerId - 1]) return;
                _selectedBetAmount += 1000;
                // Debug.Log($"Bet{_selectedBetAmount}");
                break;
            case 1:
            if(_selectedBetAmount + 100 > _betManager.PlayerChipLists[_playerController.PlayerId - 1]) return;
                if(_selectedBetAmount % 1000 >= 900)
                {
                    _isAdvance = true;
                }
                _selectedBetAmount += 100;
                break;
            case 2:
            if(_selectedBetAmount + 10 > _betManager.PlayerChipLists[_playerController.PlayerId - 1]) return;
                if(_selectedBetAmount % 100 >= 90)
                {
                    if(_selectedBetAmount % 1000 >= 900)
                    {
                        _isAdvance2 = true;
                    }
                    _isAdvance = true;
                }
                _selectedBetAmount += 10;
                break;
            case 3:
            if(_selectedBetAmount + 1 > _betManager.PlayerChipLists[_playerController.PlayerId - 1]) return;
                if(_selectedBetAmount % 10 >= 9)
                {
                    if(_selectedBetAmount % 100 >= 90)
                    {
                        if(_selectedBetAmount % 1000 >= 900)
                        {
                            _isAdvance3 = true;
                        }
                        _isAdvance2 = true;
                    }
                    _isAdvance = true;
                }
                _selectedBetAmount += 1;
                break;
            default:
                break;
        }

        BetToString();
        ChangeText();
        TextAnimation(sortNum).Forget();
    }

    /// <summary>
    /// ベット額を減らす処理
    /// </summary>
    public void DecreaseBetAmount(int sortNum)
    {
        switch (sortNum)
        {
            case 0:
                if(_selectedBetAmount - 1000 < 0) return;
                _selectedBetAmount -= 1000;
                break;
            case 1:
                if(_selectedBetAmount - 100 < 0) return;
                if(_selectedBetAmount % 1000 < 100)
                {
                    _isPostponement = true;
                }
                _selectedBetAmount -= 100;
                break;
            case 2:
                if(_selectedBetAmount - 10 < 0) return;
                if(_selectedBetAmount % 100 < 10)
                {
                    if(_selectedBetAmount % 1000 < 100)
                    {
                        _isPostponement2 = true;
                    }
                    _isPostponement = true;
                }
                _selectedBetAmount -= 10;
                break;
            case 3:
                if(_selectedBetAmount - 1 < 0) return;
                if(_selectedBetAmount % 10 < 1)
                {
                    if(_selectedBetAmount % 100 < 10)
                    {
                        if(_selectedBetAmount % 1000 < 100)
                        {
                            _isPostponement3 = true;
                        }
                        _isPostponement2 = true;
                    }
                    _isPostponement = true;
                }
                _selectedBetAmount -= 1;
                break;
            default:
                break;
        }
        BetToString();
        ChangeText();
        TextAnimation(sortNum).Forget();
    }

    /// <summary>
    /// 確定ボタンを押したときの処理
    /// </summary>
    public void Submit()
    {
        if(_selectedBetAmount < _minBetAmount)
        {   // ここで最低ベット金額に足りないことを知らせる演出をいれるかも
            return;
        }
        // _selectedWinAmount と _selectedBetAmount をここで送る
        _betManager.SetWinAndBet(_playerController.PlayerId, _selectedWinAmount, _selectedBetAmount);
        InactiveBetPanel();
    }

    /// <summary>
    /// テキスト変化
    /// </summary>
    private void ChangeText()
    {
        _winAmountText.text = $"{FontConverter.CharConversion(_selectedWinAmount.ToString())}";
        for(int i = 0; i <= _betDigits.Count - 1; i++)
        {
            // Debug.Log($"iの数値は{i}");
            // Debug.Log($"_betDigitsは{_betDigits[i]}");
            // Debug.Log($"SelectedAmountの量は{_selectedBetAmount}");
            // Debug.Log($"betdigitの左から{i}番目は{_betDigits[i]}");
            _betAmountTexts[i].text = $"{FontConverter.CharConversion(_betDigits[i].ToString())}";
        }
        _playerChipText.text = $"{FontConverter.CharConversion(_betManager.PlayerChipLists[_playerController.GetPlayerId() - 1].ToString())}";
    }

    private async UniTask TextAnimation(int sortNum)
    {
        switch (sortNum)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                _betAmountTexts[sortNum].transform.DOKill(true);
                AdvanceTextAnimation(sortNum);
                await _betAmountTexts[sortNum].transform.DOPunchPosition(new Vector3(0,_textShakeRange,0),_textAnimationSpeed, _textShakeCount);
                
                break;
            case 10:
                _winAmountText.transform.DOKill(true);
                AdvanceTextAnimation(sortNum);
                await _winAmountText.transform.DOPunchPosition(new Vector3(0,_textShakeRange,0),_textAnimationSpeed, _textShakeCount);
                break;
            default:
                Debug.LogError("BetPanelManagerのTextAnimationで使用できない値が入力されています");
                break;
        }
    }

    private void AdvanceTextAnimation(int sortNum)
    {
        if (_isAdvance3)
        {
            _betAmountTexts[sortNum - 1].transform.DOPunchPosition(new Vector3(0,_textShakeRange,0),_textAnimationSpeed, _textShakeCount);
            _betAmountTexts[sortNum - 2].transform.DOPunchPosition(new Vector3(0,_textShakeRange,0),_textAnimationSpeed, _textShakeCount);
            _betAmountTexts[sortNum - 3].transform.DOPunchPosition(new Vector3(0,_textShakeRange,0),_textAnimationSpeed, _textShakeCount);
            _isAdvance3 = false;
            _isAdvance2 = false;
            _isAdvance = false;
        }
        if (_isAdvance2)
        {
            _betAmountTexts[sortNum - 1].transform.DOPunchPosition(new Vector3(0,_textShakeRange,0),_textAnimationSpeed, _textShakeCount);
            _betAmountTexts[sortNum - 2].transform.DOPunchPosition(new Vector3(0,_textShakeRange,0),_textAnimationSpeed, _textShakeCount);
            _isAdvance2 = false;
            _isAdvance = false;
        }
        if (_isAdvance)
        {
            _betAmountTexts[sortNum - 1].transform.DOPunchPosition(new Vector3(0,_textShakeRange,0),_textAnimationSpeed, _textShakeCount);
            _isAdvance = false;
        }

        if (_isPostponement3)
        {
            _betAmountTexts[sortNum - 1].transform.DOPunchPosition(new Vector3(0,_textShakeRange,0),_textAnimationSpeed, _textShakeCount);
            _betAmountTexts[sortNum - 2].transform.DOPunchPosition(new Vector3(0,_textShakeRange,0),_textAnimationSpeed, _textShakeCount);
            _betAmountTexts[sortNum - 3].transform.DOPunchPosition(new Vector3(0,_textShakeRange,0),_textAnimationSpeed, _textShakeCount);
            _isPostponement3 = false;
            _isPostponement2 = false;
            _isPostponement = false;
        }
        if (_isPostponement2)
        {
            _betAmountTexts[sortNum - 1].transform.DOPunchPosition(new Vector3(0,_textShakeRange,0),_textAnimationSpeed, _textShakeCount);
            _betAmountTexts[sortNum - 2].transform.DOPunchPosition(new Vector3(0,_textShakeRange,0),_textAnimationSpeed, _textShakeCount);
            _isPostponement2 = false;
            _isPostponement = false;
        }
        if (_isPostponement)
        {
            _betAmountTexts[sortNum - 1].transform.DOPunchPosition(new Vector3(0,_textShakeRange,0),_textAnimationSpeed, _textShakeCount);
            _isPostponement = false;
        }
    }
}