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

    float stackSpacing = 0.8f;
    Vector3 statckPosition;

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
            Transform spawnPoint = createPoistion[Random.Range(0, createPoistion.Length)];
            GetObject(inactiveEnemy, spawnPoint.position, Quaternion.identity);
        }
    }
    #endregion

    #region 적 쌓이고 순환되는 함수
    public void StackEnemy(Enemy enemy, Vector3 collisionPoint)
    {
        if (stackEnemyList.Count == 0)
        {
            statckPosition = collisionPoint;
            enemy.transform.position = statckPosition; // 바로 정지
        }
        else
        {
            // 앞 몬스터의 위치를 기준으로 위로 이동
            Vector3 previousEnemyPos = stackEnemyList[stackEnemyList.Count - 1].transform.position;
            statckPosition = previousEnemyPos + new Vector3(0, stackSpacing, 0);

            // 부드러운 애니메이션으로 위로 이동
            enemy.transform.DOMove(statckPosition, 0.3f).SetEase(Ease.OutQuad);
        }

        stackEnemyList.Add(enemy);
    }
    #endregion
}
