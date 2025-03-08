using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class TruckData 
{
    public float moveSpeed;

    //���� ��� �����ϸ� �������� ����
    public int maxEnemyReachCount;

    public int currentEnemyReachCount;

    public Transform[] wheelTrans;

    public float wheelAngleSpeed;

    public TruckData(float _moveSpeed, int _reachCount, Transform[] wheel, float angleSpeed)
    {
        moveSpeed = _moveSpeed;
        maxEnemyReachCount = _reachCount;
        wheelTrans = wheel;
        wheelAngleSpeed = angleSpeed;
    }
}
