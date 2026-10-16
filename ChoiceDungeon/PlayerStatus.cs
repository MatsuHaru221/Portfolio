using System;
using UnityEngine;

[Serializable]
public class PlayerStat
{
    public int MaxHP = 1;
    public int CurrentHP = 1;
    public int MinAttack = 1;
    public int MaxAttack = 2;
    public float DodgeChance = 1; 
}

public enum StatName
{
    maxHP,
    currentHP,
    minAttack,
    maxAttack,
    dodgeChance,
};

public class PlayerStatus : MonoBehaviour
{
    // HP, 攻撃力の下限と上限, 回避率のステータス管理用
    [SerializeField] private PlayerStat _playerStat;
    // レベルアップ時に上がるステータス管理用
    [SerializeField] private PlayerStat _lvUpStat;

    // 外部から取得できる現在のステータス
    public PlayerStat PlayerCurrentStat => _playerStat;


    /// <summary>
    /// レベルアップ時にステータスを加算する関数
    /// </summary>
    public void LvUpAddStat()
    {
        _playerStat.MaxHP += _lvUpStat.MaxHP;
        _playerStat.CurrentHP += _lvUpStat.MaxHP;
        _playerStat.MinAttack += _lvUpStat.MinAttack;
        _playerStat.MaxAttack += _lvUpStat.MaxAttack;
        _playerStat.DodgeChance += _lvUpStat.DodgeChance;
    }


    /// <summary>
    /// 作り方が下手すぎて生まれてしまったステータス反映用の関数
    /// </summary>
    /// <param name="playerStat">反映させたいステータス</param>
    public void StatusApply(PlayerStat playerStat)
    {
        _playerStat.MaxHP = playerStat.MaxHP;
        _playerStat.CurrentHP = playerStat.CurrentHP;
        _playerStat.MinAttack = playerStat.MinAttack;
        _playerStat.MaxAttack = playerStat.MaxAttack;
        _playerStat.DodgeChance = playerStat.DodgeChance;
    }


}
