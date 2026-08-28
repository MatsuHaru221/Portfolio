using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultTime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpro;

    void Start()
    {
        tmpro.text = $"Your Time : {Timer.s_timer.ToString("F2")}";
    }

    public void OnClicked()
    {
        SceneManager.LoadScene("Title");
        Timer.s_timer = 0;
    }
}
