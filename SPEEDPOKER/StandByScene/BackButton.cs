using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    [SerializeField, Header("ƒV[ƒ“‘JˆÚæ‚Ìstring")]
    private string _sceneName;

    public void OnClick()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
