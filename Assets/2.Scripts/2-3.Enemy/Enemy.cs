using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator anim;
    //공격하고 있는 박스 오브젝트
    private GameObject targetBox;

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

    private void Start()
    {
        //적들끼리의 물리적 충돌을 무시
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));      
    }

    private void Update()
    {
        if(!GameManager.instance.isGameOver && !GameManager.instance.isPause)
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
        else if (collision.gameObject.CompareTag("Truck") && !EnemyManager.instance.enemyList[index].isStack)
        {
            EnemyManager.instance.enemyList[index].isStack = true;
            EnemyManager.instance.StackEnemy(this, collision.transform.position);
        }
    }

    public void AttackEvent()
    {
        if (targetBox != null)
        {
            int boxIndex = BoxManager.instance.GetBoxIndex(targetBox);

            if (boxIndex != -1)
            {
                BoxManager.instance.DamageBox(boxIndex, EnemyManager.instance.enemyList[index].damage);
            }
        }
    }

    public void ResetAttakEvent()
    {
        isDamage = false;
    }
}
