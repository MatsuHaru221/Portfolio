using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _winLoseText;
    [SerializeField] private string _sceneName;
    [SerializeField] private float _fadeSpeed;

    private void Start()
    {
        if (StaticGameManager.s_isWin)
        {
            _winLoseText.text = "Game Clear!";
        }
        else
        {
            _winLoseText.text = "Game Over!";
        }
    }

    public void OnClicked()
    {
        FadeManager.Instance.LoadScene(_sceneName, _fadeSpeed);
    }
}
