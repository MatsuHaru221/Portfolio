using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;

public class TitleText : MonoBehaviour
{
    [SerializeField] float moveRange = 10f;
    [SerializeField] float moveTime = 1f;
    [SerializeField] float moveSpeed = 1f;

    float firstPos, lastPos;
    private float elapsedTime;

    void Start()
    {
        firstPos = transform.localPosition.y - moveRange;
        lastPos = transform.localPosition.y + moveRange;
        StartCoroutine(Moving());
    }

    IEnumerator Moving()
    {
        while (true)
        {
            elapsedTime = 0f;
            while (transform.position.y < firstPos)
            {
                elapsedTime += moveSpeed * Time.deltaTime;
                float currentRate = elapsedTime / moveTime;
                transform.position = new Vector3(0,Mathf.Lerp(lastPos, firstPos, currentRate), 0f);
                yield return new WaitForSeconds(0.1f);
            }
            transform.position = new Vector2(transform.position.x, lastPos);
            elapsedTime = 0f;
            while (transform.position.y > lastPos)
            {
                elapsedTime += moveSpeed * Time.deltaTime;
                float currentRate = elapsedTime / moveTime;
                transform.position = new Vector3(0, Mathf.Lerp(firstPos, lastPos, currentRate), 0f);
                yield return new WaitForSeconds(0.1f);
            }
            transform.position = new Vector2(transform.position.x, firstPos);
        }
    }
}