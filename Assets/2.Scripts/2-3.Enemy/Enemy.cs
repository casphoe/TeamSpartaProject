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

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
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

        }
    }
}
