using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class TextShaker : MonoBehaviour
{

    [SerializeField] private Image _clickToStartImage;
    [SerializeField] private float moveRange = 10f;
    [SerializeField] private float moveTime = 1f;

    private Transform textTransform;
    private Vector3 initialPos;

    private void Start()
    {
        textTransform = _clickToStartImage.GetComponent<RectTransform>();
        initialPos = textTransform.localPosition;
        StartCoroutine(MoveLoop());
    }

    private IEnumerator MoveLoop()
    {
        Vector3 topPos = initialPos + Vector3.up * moveRange;
        Vector3 bottomPos = initialPos - Vector3.up * moveRange;

        while (true)
        {
            // è„Ç÷à⁄ìÆ
            yield return MoveToPosition(topPos);
            // â∫Ç÷à⁄ìÆ
            yield return MoveToPosition(bottomPos);
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPos)
    {
        Vector3 startPos = textTransform.localPosition;
        float elapsed = 0f;

        while (elapsed < moveTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveTime);
            textTransform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        textTransform.localPosition = targetPos;
    }

    public void ClickToStartFading()
    {
        DOTween.ToAlpha(
            () => _clickToStartImage.color,
            color => _clickToStartImage.color = color,
            0f,
            1f
            );
    }

}