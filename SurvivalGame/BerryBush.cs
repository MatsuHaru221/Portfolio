using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Security.Cryptography;
using System;


public class BerryBush : MonoBehaviour
{
    [SerializeField] private GameObject berry;
    [SerializeField] private BoxCollider2D berryCollider;
    [SerializeField] private float glowTime;
    [SerializeField] private float hungerRecoveryAmount;

    public bool isGlowing = false;

    private async UniTask Glowing()
    {
        isGlowing = true;
        await UniTask.Delay(TimeSpan.FromSeconds(glowTime));
        berry.gameObject.SetActive(true);
        berryCollider.enabled = true;
        isGlowing = false;
    }

    public float EatBerry()
    {
        Debug.Log("eat berry!");
        berry.gameObject.SetActive(false);
        berryCollider.enabled = false;
        Glowing().Forget();
        return hungerRecoveryAmount;
    }
}
