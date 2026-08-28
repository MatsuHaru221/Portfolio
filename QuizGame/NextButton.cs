using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NextButton : MonoBehaviour
{
    [SerializeField, Header("GameManagerのアタッチ")]
    private GameManager _gameManager;

    public void OnPointerDown()
    {
        _gameManager.NextQuestion();
    }
}
