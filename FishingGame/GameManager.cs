using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField, Header("����������܂ł̎���")]
    private float _minBaitTime;
    [SerializeField]
    private float _maxBaitTime;

    [SerializeField, Header("�r�b�N���}�[�N��object")]
    private GameObject _exclamationObj;

    [SerializeField, Header("����̃e�L�X�g")]
    private TextMeshProUGUI _moneyText;
    [SerializeField, Header("�v���C���[�̏�������")]
    private int _defaultMoney;

    [SerializeField, Header("�ő�̋��̃e�L�X�g")]
    private TextMeshProUGUI _biggestFishText;
    [SerializeField]
    private float _biggestFishSize = 0;

    [SerializeField, Header("�ނ������̕�obj")]
    private GameObject _fishStates;
    [SerializeField]
    private TextMeshProUGUI _fishSizeText;
    [SerializeField]
    private TextMeshProUGUI _takeTimeText;
    [SerializeField]
    private TextMeshProUGUI _fishPriceText;

    [SerializeField, Header("�����R�X�g�̃e�L�X�g")]
    private TextMeshProUGUI _upgradeCostText;
    [SerializeField, Header("�ŏ��̋����R�X�g")]
    public int _defaultUpgradeCost;
    [SerializeField, Header("�����R�X�g�̏オ�蕝")]
    public int _increaseUpgradeCost;
    [SerializeField, Header("�������鋭��")]
    private float _increasePoints;
    [SerializeField, Header("������")]
    private TextMeshProUGUI _upgradeCountText;

    [SerializeField, Header("�����z�̔{��")]
    private float _sellMultiplyer;

    [SerializeField, Header("���̃T�C�Y")]
    private int _minFishSize = 1;
    [SerializeField]
    private int _maxFishSize = 30;
    [SerializeField, Header("����ɑ傫�����ւ̕���_")]
    private int _firstWall = 20;    // 1~30
    [SerializeField]
    private int _secondWall = 80;   // 30 ~ 90
    [SerializeField]
    private int _thirdWall = 170;   // 90 ~ 180
    [SerializeField, Header("�傫�����ւ̕���_")]
    private List<int> _sizeWalls = new List<int>();
    [SerializeField] private AudioClip _fishSound;
    [SerializeField] private AudioSource _audioSource;

    private float defaultFishSize;
    private float fishSize;
    private float baitTime;
    private float clickSpeed = 0f;
    private float elapsedTime = 0f;

    private void Awake()
    {
        StaticGameData.s_playerMoney = _defaultMoney;
        _moneyText.text = $"Money : {StaticGameData.s_playerMoney}";
        _upgradeCostText.text = $"Cost : {_defaultUpgradeCost}";
    }

    private void Start()
    {
        Fishing().Forget();
    }


    private async UniTask Fishing()
    {
        while (true)
        {
            await FirstFishSize();
            await StartFishing();
            await EndFishing();
        }
    }

    private async UniTask StartFishing()
    {
        await RandomBaitTime();
        Debug.Log(baitTime);
        await UniTask.Delay(TimeSpan.FromSeconds(baitTime));

        _exclamationObj.SetActive(true);
        float startTime = Time.time;
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        _audioSource.PlayOneShot(_fishSound);
        _exclamationObj.SetActive(false);
        elapsedTime = Time.time - startTime;
    } 

    private async UniTask EndFishing()
    {
        // ���̑傫���v�Z
        fishSize = (defaultFishSize - elapsedTime) / defaultFishSize * defaultFishSize;
        _fishSizeText.text = $"FishSize : {fishSize.ToString("F1")}kg";
        _takeTimeText.text = $"TakeTime : {elapsedTime.ToString("F2")}sec";
        SellFish(fishSize);
        _fishStates.SetActive(true);
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        _fishStates.SetActive(false);
        
        if(fishSize >= _biggestFishSize)
        {
            _biggestFishSize = fishSize;
            _biggestFishText.text = $"BiggestFish : {_biggestFishSize.ToString("F1")}kg";
        }
    }

    private void SellFish(float fishSize)
    {
        float fishAmount = fishSize * _sellMultiplyer;
        _fishPriceText.text = $"Money : +{fishAmount.ToString("F0")}";
        StaticGameData.s_playerMoney += (int)fishAmount;
        ChangeTexts();
    }

    private async UniTask FirstFishSize()
    {
        defaultFishSize = UnityEngine.Random.Range(_minFishSize, _maxFishSize);
        float multiplyer = StaticGameData.s_upgradeNum / 10 + 1;
        if(defaultFishSize >= _firstWall)
        {
            Debug.Log("���̕�");
            defaultFishSize = UnityEngine.Random.Range(_sizeWalls[0], _maxFishSize * 3 * multiplyer);
        }
        await UniTask.Delay(10);
        if(defaultFishSize >= _secondWall)
        {
            Debug.Log("���̕�");
            defaultFishSize = UnityEngine.Random.Range(_sizeWalls[1], _maxFishSize * 6 * multiplyer);
        }
        await UniTask.Delay(10);
        if (defaultFishSize >= _thirdWall)
        {
            Debug.Log("��O�̕�");
            defaultFishSize = UnityEngine.Random.Range(_sizeWalls[2], _maxFishSize * 12 * multiplyer);
        }
        await UniTask.Delay(10);
        if(defaultFishSize >= _sizeWalls[3])
        {
            Debug.Log("��l�̕�");
            defaultFishSize = UnityEngine.Random.Range(_sizeWalls[3], _maxFishSize * 24 * multiplyer);
        }
        await UniTask.Delay(10);
        if (defaultFishSize >= _sizeWalls[4])
        {
            Debug.Log("��܂̕�");
            defaultFishSize = UnityEngine.Random.Range(_sizeWalls[4], _maxFishSize * 48 * multiplyer);
        }
        await UniTask.WhenAll();
    }

    /// <summary>
    /// �Ă񂾎��Ɍ��݂̏�����ƃA�b�v�O���[�h�R�X�g��e�L�X�g�ɍX�V
    /// </summary>
    public void ChangeTexts ()
    {
        _moneyText.text = $"Money : {StaticGameData.s_playerMoney}";
        _upgradeCostText.text = $"Cost : {_increaseUpgradeCost * StaticGameData.s_upgradeNum + _defaultUpgradeCost}";
        _upgradeCountText.text = $"Upgrade Count : {StaticGameData.s_upgradeNum}";
    }

    /// <summary>
    /// ����������܂ł̎��Ԃ̃����_���̒l��baitTime�ɓ����
    /// </summary>
    private async UniTask RandomBaitTime()
    {
        float maxBaitTime = _maxBaitTime - (StaticGameData.s_upgradeNum / 20);

        await UniTask.Delay(10);
        if (true)
        {
            baitTime = UnityEngine.Random.Range(_minBaitTime, maxBaitTime);
        }
        //else if (false) // �T���a�̎������Ɏg�p
        //{

        //}
    }


}
