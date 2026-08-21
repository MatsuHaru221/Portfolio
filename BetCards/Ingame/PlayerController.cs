using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private HandManager _handManager;
    [SerializeField] private BetPanelManager _betPanelManager;
    [SerializeField] private float _swipeRange = 2f;

    private bool _isSelectedCard = false;
    private Vector2 firstTouchPos;

    //HandManagerで決められたプレイヤーId
    public int PlayerId { get { return _handManager.PlayerId; } }


    private void Update()
    {
        // クリックもしくはタッチしたとき
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Input.mousePosition;
            screenPos.z = 1.0f;

            // 最初にタッチした位置
            firstTouchPos = Camera.main.ScreenToWorldPoint(screenPos);
        }
        // クリックもしくはタッチが離された時
        if (Input.GetMouseButtonUp(0) && MainManager.Instance.IsMyTurn(1) is true)
        {
            // カメラからマウスカーソルの位置のRayを作成
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  
            // Raycastを作成  
            RaycastHit2D hit2D = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            Vector3 screenPos = Input.mousePosition;
            screenPos.z = 1.0f;

            Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            // スワイプ判定 指を離した位置が最初に触れた位置より指定距離以上だったら
            if(worldPos.y > firstTouchPos.y + _swipeRange)
            {
                if (_isSelectedCard)
                {
                    // Debug.Log("スワイプ判定");
                    if(MainManager.Instance.IsMyTurn(PlayerId) is true)
                    {
                        _handManager.SendCard();
                    }
                }
                _isSelectedCard = false;
                return;
            }


            // Rayが何かに当たったら, カードのタッチ判定
            if(hit2D.collider)
            {
                var hitCard = hit2D.collider.gameObject.GetComponent<Card>();
                // Debug.Log(hitCard);

                _handManager.SearchMyHandCard(hitCard);
                // Debug.Log("カードを選びました");
                _isSelectedCard = true;
                //Debug.Log("タッチ判定");
                //Debug.Log(hit2D.transform.gameObject);
                //Debug.Log($"タッチされた場所:{worldPos}");
            }
            else
            {
                // Debug.Log("カードの取得ができませんでした。");
            }

            // Debug.Log("タッチが終わりました");
        }


        // カードの選択（画面タッチ実装までのテスト用）
        // if (Input.GetKeyDown(KeyCode.Alpha1) && _handManager.CardAmount >= 1)
        // { _handManager.SelectCard(0); Debug.Log($"{MainManager.Instance.IsMyTurn(_handManager.PlayerId)} {_handManager.PlayerId}" );}
        // if (Input.GetKeyDown(KeyCode.Alpha2) && _handManager.CardAmount >= 2)
        // { _handManager.SelectCard(1); }
        // if (Input.GetKeyDown(KeyCode.Alpha3) && _handManager.CardAmount >= 3)
        // { _handManager.SelectCard(2); }
        // if(Input.GetKeyDown(KeyCode.Alpha4) && _handManager.CardAmount >= 4)
        // { _handManager.SelectCard(3); }
        // if(Input.GetKeyDown(KeyCode.Alpha5) && _handManager.CardAmount >= 5)
        // { _handManager.SelectCard(4); }

        // if (Input.GetKeyDown(KeyCode.Space) && MainManager.Instance.IsMyTurn(PlayerId) is true)
        // { _handManager.SendCard(); }

        //Debug.Log($"{MainManager.Instance.IsMyTurn(_handManager.PlayerId)} {_handManager.PlayerId}" );
    }

    public int GetPlayerId()
    {
        return _handManager.PlayerId;
    }

    /// <summary>
    /// BetPanelの右矢印が押されたときの処理
    /// </summary>
    /// <param name="sortNum">1なら勝利予想数, 2ならベット額, それ以外ならエラー</param> 

    // public void OnRightButton(int sortNum)
    // {
    //     switch (sortNum)
    //     {
    //         case 0:
    //             Debug.LogError("右矢印の番号に0が入力されています。");
    //             break;
    //         case 1:
    //             // ここで関数を呼ぶ
    //             // Debug.Log("increaseWinAmount");
    //             _betPanelManager.IncreaseWinAmount();
    //             break;
    //         case 2:
    //             // ここで関数を呼ぶ
    //             // Debug.Log("increaseBetAmount");
    //             // _betPanelManager.IncreaseBetAmount();
    //             break;
    //         default:
    //             Debug.LogError("右矢印ボタンに何も数字が入力されていません");
    //             break;
    //     }
    // }

    /// <summary>
    /// ベット増加ボタン
    /// </summary>
    /// <param name="sortNum">何桁目のボタンか</param>
    public void OnBetAmountIncreaseButton(int sortNum)
    {
        _betPanelManager.IncreaseBetAmount(sortNum);
    }

    /// <summary>
    /// ベット減少ボタン
    /// </summary>
    /// <param name="sortNum">何桁目のボタンか</param>
    public void OnBetAmountDecreaseButton(int sortNum)
    {
        _betPanelManager.DecreaseBetAmount(sortNum);
    }

    /// <summary>
    /// 勝利予想数のボタン関数
    /// </summary>
    /// <param name="sortNum">1.増加 2.減少</param>
    public void OnWinAmountButton(int sortNum)
    {
        switch (sortNum)
        {
            case 0:
                Debug.LogError("勝利予想ボタンのソート番号に0が入力されています");
                break;
            case 1:
                _betPanelManager.IncreaseWinAmount();
                break;
            case 2:
                _betPanelManager.DecreaseWinAmount();
                break;
            default:
                Debug.LogError("勝利予想ボタンのソート番号に不正な数字が入力されています");
                break;
        }
    }

    /// <summary>
    /// BetPanelの左矢印が押されたときの処理
    /// </summary>
    /// <param name="sortNum">1なら勝利予想数, 2ならベット額, それ以外ならエラー</param>
    // public void OnLeftButton(int sortNum)
    // {
    //     switch (sortNum)
    //     {
    //         case 0:
    //             Debug.LogError("左矢印の番号に0が入力されています。");
    //             break;
    //         case 1:
    //             // ここで関数を呼ぶ
    //             // Debug.Log("decreaseWinAmount");
    //             _betPanelManager.DecreaseWinAmount();
    //             break;
    //         case 2:
    //             // ここで関数を呼ぶ
    //             // Debug.Log("decreaseBetAmount");
    //             // _betPanelManager.DecreaseBetAmount();
    //             break;
    //         default:
    //             Debug.LogError("左矢印ボタンに何も数字が入力されていません");
    //             break;
    //     }
    // }

    /// <summary>
    /// 確定ボタンを押したときの処理
    /// </summary>
    public void SubmitButton()
    {   // ベットと勝敗予想の確定関数を呼ぶ
        _betPanelManager.Submit();
    }
}