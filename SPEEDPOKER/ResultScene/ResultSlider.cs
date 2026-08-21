using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResultSlider : MonoBehaviour
{
    [SerializeField] private Animator _animator;


    public void PlayerResult()
    {
        _animator.SetTrigger("LeftSlide");
    }

    public void EnemyResult()
    {
        _animator.SetTrigger("RightSlide");
    }
}
