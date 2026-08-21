using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDisplayManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }
}
