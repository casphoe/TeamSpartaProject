using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class TruckData 
{
    public float moveSpeed;

    //적이 몇명 도달하면 움직임을 멈춤
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
