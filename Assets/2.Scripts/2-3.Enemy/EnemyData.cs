using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    Run,Attack,Dead
}

[SerializeField]
public class EnemyData 
{
    public GameObject enemyObject;

    public float currentHp;

    public float maxHp;

    public float damage;

    public int dropMoney;

    public Slider hpSlider;

    public float createCoolTime;

    public float moveSpeed;

    public EnemyState state;

    public EnemyData(GameObject _enemy, float hp, float _damage, int _dropMoney, Slider _hpSlider, float _createCoolTime, float _speed, EnemyState _state)
    {
        enemyObject = _enemy;
        currentHp = hp;
        maxHp = currentHp;
        damage = _damage;
        dropMoney = _dropMoney;
        hpSlider = _hpSlider;
        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;
        moveSpeed = _speed;
        createCoolTime = _createCoolTime;
        state = _state;
    }
}
