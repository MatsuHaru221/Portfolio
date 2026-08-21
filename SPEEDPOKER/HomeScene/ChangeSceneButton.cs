using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ChangeSceneButton : MonoBehaviour // , IPointerClickHandler
{
    [SerializeField, Header("遷移先のシーン名")] 
    private string _sceneName;

    [SerializeField] FadeSceneLoder _fadeSceneLoder;

    public void OnPointerClick()
    {
        // Debug.Log("clicked");
        SoundManager.Instance.PlaySE(2);
        if (_sceneName == null)
        {
            Debug.Log("scenename naiyo");
            return;
        }
        else
        {
            _fadeSceneLoder.CallFadeAndLoad(_sceneName);
        }

    }
}