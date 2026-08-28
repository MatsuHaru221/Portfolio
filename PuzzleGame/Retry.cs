using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Retry : MonoBehaviour
{
    [SerializeField] private string _sceneName;

    public void OnPointerClick()
    {
        SceneManager.LoadScene(_sceneName);
        StaticManager.s_score = 0;
    }
}
