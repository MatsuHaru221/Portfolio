using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks.Triggers;

public class ArrowButton : MonoBehaviour
{
    [SerializeField, Header("子のボタンアタッチ")]
    private List<GameObject> _upDownArrows = new List<GameObject>();

    [SerializeField, Header("ルーレットの親アタッチ")]
    private GameObject _rouletteObject;

    [SerializeField, Header("ルーレットの回転時間")]
    private float _rouletteRotDuration;

    [SerializeField, Header("ルーレットの回転角度")]
    private float _rouletteRotAngle;

    [SerializeField, Header("全ボタンのアタッチ 0,GP 1,CM 2,OP 3,OM")]
    private List<Button> _buttons = new List<Button>();

    [SerializeField, Header("全ボタンに付属しているスプライト 0,GP 1,CM 2,OP 3,OM")]
    private List<Image> _buttonsImage = new List<Image>();

    [SerializeField, Header("全ボタンのGameObjectのアタッチ")]
    private List<GameObject> _buttonsObj = new List<GameObject>();

    [SerializeField, Header("ハイライト時のボタンサイズ ")]
    private Vector2 _hiLightedSize = new Vector2(1000, 200);

    [SerializeField, Header("ハイライト時のスプライト 0,GP 1,CM 2,OP 3,OM")]
    private List<Sprite> _hiLightedSprite = new List<Sprite>();

    [SerializeField, Header("通常時のボタンサイズ")]
    private Vector2 _defaultSize = new Vector2(900, 150);

    [SerializeField, Header("通常時のスプライト 0,GP 1,CM 2,OP 3,OM")]
    private List<Sprite> _defaultSprite = new List<Sprite>();

    [SerializeField, Header("各ボタンの説明スプライト")]
    private List<Sprite> _explainSprites = new List<Sprite>();
    [SerializeField] private Image _explainImage;

    public int _targetCenterIndex = 0;
    private bool _isSpining = false;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(0);

        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].interactable = false;
            // _buttonsImage[i].color = new Color(1f, 1f, 1f, 0.5f);
        }

        _buttons[0].interactable = true;
        // _buttonsImage[0].color = new Color(1f, 1f, 1f, 1f);
        _buttonsImage[0].sprite = _hiLightedSprite[0];
        RectTransform rectTransform = _buttonsObj[0].GetComponent<RectTransform>();
        rectTransform.sizeDelta = _hiLightedSize;
    }

    public async void OnPointerDownUP()
    {
        if(_isSpining) return;
        SoundManager.Instance.PlaySE(1);
        if(_targetCenterIndex == 0)
        {
            _targetCenterIndex = 11;
        }
        else
        {
            _targetCenterIndex--;
        }
        ChangeSpriteUp();
        await UpRotate();
    }

    public async void OnPointerDownDown()
    {
        if (_isSpining) return;
        SoundManager.Instance.PlaySE(1);
        if ((_targetCenterIndex == 11))
        {
            _targetCenterIndex = 0;
        }
        else
        {
            _targetCenterIndex++;
        }
        ChangeSpriteDown();
        await DownRotate();
    }

    private async UniTask UpRotate()
    {
        _isSpining = true;
        float currentZ = _rouletteObject.transform.eulerAngles.z;
        // Debug.Log(currentZ);
        float rotateZ = currentZ + _rouletteRotAngle;
        // Debug.Log(rotateZ);
        _rouletteObject.transform.DORotate(new Vector3(0, 0, rotateZ), _rouletteRotDuration, RotateMode.Fast)
            .SetEase(Ease.OutCubic);
        await UniTask.Delay(500);
        _rouletteObject.transform.rotation = Quaternion.Euler(0, 0, rotateZ);
        _isSpining = false;
    }

    private async UniTask DownRotate()
    {
        _isSpining = true;
        float currentZ = _rouletteObject.transform.eulerAngles.z;
        // Debug.Log(currentZ);
        float rotateZ = currentZ - _rouletteRotAngle;
        // Debug.Log(rotateZ);
        _rouletteObject.transform.DORotate(new Vector3(0, 0, rotateZ), _rouletteRotDuration, RotateMode.Fast)
            .SetEase(Ease.OutCubic);
        await UniTask.Delay(500);
        _rouletteObject.transform.rotation = Quaternion.Euler(0, 0, rotateZ);
        _isSpining = false;
    }

    private void SizeUp(RectTransform changeButton)
    {
        changeButton.DOSizeDelta(_hiLightedSize, _rouletteRotDuration);
    }

    private void SizeDown(RectTransform changeButton)
    {
        changeButton.DOSizeDelta(_defaultSize, _rouletteRotDuration);
    }

    private int GetSpriteIndex(int index)
    {
        if (index == 0 || index == 4 || index == 8)
            return 0;
        if (index == 1 || index == 5 || index == 9)
            return 1;
        if (index == 2 || index == 6 || index == 10)
            return 2;
        if (index == 3 || index == 7 || index == 11)
            return 3;

        return -1;
    }

    private void SetExplainSprite(int index)
    {
        int spriteIndex = GetSpriteIndex(index);
        _explainImage.sprite = _explainSprites[spriteIndex];
    }

    /// <summary>
    /// ボタンのスプライトの変更
    /// </summary>
    /// <param name="index">targetCenterIndex</param>
    /// <param name="interactable">ボタンのオンオフ</param>
    /// <param name="highlighted">真ん中にいるかどうか</param>
    private void SetButtonState(int index, bool interactable, bool highlighted)
    {
        _buttons[index].interactable = interactable;

        int spriteIndex = GetSpriteIndex(index);
        if (spriteIndex >= 0)
        {
            _buttonsImage[index].sprite = highlighted ? _hiLightedSprite[spriteIndex] : _defaultSprite[spriteIndex];
        }

        RectTransform rect = _buttonsObj[index].GetComponent<RectTransform>();
        if (highlighted)
            SizeUp(rect);
        else
            SizeDown(rect);
    }

    private void ChangeSpriteUp()
    {
        if(_targetCenterIndex == 11)
        {
            SetButtonState(_targetCenterIndex - 11, false, false);
            SetButtonState(_targetCenterIndex, true, true);
        }
        else
        {
            SetButtonState(_targetCenterIndex + 1, false, false);
            SetButtonState(_targetCenterIndex, true, true);
        }
        SetExplainSprite(_targetCenterIndex);
    }

    private void ChangeSpriteDown()
    {
        if (_targetCenterIndex == 0)
        {
            SetButtonState(_targetCenterIndex + 11, false, false);
            SetButtonState(_targetCenterIndex, true, true);
        }
        else
        {
            SetButtonState(_targetCenterIndex - 1, false, false); 
            SetButtonState(_targetCenterIndex, true, true);
        }
        SetExplainSprite(_targetCenterIndex);
    }

}
