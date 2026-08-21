using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using Newtonsoft.Json.Bson;


public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField, Header("全アニメーション")]
    private List<Animator> _titleAnimator = new List<Animator>();
    [SerializeField, Header("アニメーション後の画面遷移までのラグ")]
    private float _changeSceneLagTime = 0f;
    [SerializeField] private string _animationStateName = "TitleAnimation";
    [SerializeField] private TextShaker _textShaker;

    private bool _isClicked = false;

    private void Update()
    {
        if(!_isClicked && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clicked");
            _isClicked = true;
            PlayAnimations().Forget();
        }
    }

    private async UniTaskVoid PlayAnimations()
    {
        List<UniTask> animationTasks = new List<UniTask>();
        float maxDuration = 0f;
        _textShaker.ClickToStartFading();

        // アニメーターに再生させて、その内アニメーション時間が最も長いものを取得
        foreach(var animator in _titleAnimator)
        {
            animator.SetTrigger(_animationStateName);
            Debug.Log(animator.name);
            var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0)
            {
                float length = clipInfo[0].clip.length;
                if(length > maxDuration)
                {
                    maxDuration = length;
                }
            }
        }

        // 最大時間だけ待機してからシーン遷移
        await UniTask.Delay(System.TimeSpan.FromSeconds(maxDuration + _changeSceneLagTime));
        SceneManager.LoadScene(sceneName);
    }

}
