using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_CollisionCheck : MonoBehaviour
{
    // ê⁄êGÉtÉâÉO
    [HideInInspector] public bool isOn = false;

    private string groundTag = "Ground";
    private string enemyTag = "Enemy";
    private string wallTag = "Wall";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == groundTag || collision.tag == enemyTag || collision.tag == wallTag)
        {
            isOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == groundTag || collision.tag == enemyTag || collision.tag == wallTag)
        {
            isOn = false;
        }
    }
}
