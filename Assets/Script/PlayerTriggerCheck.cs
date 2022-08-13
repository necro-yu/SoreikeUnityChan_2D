using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerCheck : MonoBehaviour
{
    // �ڐG�t���O
    [HideInInspector] public bool isOn = false;

    private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == playerTag)
        {
            isOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == playerTag)
        {
            isOn = false;
        }
    }
}
