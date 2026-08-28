using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _leftLimit;
    [SerializeField] private GameObject _rightLimit;
    [SerializeField] private float _arrowSpeed;
    [SerializeField] private List<GameObject> _spawnCircles = new List<GameObject>();
    [SerializeField] private Transform _circleBox;
    private int _randomSpawn;
    private int _adjustmentValue;
    [SerializeField] private float _spawnTime;

    private void SpawnCircles()
    {
        CircleMerge circle = Instantiate(_spawnCircles[_adjustmentValue],transform.position, Quaternion.identity, _circleBox).GetComponent<CircleMerge>();
        circle.SetParentTransform(_circleBox);
    }

    void Update()
    {
        _spawnTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && _spawnTime >= 1f)
        {
            this.transform.position = new Vector2(transform.position.x + 0.01f, transform.position.y);
            _randomSpawn = Random.Range(1, (_spawnCircles.Count + 1) * 3);
            _spawnTime = 0f;
            if (_randomSpawn <= 4)
            {
                _adjustmentValue = 0;
            }
            else if (_randomSpawn <= 7)
            {
                _adjustmentValue = 1;
            }
            else if( _randomSpawn == 9)
            {
                _adjustmentValue = 2;
            }
            SpawnCircles();
            // Debug.Log(_randomSpawn);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector2(transform.position.x - _arrowSpeed * Time.deltaTime, transform.position.y);   
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector2(transform.position.x + _arrowSpeed * Time.deltaTime, transform.position.y);
        }

        if(transform.position.x <= _leftLimit.transform.position.x)
        {
            transform.position = new Vector2(_leftLimit.transform.position.x,transform.position.y) ;
        }
        if (transform.position.x >= _rightLimit.transform.position.x)
        {
            transform.position = new Vector2(_rightLimit.transform.position.x, transform.position.y);
        }
    }
}
