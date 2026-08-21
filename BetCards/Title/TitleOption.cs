using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleOption : MonoBehaviour
{
    [SerializeField] private Image _optionPanel;
    [SerializeField] private GameObject _hidePos;
    [SerializeField] private GameObject _displayPos;
    [SerializeField] private float _animationSpeed = 0.3f;
    private bool _isTrue = false;

    public void OnTouchedOption()
    {
        _isTrue = !_isTrue;

        if(_isTrue)DisplayAnimation();
        else HideAnimation();
    }

    private void DisplayAnimation()
    {
        _optionPanel.DOKill(this);
        _optionPanel.transform.DOMoveY(_displayPos.transform.position.y, _animationSpeed)
                                .SetEase(Ease.OutQuad)
                                .OnComplete(() => _isTrue = true);
    }

    private void HideAnimation()
    {
        _optionPanel.DOKill(this);
        _optionPanel.transform.DOMoveY(_hidePos.transform.position.y, _animationSpeed)
                                .SetEase(Ease.OutQuad)
                                .OnComplete(() => _isTrue = false);
    }
}
