using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    // プレイヤーのHPを表示するスライダー
    [SerializeField] private Slider _playerHPSlider;
    // プレイヤーのHPを表示するテキスト
    [SerializeField] private TextMeshProUGUI _hpText;

    // プレイヤーの経験値を表示するスライダー
    [SerializeField] private Slider _playerExpSlider;
    // プレイヤーの経験値を表示するテキスト
    [SerializeField] private TextMeshProUGUI _expText;
    // プレイヤーのレベルを表示するテキスト
    [SerializeField] private TextMeshProUGUI _lvText;

    // プレイヤーの攻撃力を表示するテキスト
    [SerializeField] private TextMeshProUGUI _attackText;

    // プレイヤーの回避力を表示するテキスト
    [SerializeField] private TextMeshProUGUI _dodgeText;

    /// <summary>
    /// HPが変化したときに呼ぶ関数
    /// </summary>
    /// <param name="currentHP">現在のHP</param>
    /// <param name="maxHP">最大HP</param>
    public void HPChange(int currentHP, int maxHP)
    {
        float value = (float)currentHP / (float)maxHP;
        // スライダーに適用
        _playerHPSlider.value = value;

        // テキストに適用
        _hpText.text = $"{currentHP}/{maxHP}";
    }

    /// <summary>
    /// 経験値が変化,レベルが上がった時に呼ぶ関数
    /// </summary>
    /// <param name="currentExp">現在の経験値</param>
    /// <param name="reqExp">レベルアップに必要な経験値</param>
    /// <param name="currentLv">現在のレベル</param>
    public void ExpChange(int currentExp, int reqExp, int currentLv)
    {
        float value = (float)currentExp / (float)reqExp;
        _playerExpSlider.value = value;

        _expText.text = $"{currentExp}/{reqExp}";
        _lvText.text =$"Lv.{currentLv}";
    }
    
    public void AttackChange(int minAttack, int maxAttack)
    {
        _attackText.text = $"ATK: {minAttack} ~ {maxAttack}";
    }

    public void DodgeChange(float dodgeChance)
    {
        _dodgeText.text = $"DEX: {dodgeChance}";
    }
}
