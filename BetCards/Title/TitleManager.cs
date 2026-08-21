using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    [SerializeField] string _loadSceneName;

    void Start()
    {
        SoundManager.Instance.PlayBGM(BGM.BGM_Title);
    }

    public void OnClickStartButton()
    {
        FadeManager.Instance.FadeOut(SceneName.Main);
    }
    
    public void OnCreditButton()
    {
        
    }
}
