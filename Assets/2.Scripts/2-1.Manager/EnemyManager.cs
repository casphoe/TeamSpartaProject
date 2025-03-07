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
    List<EnemyData> enemyList = new List<EnemyData>();

    int createPoolSize = 15;

    GameObject enemyPrefab;

    private void Awake()
    {
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Monster/ZombieMelee");
        enemyPoolTrans = GameObject.Find("ObjectPool").transform.GetChild(0).transform;
        for (int i = 0; i < createPoolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.parent = enemyPoolTrans;
            enemy.SetActive(false);
            EnemyStateSetting(enemy, 15, 2, 5, enemy.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Slider>(), 2.5f);
        }
    }

    void EnemyStateSetting(GameObject obj, float hp, float attack, int dropValue, Slider hpSlider,float createDelay)
    {
        EnemyData enemyData = new EnemyData(obj, hp, attack, dropValue, hpSlider, createDelay);
        enemyList.Add(enemyData);

    }

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

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, enemyList[0].createCoolTime);
    }

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
}
