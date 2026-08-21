using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _pointList = new List<GameObject>();
    [SerializeField] private SpeedManager _speedManager;

    [SerializeField] private GameObject _holeCard;
    [SerializeField] private GameObject _policeCard;
    [SerializeField] private GameObject _barricadeCard;
    [SerializeField] private List<string> _tagName = new List<string>();
    [SerializeField] private List<string> _layerName = new List<string>();

    private Coroutine _playingCoroutine;

    public int _currentNum = 2;

    private void MoveToUp() //  è„ÉåÅ[ÉìÇ÷ÇÃà⁄ìÆ
    {
        _currentNum--;
        if(_currentNum < 0)
        {
            // Debug.Log("up over");
            _currentNum = 0;
            return;
        }
        this.transform.position = _pointList[_currentNum].transform.position;
        this.gameObject.tag = _tagName[_currentNum];
    }

    private void MoveToDown()   //  â∫ÉåÅ[ÉìÇ÷ÇÃà⁄ìÆ
    {
        _currentNum++;
        if(_currentNum > 2)
        {
            // Debug.Log("down over");
            _currentNum = 2;
            return;
        }
        this.transform.position = _pointList[_currentNum].transform.position;
        this.gameObject.tag = _tagName[_currentNum];
    }

    private void Hole()
    {
        // Debug.Log("HoleCard");
        StaticManager.s_isHoleCard = true;
        StaticManager.s_isPoliceCard = false;
        StaticManager.s_isBarricadeCard = false;
        StaticManager.SetIsHandlingCard = true;
    }

    private void Police()
    {
        // Debug.Log("PoliceCard");
        StaticManager.s_isPoliceCard = true;
        StaticManager.s_isBarricadeCard = false;
        StaticManager.s_isHoleCard = false;
        StaticManager.SetIsHandlingCard = true;
    }

    private void Barricade()
    {
        // Debug.Log("Barricade");
        StaticManager.s_isBarricadeCard = true;
        StaticManager.s_isHoleCard = false;
        StaticManager.s_isPoliceCard = false;
        StaticManager.SetIsHandlingCard = true;
    }


     private void OnTriggerEnter2D(Collider2D collision) // è·äQï®Ç…ìñÇΩÇ¡ÇΩéûÇÃå∏ë¨
     {
        if (collision.gameObject.CompareTag("Police"))
        {
            SoundManager.Instance.PlaySE(9);
        }
        if (collision.gameObject.CompareTag("Hole"))
        {
            SoundManager.Instance.PlaySE(8);
        }
        if (collision.gameObject.CompareTag("Barricade"))
        {
            SoundManager.Instance.PlaySE(7);
        }
        // Debug.Log("alwdbawbd");
        if(_playingCoroutine != null)
        {
            StopCoroutine(_playingCoroutine);
        }
        _playingCoroutine = StartCoroutine(_speedManager.ObjectStacking());
        
     }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SoundManager.Instance.PlaySE(1);
            MoveToUp();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SoundManager.Instance.PlaySE(1);
            MoveToDown();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) // HoleCard
        {
            SoundManager.Instance.PlaySE(2);
            Hole();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))  //  PoliceCard
        {
            SoundManager.Instance.PlaySE(2);
            Police();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))   // BarricadeCard
        {
            SoundManager.Instance.PlaySE(2);
            Barricade();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))    // NonChoise
        {
            SoundManager.Instance.PlaySE(2);
            StaticManager.SetIsHandlingCard = false;
            StaticManager.s_isHoleCard = false;
            StaticManager.s_isBarricadeCard = false;
            StaticManager.s_isPoliceCard = false;
        }
    }
}
