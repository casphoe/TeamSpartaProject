using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//적 생성, 관리 하는 스크립트
public class EnemyManager : MonoBehaviour
{
    [SerializeField] Transform[] createPoistion;

    Transform enemyPoolTrans;

    //적을 관리할 리스트
    public List<EnemyData> enemyList = new List<EnemyData>();

    public List<Enemy> stackEnemyList = new List<Enemy>();

    int createPoolSize = 25;

    GameObject enemyPrefab;

    float stackSpacing = 1f;
    Vector3 stackPosition;

    public static EnemyManager instance;

    private void Awake()
    {
        instance = this;
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Monster/ZombieMelee");
        enemyPoolTrans = GameObject.Find("ObjectPool").transform.GetChild(0).transform;
        for (int i = 0; i < createPoolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.parent = enemyPoolTrans;
            enemy.SetActive(false);
            enemy.GetComponent<Enemy>().SetIndex(i);

            EnemyStateSetting(enemy, 15, 1, 5, enemy.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Slider>(), 1.5f, 1, EnemyState.Run);
        }
    }

    void EnemyStateSetting(GameObject obj, float hp, float attack, int dropValue, Slider hpSlider,float createDelay, float speed, EnemyState state)
    {
        EnemyData enemyData = new EnemyData(obj, hp, attack, dropValue, hpSlider, createDelay, speed, state);
        enemyList.Add(enemyData);
    }

    #region 오브젝트 풀링
    private void GetObject(EnemyData enemyData, Vector3 position, Quaternion rotation)
    {
        enemyData.enemyObject.transform.position = position;
        enemyData.enemyObject.transform.rotation = rotation;
        enemyData.enemyObject.SetActive(true);
    }

    //죽었을 때 실행되는 함수 (비활성화 시킴)
    public void ReturnObject(EnemyData enemyData)
    {
        enemyData.enemyObject.SetActive(false);
    }
    #endregion

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, enemyList[0].createCoolTime);
    }

    #region 적 생성 함수
    void SpawnEnemy()
    {
        //비활성화 된 적 찾아서 생성해줌
        EnemyData inactiveEnemy = enemyList.Find(e => !e.enemyObject.activeInHierarchy);

        if(inactiveEnemy != null)
        {
            Transform spawnPoint = createPoistion[0];
            GetObject(inactiveEnemy, spawnPoint.position, Quaternion.identity);
        }
    }
    #endregion
    #region 적 쌓이고 순환되는 함수
    public void StackEnemy(Enemy enemy, Vector3 collisionPoint)
    {
        if (stackEnemyList.Count == 0)
        {
            stackPosition = collisionPoint;
        }

        stackEnemyList.Add(enemy);
        RearrangeStack();
    }

    private void RearrangeStack()
    {
        GameManager.instance.truckData.currentEnemyReachCount = stackEnemyList.Count;

        for (int i = 0; i < stackEnemyList.Count; i++)
        {
            Vector3 targetPos = stackPosition + Vector3.up * (i * stackSpacing);
            stackEnemyList[i].transform.DOMove(targetPos, 0.3f);
        }
    }

    public void RemoveStackEnemy(Enemy enemy)
    {
        if (stackEnemyList.Contains(enemy))
        {
            stackEnemyList.Remove(enemy);
            RearrangeStack();
        }
    }

    private IEnumerator CycleStackEnemies()
    {
        while (true)
        {
            if (stackEnemyList.Count > 1)
            {
                Enemy topEnemy = stackEnemyList[stackEnemyList.Count - 1];

                // 스택의 맨 위 몬스터를 순환시키기
                stackEnemyList.RemoveAt(stackEnemyList.Count - 1);
                stackEnemyList.Insert(0, topEnemy);

                // 이동 애니메이션 적용
                RearrangeStack();
            }

            yield return new WaitForSeconds(1f);  // 순환 속도 조절
        }
    }
    #endregion
}
