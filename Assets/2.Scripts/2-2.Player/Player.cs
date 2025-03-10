using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private LineRenderer attackLineInner; // 중앙 흰색 라인 랜더러

    private bool isUiActive = false; //직적 공격 여부 (false이면 자동 공격)

    private Vector3 uiAttackPosition = Vector3.zero;

    private Transform attackPoint; //공격 지점
    private Transform gunAttackPosition; //총구 위치
    public Transform targetAuto; //자동 공격 할 때 타겟 포지션

    private bool isHoldingInput = false;

    HeroData heroData;

    public LayerMask enemyLayer;

    private void Awake()
    {
        heroData = PlayerManager.instance.PlayerGameData();
    }

    private void Start()
    {  
        //Enemy 레이러 값을 찾아서 대입
        enemyLayer = LayerMask.GetMask("Enemy");

        gunAttackPosition = transform.GetChild(0).transform;
        attackLineInner = transform.GetChild(0).GetChild(0).GetComponent<LineRenderer>();
        attackPoint = attackLineInner.transform;
        AttackLineInnerSetting();
    }

    // 라인랜더러 설정 값
    void AttackLineInnerSetting()
    {
        attackLineInner.positionCount = 2; //시작점 ,끝점 설정
        attackLineInner.enabled = false;
        attackLineInner.startColor = new Color(1, 1, 1, 0.6f);
        attackLineInner.endColor = new Color(1, 1, 1, 0.15f);
        attackLineInner.startWidth = 0.1f; //시작 두꼐
        attackLineInner.endWidth = 1f;  //라인 끝 두꼐
    }

    private void Update()
    {
        HandleUIInput();
        if(isUiActive) //조종해서 해당 위치에 공격
        {
            AttackAtUI();
        }
        else //자동 공격
        {
            AutoAttackEnemies();
        }
    }

    void HandleUIInput()
    {
#if UNITY_ANDROID || UNITY_IOS
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            isHoldingInput = true;
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            isHoldingInput = false;
            targetAuto = FindTarget(); // 마우스를 놓을 때 바로 적을 찾음
            if (targetAuto != null)
            {
                RotateGunTowards(targetAuto.position); // 즉시 총구를 돌림
                ToggleAttackUI(targetAuto.position);
            }
        }

        if (isHoldingInput)
        {
            ToggleAttackUI(touch.position);
        }
    }
#else
        if (Input.GetMouseButtonDown(0)) // 마우스 클릭 시작
        {
            isHoldingInput = true;
        }
        else if (Input.GetMouseButtonUp(0)) // 마우스 클릭 해제
        {
            isHoldingInput = false;
            targetAuto = FindTarget();
            if (targetAuto != null)
            {
                RotateGunTowards(targetAuto.position);
                ToggleAttackUI(targetAuto.position);
            }
        }

        if (isHoldingInput)
        {
            ToggleAttackUI(Input.mousePosition);
        }
#endif
    }

    void ToggleAttackUI(Vector2 position)
    {
        isUiActive = !isUiActive;
        attackLineInner.enabled = isUiActive;

        if(isUiActive)
        {
            //화면 좌표를 뷰포트 좌표로 변환
            /*
             * 0,0 왼쪽 아래
             * 1,1 오른쪽 위
             * 
             * 해상도와 관계 없이 동일한 변환이 가능
             */
            Vector3 viewportPoint = Camera.main.ScreenToViewportPoint(position);

            //게임 내 실제 위치로 변환 해주는 기능 카메라가 일반적으로 -z 축이기 때문에 절대값 사용
            Vector3 worldPos = Camera.main.ViewportToWorldPoint(new Vector3(viewportPoint.x, viewportPoint.y, Mathf.Abs(Camera.main.transform.position.z)));
            uiAttackPosition = new Vector3(worldPos.x, worldPos.y, gunAttackPosition.position.z);

            RotateGunTowards(uiAttackPosition);
            DrawAttackLine(uiAttackPosition);
        }
    }

    void RotateGunTowards(Vector3 targetPosition)
    {
        //크기에 관계 없이 방향을 유지 하기 위해 사용 => 크기를 1로 조정하여 방향으로 사용하기 위해 normalized 사용
        Vector3 direction = (targetPosition - attackPoint.position).normalized;

        // 각도를 구함 
        /*
         * Mathf.Atan2 : x,y 백터를 라디안 각도로 변환
         * Mathf.Rad2Deg : 라디안을 도로 변환 시킴
         */
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //z 축 기준으로 회전
        gunAttackPosition.rotation = Quaternion.Euler(0, 0, angle);
    }



    void DrawAttackLine(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - attackPoint.position).normalized;
        Vector3 rangeEnd = attackPoint.position + direction * heroData.attackRange;
        Vector3 offset = new Vector3(0.1f, 0, 0); // 외곽 라인 간격 조정

        //  부모 회전 영향 제거
        attackLineInner.transform.rotation = Quaternion.identity;

        //  라인 시작 및 끝 좌표 설정    
        attackLineInner.SetPosition(0, attackPoint.position);
        attackLineInner.SetPosition(1, rangeEnd);       
    }

    void AutoAttackEnemies()
    {
        if(Time.time >= heroData.nextAttackCoolTime)
        {
            targetAuto = FindTarget();
            if(targetAuto != null)
            {
                attackLineInner.enabled = true;
                RotateGunTowards(targetAuto.position);
                Attack(targetAuto.position);
                heroData.nextAttackCoolTime = Time.time + heroData.attackCoolTime;
            }
            else
            {
                attackLineInner.enabled = false;
            }
        }
    }

    void AttackAtUI()
    {
        if (Time.time >= heroData.nextAttackCoolTime)
        {
            Attack(uiAttackPosition);
            RotateGunTowards(uiAttackPosition);
            DrawAttackLine(uiAttackPosition);
            heroData.nextAttackCoolTime = Time.time + heroData.attackCoolTime;
        }
    }

    Transform FindTarget()
    {
        //현재 위치에서 가까이 있는 적들을 찾는 함수
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, heroData.attackRange, enemyLayer);
        Transform nearestEnemy = null;

        float minDistance = Mathf.Infinity;

        foreach (Collider2D enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if(distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

       return nearestEnemy;
    }

    void Attack(Vector3 position)
    {
        RotateGunTowards(position);
        DrawAttackLine(position);
    }
}