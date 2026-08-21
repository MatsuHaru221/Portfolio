using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
    [SerializeField, Header("SpeedManager")]
    private SpeedManager _speedManager;

    [SerializeField, Header("進行速度と移動速度の比")]
    private float _speedPerProgress = 10.0f;

    // スクロールのスピード
    private float _scrollSpeed = -2.0f;

    [SerializeField, Header("スクロール終了のx座標")]
    private float _scrollEndX = -11.7f;

    private Vector3 _defPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        _defPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _scrollSpeed = _speedManager._playerSpeed / _speedPerProgress * -1;
        Vector3 performedPos = new Vector3(transform.position.x + _scrollSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        if (performedPos.x <= _scrollEndX)
        {
            performedPos = _defPos;
        }
        transform.position = performedPos;
    }
}
