using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator anim;
    //공격하고 있는 박스 오브젝트
    GameObject targetBox;

    Rigidbody2D rigid;

    //자신이 EnemyManager enemyList 리스트에 몇번째 인덱스인지 알 수 있게 해주는 변수
    int index = 0;

    //박스한테 데미지를 입혔는지 판단 하기 위한 변수
    bool isDamage = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        if (!GameManager.instance.isGameOver && !GameManager.instance.isPause)
        {
            Move();
        }
    }


    public void SetIndex(int _index)
    {
        index = _index;
    }

    private void Move()
    {
        if (EnemyManager.instance.enemyList[index].state == EnemyState.Run)
        {
            anim.SetBool("IsIdle", true);
            anim.SetBool("IsAttacking", false);
            anim.SetBool("IsDead", false);
            transform.position += Vector3.left * EnemyManager.instance.enemyList[index].moveSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Truck"))
        {         
            EnemyManager.instance.enemyList[index].state = EnemyState.Idle;
            StopMoving();            
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {     
        if (collision.gameObject.CompareTag("Box"))
        {          
            targetBox = collision.gameObject;
            EnemyManager.instance.enemyList[index].state = EnemyState.Attack;
            anim.SetBool("IsIdle", false);
            anim.SetBool("IsAttacking", true);
            anim.SetBool("IsDead", false);

            isDamage = true;      
        }
    }

    private void StopMoving()
    {
        rigid.velocity = Vector2.zero;
    }

    public void AttackEvent()
    {
        if (targetBox != null && isDamage == true)
        {
            int boxIndex = BoxManager.instance.GetBoxIndex(targetBox);

            if (boxIndex != -1)
            {
                //BoxManager.instance.DamageBox(boxIndex, EnemyManager.instance.enemyList[index].damage);
            }
        }
    }

    public void ResetAttakEvent()
    {
        isDamage = false;
    }
}
