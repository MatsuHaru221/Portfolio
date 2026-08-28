using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField, Header("‘JˆÚæ‚ÌƒV[ƒ“‚ÌString")]
    private string _nextScene;

    public void OnPointerDown()
    {
        SceneManager.LoadScene(_nextScene);
    }
}
