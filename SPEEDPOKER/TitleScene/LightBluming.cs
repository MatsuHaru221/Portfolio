using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LightBluming : MonoBehaviour
{
    [SerializeField, Header("�������g���A�^�b�`")]
    private SpriteRenderer _lightSprite;

    [SerializeField, Header("����a�l�̐U�ꕝ")]
    private float _lightBloom = 0.1f;

    [SerializeField, Header("���̗h��鎞��")]
    private float _lightBloomTime = 1f;


    private float _maxLightAlpha;
    private float _minLightAlpha;
    private float _dafaultLightAlpha;

    private void Start()
    {
        _maxLightAlpha = _lightSprite.color.a + _lightBloom;
        _minLightAlpha = _lightSprite.color.a - _lightBloom;
        _dafaultLightAlpha = _lightSprite.color.a;

        StartCoroutine(LightBlooming());
    }

    private IEnumerator LightBlooming()
    {
        while (true)
        {
            float elapsedTime = 0f;
            float progressTime = 0f;
            //Debug.Log("ugoiteru");
            while(_lightSprite.color.a < _maxLightAlpha)
            {
                elapsedTime += Time.deltaTime;
                progressTime = elapsedTime / _lightBloomTime * 10f;
                _lightSprite.color = new Color(_lightSprite.color.r, _lightSprite.color.g, _lightSprite.color.b, Mathf.Lerp(_minLightAlpha, _maxLightAlpha, progressTime));
                //Debug.Log(_lightSprite.color.a);
                yield return new WaitForSeconds(0.1f);
            }

            _lightSprite.color = new Vector4(_lightSprite.color.r, _lightSprite.color.g, _lightSprite.color.b, _maxLightAlpha);
            elapsedTime = 0f;
            progressTime = 0f;

            while (_lightSprite.color.a > _minLightAlpha)
            {
                elapsedTime += Time.deltaTime;
                progressTime = elapsedTime / _lightBloomTime * 10;
                _lightSprite.color = new Color(_lightSprite.color.r, _lightSprite.color.g, _lightSprite.color.b, Mathf.Lerp(_maxLightAlpha, _minLightAlpha, progressTime));
                //Debug.Log(_lightSprite.color.a);
                yield return new WaitForSeconds(0.1f);
            }

            _lightSprite.color = new Vector4(_lightSprite.color.r, _lightSprite.color.g, _lightSprite.color.b, _minLightAlpha);
        }

    }

}
