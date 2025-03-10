using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//플레이어 관리 및 플레이어 자동 공격 시 총알 오브젝트 풀링 방식으로 관리하는 스크립트
public class PlayerManager : MonoBehaviour
{
    Player player;

    public static PlayerManager instance;

    GameObject bulletPrefab;

    Transform bullParent;

    int bulletPoolSize = 25;

    List<GameObject> bulletList = new List<GameObject>();

    private void Awake()
    {
        player = GameObject.Find("Hero").GetComponent<Player>();
        bullParent = GameObject.Find("ObjectPool").transform.GetChild(1).transform;
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet/bullet");
        instance = this;
        InitializeBulletPool();
    }

    public HeroData PlayerGameData()
    {
       HeroData _heroData = new HeroData(100, 5, 2.5f, 1.5f, 5, 50, player.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Slider>());

        return _heroData;
    }

    void InitializeBulletPool()
    {
        for (int i = 0; i < bulletPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.SetParent(bullParent);
            bullet.SetActive(false);
            bulletList.Add(bullet);
        }
    }

    // 사용 가능한 총알 반환(없으면 새로 생성)
    public GameObject GetBullet()
    {
        foreach (GameObject bullet in bulletList)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(true);
        bulletList.Add(newBullet);
        return newBullet;
    }
    //비활성화 처리
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }
}