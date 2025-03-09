using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//게임 매니저,시작,종료,타워 이동 등 게임에 대한 것을 관리
public class GameManager : MonoBehaviour
{
    public bool isPause = false;
    public bool isGameOver = false;
    public bool isMove = false;

    [SerializeField] float towerMovePower = 1;

    public GameObject truckObject;

    //싱들톤 한 메모리 할당으로 스크립트 기능을 가져올 수 있기에 사용
    public static GameManager instance;

    public TruckData truckData;

    //Start문 보다 먼저 실행되는 함수
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        Transform parent = truckObject.transform.GetChild(1).GetChild(0);
        truckData = new TruckData(towerMovePower, 4, parent.GetComponentsInChildren<Transform>().Skip(1).ToArray(), 30);
    }

    private void Update()
    {
        //둘다 false 상태일 때 움직임
        if(!isPause && !isGameOver)
        {
            TowerMove();
        }
    }

    public void EnemyReachCountCalculation(int count)
    {
        truckData.currentEnemyReachCount = count;
    }

    //타워 이동함수
    void TowerMove()
    {
        if(truckData.currentEnemyReachCount >= truckData.maxEnemyReachCount)
        {
            isMove = false;
        }
        else
        {
            isMove = true;
            truckObject.transform.position += Vector3.right * truckData.moveSpeed * Time.deltaTime;
            truckData.wheelTrans[0].transform.Rotate(0, 0, truckData.wheelAngleSpeed);
            truckData.wheelTrans[1].transform.Rotate(0, 0, truckData.wheelAngleSpeed);
        }
    }
}