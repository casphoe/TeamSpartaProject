using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//게임 매니저,시작,종료,타워 이동 등 게임에 대한 것을 관리
public class GameManager : MonoBehaviour
{
    //타워 박스 Hp;
    [SerializeField] Image[] imgBoxHpUi;

    [SerializeField] Image imgPlayerHpUi;

    public bool isPause = false;
    public bool isGameOver = false;

    [SerializeField] float towerMovePower = 5;

    public GameObject towerObject;

    //싱들톤 한 메모리 할당으로 스크립트 기능을 가져올 수 있기에 사용
    public static GameManager instance;
    //Start문 보다 먼저 실행되는 함수
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }     
    }

    private void Update()
    {
        //둘다 false 상태일 때 움직임
        if(!isPause && !isGameOver)
        {
            TowerMove();
        }
    }
    //타워 이동함수
    void TowerMove()
    {
        //x축으로 이동
        towerObject.transform.position += Vector3.right * towerMovePower * Time.deltaTime;
    }
}
