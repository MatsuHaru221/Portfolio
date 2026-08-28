using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField, Header("true‚È‚çfront, false‚È‚çback")]
    private bool _witchGoal;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerController.s_isThroughPoint)
            {
                PlayerController.s_gameRapNum++;
                PlayerController.s_isThroughPoint = false;
                // Debug.Log(PlayerController.s_gameRapNum);
            }
        }
    }
}
