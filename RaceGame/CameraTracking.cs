using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraTracking : MonoBehaviour
{
    [SerializeField, Header("プレイヤーのTransform")]
    private Transform _target;

    [SerializeField, Header("カメラとプレイヤーの距離")]
    private Vector3 _cameraOffset = new Vector3(0, 5, -10);

    [SerializeField, Header("追従のスムーズさ")]
    private float _cameraFollowSpeed = 10f;

    [SerializeField, Header("回転のスムーズさ")]
    private float _cameraRotationSpeed = 5f;

    void Update()
    {
        Vector3 desiredPosition = _target.TransformPoint(_cameraOffset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, _cameraFollowSpeed * Time.deltaTime);

        Quaternion desiredRotation = Quaternion.LookRotation(_target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _cameraRotationSpeed * Time.deltaTime);
    }
}
