using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultCardChanger : MonoBehaviour
{
    [SerializeField] private Image _mySprite;
    [SerializeField, Header("このカードリストの位置 上から下に0～2")] 
    private int _myCardListIndex = 0;
    [SerializeField, Header("このカードの位置 左から右に0～4")]
    private int _myCardIndex = 0;

    private int index;

    private void Start()
    {
        // デバッグ用の仮データ
        // index = Random.Range(1, 53);

        // 必要なカードのindexを取ってくる
        index = StaticGameData.s_wonPlayerHands[_myCardListIndex,_myCardIndex];
        GetSprite();
    }

    private void GetSprite()
    {
        // Debug.Log(index);
        _mySprite.sprite = ResultCardManager.Instance.CardSprites(index);
    }
}
