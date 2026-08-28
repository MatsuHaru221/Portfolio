using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.s_isThroughPoint = true;
            // Debug.Log(PlayerController.s_isThroughPoint);
        }
    }
}
