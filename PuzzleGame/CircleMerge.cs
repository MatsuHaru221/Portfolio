using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMerge : MonoBehaviour
{
    [SerializeField] private GameObject _nextSize;
    [SerializeField] private Transform _circleBox;
    [SerializeField] private int _addScorePoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.GetInstanceID() < collision.gameObject.GetInstanceID())
        {
            if (collision.gameObject.tag == "6" && this.gameObject.tag == "6")
            {
                StaticManager.s_score += _addScorePoints;
                Destroy(collision.gameObject);
                Destroy(this.gameObject);
            }
            else if (collision.gameObject.tag == this.gameObject.tag)
            {
                CircleMerge circle = Instantiate(_nextSize, transform.position, Quaternion.identity, _circleBox).GetComponent<CircleMerge>();
                circle.SetParentTransform(_circleBox);
                StaticManager.s_score += _addScorePoints;
                Destroy(collision.gameObject);
                Destroy(this.gameObject);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetParentTransform(Transform parentTransform)
    {
        _circleBox = parentTransform;
    }
}
