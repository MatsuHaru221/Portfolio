using UnityEngine;
using Cysharp.Threading.Tasks;
using Assets.MAIN.Common.Scripts.Domain;

public class PlayerManager : MonoBehaviour
{
    // PlayerUIManagerの取得
    [SerializeField] private PlayerUIManager _playerUIManager;
    // PlayerStatusの取得
    [SerializeField] private PlayerStatus _playerStatus;
    // FloorPresenterの取得
    [SerializeField] private FloorPresenter _floorPresenter;

    // 現在のプレイヤーレベル
    [SerializeField] private int _currentLv = 1;
    // 現在の経験値量
    [SerializeField] private int _currentExp = 0;
    // プレイヤーがレベルアップに必要な経験値
    [SerializeField] private int _expReqForLvUp = 5;
    // 仮 1レベルごとに上がっていく必要経験値量の倍率
    [SerializeField] private float _expReqMultiplier = 1.3f;
    
    //プレイヤーの現在のパラメータ
    private PlayerStat playerCurrentStat;

   // // 仮の敵のHP
   // private int enemyHP = 8;
   // // 仮のボス敵のHP
   // private int _bossEnemyHP = 30;
   // // 仮の敵の攻撃力
   // private int enemyAttack = 3;
   // // 仮のボス敵の攻撃力
   // private int _bossEnemyAttack = 5;

    void Awake()
    {
        GetPlayerStatus(_playerStatus.PlayerCurrentStat);
        _playerUIManager.HPChange(playerCurrentStat.CurrentHP, playerCurrentStat.MaxHP);
        _playerUIManager.ExpChange(_currentExp, _expReqForLvUp, _currentLv);
        _playerUIManager.AttackChange(playerCurrentStat.MinAttack, playerCurrentStat.MaxAttack);
        _playerUIManager.DodgeChange(playerCurrentStat.DodgeChance);
    }

    /// <summary>
    /// プレイヤーのステータスの取得
    /// </summary>
    /// <param name="playerStat">ステータス元の取得</param>
    private void GetPlayerStatus(PlayerStat playerStat)
    {
        playerCurrentStat = playerStat;
    }
    public void AttackTest()
    {
       // AttackCommand().Forget();
    }

    /// <summary>
    /// 攻撃したときの処理
    /// </summary>
    public async UniTask AttackCommand(EnemyModel currentEnemy)
    {
        await _floorPresenter.UpdatePlayerText($"武器を構えた!");

        // 攻撃の上限と下限からランダムな値を取得
        var rand = Random.Range(playerCurrentStat.MinAttack, playerCurrentStat.MaxAttack + 1);

        if (_floorPresenter.ChackHayabusaSword())
        {
            rand *= 2;
            await _floorPresenter.UpdatePlayerText($"はやぶさの剣の効果で攻撃力が2倍になった!");
        }

        if (_floorPresenter.ChackCursedSword())
        {
            rand /= 2;
            await _floorPresenter.UpdatePlayerText($"呪いの剣の効果でダメージが半分になった!");
        }

        _floorPresenter.ResetSword();
        Debug.Log($"攻撃力{rand}!");
        await _floorPresenter.UpdatePlayerText($"攻撃力{rand}!");
        SoundService.Instance.PlaySE(SEType.Beam);

        if (_floorPresenter.IsFinalFloor())
        {
            // ランダムな値が敵のHPを上回ったら
            if (currentEnemy.CurrentHP.Value <= rand)
            { // 攻撃
                currentEnemy.TakeEnemyDamage(rand);
                await _floorPresenter.HideEnemy();

                _floorPresenter.SetResulData(_currentLv);
                await FadeManager.Instance.FadeLoadScene("Result");
            }
            // 下回っていたら
            else if (currentEnemy.CurrentHP.Value > rand)
            {
                Debug.Log("攻撃成功!");
                await SuccessAttack(currentEnemy);
                await _floorPresenter.UpdatePlayerText($"攻撃成功！");

                Debug.Log("回避チャレンジ!");
                await _floorPresenter.UpdatePlayerText($"回避チャレンジ!");

                SoundService.Instance.PlaySE(SEType.BlackHole);

                // 相手の攻撃を回避できるか受けるか
                if (DodgeChallenge() is true)
                { // 回避成功
                    await SuccessDodge();
                    await _floorPresenter.UpdatePlayerText($"回避成功!");
                }
                else
                { // 回避失敗
                    await TakeDamage(currentEnemy.AtkPower.Value);
                }
            }
        }
        else
        {
            // ランダムな値が敵のHPを上回ったら
            if (currentEnemy.CurrentHP.Value <= rand)
            { // 攻撃
                currentEnemy.TakeEnemyDamage(rand);
                await _floorPresenter.HideEnemy();
                await _floorPresenter.UpdatePlayerText($"撃破成功!");
                await SuccessAttack(currentEnemy);
            }
            // 下回っていたら
            else if (currentEnemy.CurrentHP.Value > rand)
            {
                currentEnemy.TakeEnemyDamage(rand);

                Debug.Log("攻撃失敗!");
                await _floorPresenter.UpdatePlayerText($"撃破失敗!");

                Debug.Log("回避チャレンジ!");
                await _floorPresenter.UpdatePlayerText($"回避チャレンジ!");

                // 相手の攻撃を回避できるか受けるか
                if (DodgeChallenge() is true)
                { // 回避成功
                    await SuccessDodge();
                }
                else
                { // 回避失敗
                    await TakeDamage(currentEnemy.AtkPower.Value);
                }

                await _floorPresenter.HideEnemy();
                await _floorPresenter.UpdatePlayerText($"敵は逃げ出した！");
            }
        }
        
    }

    /// <summary>
    /// 回避できるかどうかのチャレンジ
    /// </summary>
    private bool DodgeChallenge()
    {
        if(playerCurrentStat.DodgeChance >= 100)
        return true;
        var value = Random.Range(1, 101);

        // 回避値がランダムな値より小さかったら回避成功
        if(playerCurrentStat.DodgeChance >= value)
        {  
            return true;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// ダメージを受けたときに呼ぶ関数です
    /// </summary>
    /// <param name="damage">被ダメージ</param>
    private async UniTask TakeDamage(int damage)
    {
        Debug.Log("回避失敗!");
        await _floorPresenter.UpdatePlayerText($"回避失敗!");

        SoundService.Instance.PlaySE(SEType.BlackHole);

        if (_floorPresenter.ChackIronShield())
        {
            damage /= 2;
            await _floorPresenter.UpdatePlayerText($"鉄の盾の効果でダメージが半分になった!");
        }

        // ダメージ分現在のHPを減らす
        playerCurrentStat.CurrentHP -= damage;

        if(playerCurrentStat.CurrentHP <= 0)
            playerCurrentStat.CurrentHP = 0;

        // HPスライダーに現在のHPを反映させる
        _playerUIManager.HPChange(playerCurrentStat.CurrentHP, playerCurrentStat.MaxHP);

        Debug.Log($"{damage}ダメージを受けた!");
        await _floorPresenter.UpdatePlayerText($"{damage}ダメージを受けた!");

        // HPが0以下になったら死
        if(playerCurrentStat.CurrentHP <= 0)
        {
            Debug.Log("死んだ!");
            await _floorPresenter.UpdatePlayerText($"死んだ!");
            _floorPresenter.SetResulData(_currentLv);
        }
    }

    /// <summary>
    /// 攻撃が成功したときに呼ぶ関数
    /// </summary>
    private async UniTask SuccessAttack(EnemyModel currentEnemy)
    {
        Debug.Log("攻撃成功!");

        if (_floorPresenter.IsFinalFloor())
        {
            currentEnemy.TakeEnemyDamage(Random.Range(playerCurrentStat.MinAttack, playerCurrentStat.MaxAttack + 1)) ;
            //Debug.Log($"ボスの残りHPは{_bossEnemyHP}です。");
            AttackCommand(currentEnemy).Forget();
        }
        else
        {
            await GetExp(10);
            await UniTask.WaitForSeconds(1);
        }
        
        
    }

        /// <summary>
    /// 回避が成功したときに呼ぶ関数
    /// </summary>
    private async UniTask SuccessDodge()
    {
        Debug.Log("回避成功!");
        await _floorPresenter.UpdatePlayerText($"回避成功!");
    }


    /// <summary>
    /// 経験値を取得したときの処理
    /// </summary>
    /// <param name="exp">取得した経験値量</param>
    private async UniTask GetExp(int exp)
    {
        _currentExp += exp;
        _playerUIManager.ExpChange(_currentExp, _expReqForLvUp, _currentLv);
        Debug.Log($"{exp}経験値獲得!");
        await _floorPresenter.UpdatePlayerText($"{exp}経験値獲得!");

        // 現在の経験値がレベルアップ必要量に達したら
        if(_currentExp >= _expReqForLvUp)
        {
            SoundService.Instance.PlaySE(SEType.Goal_Jingle, 50);
            await LevelUp();
        }
    }

    /// <summary>
    /// レベルアップしたときの処理
    /// </summary>
    private async UniTask LevelUp()
    {
        _currentLv++;

        // レベルアップでのステータス加算
        _playerStatus.LvUpAddStat();

        // 加算後のステータスを新たに取得 ここもっといい方法知りたい
        GetPlayerStatus(_playerStatus.PlayerCurrentStat);

        _playerUIManager.HPChange(playerCurrentStat.CurrentHP, playerCurrentStat.MaxHP);

        // 現在の経験値量を必要分だけ減らす
        _currentExp -= _expReqForLvUp;

        // レベルアップに必要な経験値量を増加
        _expReqForLvUp = Mathf.FloorToInt(_expReqForLvUp * _expReqMultiplier);

        // UIに反映
        _playerUIManager.ExpChange(_currentExp, _expReqForLvUp, _currentLv);

        Debug.Log($"レベルアップした!");
        _playerUIManager.AttackChange(playerCurrentStat.MinAttack, playerCurrentStat.MaxAttack);
        _playerUIManager.DodgeChange(playerCurrentStat.DodgeChance);
        await _floorPresenter.UpdatePlayerText($"レベルアップした!");

        // 2レベル以上一気に上がるときのための再処理
        if(_currentExp >= _expReqForLvUp)
        {
            await GetExp(0);
        }
    }

        /// <summary>
    /// 指定したステータスを引数に入れた値分そのまま加算します
    /// </summary>
    /// <param name="statName">上げたいステータスをenum型で指定</param>
    /// <param name="value">加算する値</param>
    public async UniTask AddStatus(StatName statName, int value)
    {
        switch (statName)
        {
            case StatName.maxHP:
                playerCurrentStat.MaxHP += value;
                break;
            case StatName.currentHP:
                playerCurrentStat.CurrentHP += value;
                //　加算後のHPが最大HPWを超えていたら
                if(playerCurrentStat.CurrentHP > playerCurrentStat.MaxHP)
                { // 最大HPに制限
                    playerCurrentStat.CurrentHP = playerCurrentStat.MaxHP;
                }
                _playerUIManager.HPChange(playerCurrentStat.CurrentHP, playerCurrentStat.MaxHP);
                break;
            case StatName.minAttack:
                playerCurrentStat.MinAttack += value;
                // 加算後の攻撃最低値が最大値を超えていたら
                if(playerCurrentStat.MinAttack > playerCurrentStat.MaxAttack)
                { // 最大値に制限
                    playerCurrentStat.MinAttack = playerCurrentStat.MaxAttack;
                }
                _playerUIManager.AttackChange(playerCurrentStat.MinAttack, playerCurrentStat.MaxAttack);
                break;
            case StatName.maxAttack:
                playerCurrentStat.MaxAttack += value;
                _playerUIManager.AttackChange(playerCurrentStat.MinAttack, playerCurrentStat.MaxAttack);
                break;
            case StatName.dodgeChance:
                playerCurrentStat.DodgeChance += value;
                _playerUIManager.DodgeChange(playerCurrentStat.DodgeChance);
                break;
        }

        // ステータスの反映と念のためのそこからの再取得
        _playerStatus.StatusApply(playerCurrentStat);
        GetPlayerStatus(_playerStatus.PlayerCurrentStat);

        DisplayStat();
    }

    /// <summary>
    /// 今のステータスをログに表示します
    /// </summary>
    private void DisplayStat()
    {
        Debug.Log($"PlayerCurrentStat\nMaxHP:{playerCurrentStat.MaxHP}\nCurrentHP:{playerCurrentStat.CurrentHP}\nMinAttack:{playerCurrentStat.MinAttack}\nMaxAttack:{playerCurrentStat.MaxAttack}\nDodgeChance:{playerCurrentStat.DodgeChance}");
    }
}
