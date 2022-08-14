using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private string groundTag = "Ground";
    private string enemyTag = "Enemy";
    private bool isGround;
    private bool isEnemy;
    private bool isGroundEnter, isGroundStay, isGroundExit;
    private bool isEnemyEnter, isEnemyStay, isEnemyExit;

    // 敵の判定を返すメソッド
    // 接地判定同様
    public bool isEnemyCheck()
    {
        if (isEnemyEnter || isEnemyStay)
        {
            isEnemy = true;
        }
        else if(isEnemyExit)
        {
            isEnemy = false;
        }

        isEnemyEnter = false;
        isEnemyStay = false;
        isEnemyExit = false;
        return isEnemy;
    }

    //接地判定を返すメソッド
    //物理判定の更新毎に呼ぶ必要がある (もっと良い書き方がある?)
    public bool IsGround()
    {
        if (isGroundEnter || isGroundStay)
        {
            isGround = true;
        }
        else if (isGroundExit)
        {
            isGround = false;
        }

        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;
        return isGround;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == groundTag)
        {
            isGroundEnter = true;
        }
        else if (collision.tag == enemyTag)
        {
            isEnemyEnter = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == groundTag)
        {
            isGroundStay = true;
        }
        else if (collision.tag == enemyTag)
        {
            isEnemyStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == groundTag)
        {
            isGroundExit = true;
        }
        else if (collision.tag == enemyTag)
        {
            isEnemyExit = true;
        }
    }
}