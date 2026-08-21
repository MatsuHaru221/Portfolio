using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    [SerializeField] private bool _isSoloGame = false;
    [SerializeField, Header("�J�ڐ�̃V�[����String")]
    private string _sceneName;
    [SerializeField] private bool _isP1Exit = false;
    [SerializeField] private bool _isP2Exit = false;

    [SerializeField] private List<TextMeshProUGUI> _waitingText = new List<TextMeshProUGUI>();
    private int _waitingCount = 0;

    [SerializeField] FadeSceneLoder _fadeSceneLoder;

    public void P1OnClicked()
    {
        _isP1Exit = !_isP1Exit;
    }

    public void P2OnClicked()
    {
        _isP2Exit = !_isP2Exit;
    }

    void Update()
    {
        ChangeWaitingText();
        _waitingCount = (_isP1Exit ? 1 : 0) + (_isP2Exit ? 1 : 0);

        if (_isSoloGame)
        {
            if(_waitingCount >= 1)
            {
                _fadeSceneLoder.CallFadeAndLoad(_sceneName);
            }
        }
        else
        {
            if (_isP1Exit && _isP2Exit)
            {
                _fadeSceneLoder.CallFadeAndLoad(_sceneName);
            }
        }
        
    }

    private void ChangeWaitingText()
    {
        if (_isSoloGame)
        {
            for (int i = 0; i < _waitingText.Count; i++)
            {
                _waitingText[i].text = "";
            }
        }
        else
        {
            for(int i = 0; i < _waitingText.Count; i++)
            {
                _waitingText[i].text = $"Waiting ( {_waitingCount}/2 ) ...";
            }
        }
    }
}
