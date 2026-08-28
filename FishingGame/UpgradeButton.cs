using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField, Header("�Q�[���}�l�[�W���[")]
    private GameManager _gameManager;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _upgradeSound;
    [SerializeField] private RodColorChanger _rodColorChanger;

    public void OnClicked()
    {
        // �����{�^����������Ƃ��Ɍ��݂̏�����������R�X�g��葽��������
        if(StaticGameData.s_playerMoney >= _gameManager._increaseUpgradeCost * StaticGameData.s_upgradeNum + _gameManager._defaultUpgradeCost)
        {
            StaticGameData.s_playerMoney -= _gameManager._increaseUpgradeCost * StaticGameData.s_upgradeNum + _gameManager._increaseUpgradeCost;
            StaticGameData.s_upgradeNum++;
            _audioSource.PlayOneShot(_upgradeSound);
            _rodColorChanger.ChangeRodColor();
            _gameManager.ChangeTexts();
        }
        else
        {
            Debug.Log("No Money");
        }
    }
}
