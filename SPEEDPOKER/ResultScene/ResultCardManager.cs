using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCardManager : MonoBehaviour
{
    [SerializeField, Header("全カードのスプライト")]
    private List<Sprite> _allCardSprites = new List<Sprite>();

    public static ResultCardManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public Sprite CardSprites(int spriteIndex)
    {
        return _allCardSprites[spriteIndex];
    }
}
