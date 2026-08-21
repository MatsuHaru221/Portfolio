using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CardController : MonoBehaviour
{
    [SerializeField] private GameObject _holeCardUI;
    [SerializeField] private GameObject _policeCardUI;
    [SerializeField] private GameObject _barricadeCardUI;
    [SerializeField] private GameObject _maxHeight;
    [SerializeField] private GameObject _minHeight;


    void Update()
    {
        if (StaticManager.s_isHoleCard)
        {
            _holeCardUI.transform.position = new Vector2(_holeCardUI.transform.position.x, _maxHeight.transform.position.y);
        }
        else if(!StaticManager.s_isHoleCard)
        {
            _holeCardUI.transform.position = new Vector2(_holeCardUI.transform.position.x, _minHeight.transform.position.y);
        }

        if (StaticManager.s_isPoliceCard)
        {
            _policeCardUI.transform.position = new Vector2(_policeCardUI.transform.position.x, _maxHeight.transform.position.y);
        }
        else if (!StaticManager.s_isPoliceCard)
        {
            _policeCardUI.transform.position = new Vector2(_policeCardUI.transform.position.x, _minHeight.transform.position.y);
        }

        if (StaticManager.s_isBarricadeCard)
        {
            _barricadeCardUI.transform.position = new Vector2(_barricadeCardUI.transform.position.x, _maxHeight.transform.position.y);
        }
        else if (!StaticManager.s_isBarricadeCard)
        {
            _barricadeCardUI.transform.position = new Vector2(_barricadeCardUI.transform.position.x, _minHeight.transform.position.y);
        }
    }
}
