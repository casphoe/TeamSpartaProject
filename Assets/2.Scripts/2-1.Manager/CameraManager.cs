using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] float followTruckSpeed = 1;

    //카메라가 따라갈 오브젝트
    Transform followTarget;

    private void Start()
    {
        followTarget = GameManager.instance.truckObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //게임을 일시 정지 하거나 게임 오버가 되었을 경우 제외 하고 계속 이동
        if (!GameManager.instance.isPause && !GameManager.instance.isGameOver)
        {
            //CameraMove();
        }            
    }

    //트럭이 이동할 때 카메라도 같이 따라서 이동을 해야 함
    void CameraMove()
    {
        if (followTarget != null && GameManager.instance.isMove == true)
        {
            Vector3 newPosition = transform.position;
            // Mathf.MoveTowards : A 에서 B로 Speed 값 만큼 이동 (현재 위치에서 목표 위치 까지 이동)
            newPosition.x = Mathf.MoveTowards(transform.position.x, followTarget.position.x, followTruckSpeed * 0.9f * Time.deltaTime);
            transform.position = newPosition;
        }
    }
}
