using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("プレイヤ―の移動速度")]
    private float _movePower;

    [SerializeField, Header("プレイヤーの最高速度")]
    private float _maxMovePower = 200f;

    [SerializeField, Header("横回転の力")]
    private float _rotationPower;

    [SerializeField, Header("横回転速度の制限")]
    private float _maxRotationPower = 0.5f;

    [SerializeField, Header("ラップ数表示のテキスト")]
    private TextMeshProUGUI _rapText;

    public static bool s_isThroughPoint = false;
    public static int s_gameRapNum = 1;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        Debug.Log(_rigidbody);
    }

    private void FixedUpdate()
    {
        _rapText.text = $"Rap {s_gameRapNum} / 3";
        if(s_gameRapNum >= 4)
        {
            SceneManager.LoadScene("Result");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            this.transform.rotation = Quaternion.Euler(transform.rotation.x, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _rigidbody.AddForce(transform.forward * _movePower * 1.5f, ForceMode.Impulse);
        }

        UpdateMoving();
        UpdateRotating();
    }

    private void UpdateMoving()
    {
        float nowSpeed = _rigidbody.velocity.sqrMagnitude;
        if (nowSpeed > _maxMovePower) { return; }

        if (Input.GetKey(KeyCode.W))
        {
            _rigidbody.AddForce(transform.forward * _movePower, ForceMode.Force);
        }

        if (nowSpeed > _maxMovePower * 0.2f) { return; }
        if (Input.GetKey(KeyCode.S))
        {
            _rigidbody.AddForce(-transform.forward * _movePower, ForceMode.Force);
        }
    }

    private void UpdateRotating()
    {
        float nowRotSpeed = _rigidbody.angularVelocity.sqrMagnitude;
        if(nowRotSpeed > _maxRotationPower) { return; }

        if (Input.GetKey(KeyCode.A) == true)
        {
            _rigidbody.AddTorque(-transform.up * _rotationPower, ForceMode.Force);
        }

        if (Input.GetKey(KeyCode.D) == true)
        {
            _rigidbody.AddTorque(transform.up * _rotationPower, ForceMode.Force);
        }
    }
}
