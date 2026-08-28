using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Tree : MonoBehaviour
{
    [SerializeField] private GameObject _cuttingPart;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private float _glowingTime;
    [SerializeField] private int _getPlankAmount;

    public bool isGlowing = false;

    public async UniTask Glowing()
    {
        isGlowing = true;
        await UniTask.Delay(TimeSpan.FromSeconds(_glowingTime));
        _collider.enabled = true;
        _cuttingPart.SetActive(true);
        isGlowing = false;
    }

    public int CutTree()
    {
        Debug.Log("Cut Tree!");
        _collider.enabled = false;
        _cuttingPart.SetActive(false);
        Glowing().Forget();
        return _getPlankAmount;
    }
}
