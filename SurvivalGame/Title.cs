using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{

    public void Clicked()
    {
        FadeManager.Instance.LoadScene("MainGame", 1f);
    }
}
