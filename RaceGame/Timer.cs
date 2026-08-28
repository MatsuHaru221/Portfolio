using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI m_TextMeshProUGUI;
    public static float s_timer = 0;

    void Update()
    {
        s_timer += Time.deltaTime;
        m_TextMeshProUGUI.text = $"Time:{s_timer.ToString("F2")}";
    }
}
