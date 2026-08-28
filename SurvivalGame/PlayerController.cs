using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("プレイヤー")]
    [SerializeField] private float _playerMoveSpeed = 5;                    // プレイヤーの移動速度
    [SerializeField] private float _playerHunger = 100;                     // プレイヤーの空腹度の最大値
    [SerializeField] private float _nowPlayerHunger;                        // 現在のプレイヤーの空腹度
    [SerializeField] private float _hungerDecreaseSpeed = 1;                // お腹が空くスピード
    [SerializeField] private Rigidbody2D _playerRb;
    [SerializeField] private int _woodPlankAmount = 0;                      // 持っている木材の数
    [SerializeField] private KeyCode _interactKey = KeyCode.E;              // インタラクトに使うキー
    [SerializeField] private float _interactRadius = 2;                     // インタラクトできる範囲

    [Header("資材")]

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _hungerText;                   // 空腹度の表示テキスト
    [SerializeField] private TextMeshProUGUI _woodAmountText;               // 木の所持数表示テキスト
    [SerializeField] private TextMeshProUGUI _interactText;                 // インタラクトできるときに表示されるテキスト

    [Header("その他")]
    [SerializeField] private Transform _maxMapLimit;                        // 
    [SerializeField] private Transform _minMapLimit;                        // マップの移動制限
    [SerializeField] public LayerMask _layerMask;
    [SerializeField] private string _sceneName;
    [SerializeField] private float _fadeSpeed;


    private GameObject _currentGameObject;
    private string _currentContactObjTag;
    private bool _isContacting = false;

    private void Start()
    {
        TextChanger();
        _playerRb = GetComponent<Rigidbody2D>();
        _nowPlayerHunger = _playerHunger;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isContacting = true;
        _interactText.gameObject.SetActive(true);
        _currentGameObject = collision.gameObject;
        // Debug.Log(_currentGameObject.tag);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isContacting = false;
        _interactText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (StaticGameManager.s_isGameEnd) return;
        HungerDecrease();
        PlayerMapLimit();
        PlayerMovement();

        if(Input.GetKeyDown(_interactKey))
        {
            if (!_isContacting) return;
            PlayerInteract();
        }
    }
    
    /// <summary>
    /// プレイヤーの空腹度減少
    /// </summary>
    private void HungerDecrease()
    {
        _nowPlayerHunger -= Time.deltaTime * _hungerDecreaseSpeed;
        TextChanger();

        if (_nowPlayerHunger > _playerHunger)
            _nowPlayerHunger = _playerHunger;

        if(_nowPlayerHunger < 0)
        {
            StaticGameManager.s_isGameEnd = true;
            StaticGameManager.s_isWin = false;
            FadeManager.Instance.LoadScene(_sceneName, _fadeSpeed);
        }
    }

    private void TextChanger()
    {
        _hungerText.text = $"Hunger : {_nowPlayerHunger.ToString("F0")}";
        _woodAmountText.text = $"Wood : {_woodPlankAmount}";
    }

    /// <summary>
    /// 振れているオブジェクトのタグを判定してそれぞれの処理を呼び出す
    /// </summary>
    private void CollisionJudge()
    {
        // ベリーブッシュの判定
        if (_currentGameObject.CompareTag("BerryBush"))
        {
            _nowPlayerHunger += _currentGameObject.GetComponent<BerryBush>().EatBerry();
            _currentGameObject = null;
        }

        // 木の判定
        else if (_currentGameObject.CompareTag("Tree"))
        {
            _woodPlankAmount += _currentGameObject.GetComponent<Tree>().CutTree();
            _currentGameObject = null;
            TextChanger();
        }

        else if (_currentGameObject.CompareTag("Raft"))
        {
            _woodPlankAmount = _currentGameObject.GetComponent<Raft>().ConsumeWoodAmount(_woodPlankAmount);
            _currentGameObject = null;
            TextChanger();
        }
    }

    private void PlayerInteract()
    {
        Collider2D[] touchedObj = Physics2D.OverlapCircleAll(transform.position, _interactRadius, _layerMask);
        float distance = float.MaxValue ;
        // Debug.Log(touchedObj);

        foreach(Collider2D collider in touchedObj)
        {
            float objDistance = Vector3.Distance(collider.gameObject.transform.position, transform.position);

            if(distance > objDistance)
            {
                _currentGameObject = collider.gameObject;
            }
        }
        
        CollisionJudge();
    }

    /// <summary>
    /// プレイヤーの移動 WASD
    /// </summary>
    private void PlayerMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * _playerMoveSpeed, transform.position.z);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * _playerMoveSpeed, transform.position.z);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position = new Vector3(transform.position.x - Time.deltaTime * _playerMoveSpeed, transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position = new Vector3(transform.position.x + Time.deltaTime * _playerMoveSpeed, transform.position.y, transform.position.z);
        }
    }

    /// <summary>
    /// プレイヤーのマップ移動制限
    /// </summary>
    private void PlayerMapLimit()
    {
        if(transform.position.x >= _maxMapLimit.position.x)
        {
            transform.position = new Vector2(_maxMapLimit.position.x, transform.position.y);
        }
        if (transform.position.x <= _minMapLimit.position.x)
        {
            transform.position = new Vector2(_minMapLimit.position.x, transform.position.y);
        }
        if (transform.position.y >= _maxMapLimit.position.y)
        {
            transform.position = new Vector2(transform.position.x, _maxMapLimit.position.y);
        }
        if (transform.position.y <= _minMapLimit.position.y)
        {
            transform.position = new Vector2(transform.position.x, _minMapLimit.position.y);
        }
    }

    /// <summary>
    /// Gizmoを使ってoverlapの当たり判定を可視化
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _interactRadius);
    }
}
