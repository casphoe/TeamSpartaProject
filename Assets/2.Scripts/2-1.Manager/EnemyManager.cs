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

    //적 쌓이고 순환 되는 리스트
    public List<Enemy> stackEnemyList = new List<Enemy>();

    //몬스터 간 간격
    float stackSpacing = 0.5f;

    //트럭과 충돌한 위치
    Vector3 statckPosition;

    int createPoolSize = 15;

    GameObject enemyPrefab;

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

            EnemyStateSetting(enemy, 15, 1, 5, enemy.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Slider>(), 1.5f, 1, EnemyState.Run, false);
        }
    }

    void EnemyStateSetting(GameObject obj, float hp, float attack, int dropValue, Slider hpSlider,float createDelay, float speed, EnemyState state, bool isStack)
    {
        EnemyData enemyData = new EnemyData(obj, hp, attack, dropValue, hpSlider, createDelay, speed, state, isStack);
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

    #region 적 쌓이고 순환 되는 함수
    public void StackEnemy(Enemy enemy, Vector3 collisionPoint)
    {
        if (stackEnemyList.Count == 0)
            statckPosition = collisionPoint;
        stackEnemyList.Add(enemy);
        RearrangeStack();
    }

    private void RearrangeStack()
    {
        GameManager.instance.truckData.currentEnemyReachCount = stackEnemyList.Count;
        for (int i = 0; i < stackEnemyList.Count; i++)
        {
            stackEnemyList[i].transform.position = statckPosition + Vector3.up * (i * stackSpacing);
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
    #endregion
}
