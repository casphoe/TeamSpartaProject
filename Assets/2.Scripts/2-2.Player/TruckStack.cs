using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckStack : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            EnemyManager.instance.StackEnemy(collision.gameObject.GetComponent<Enemy>(), collision.transform.position);
        }
    }
}
