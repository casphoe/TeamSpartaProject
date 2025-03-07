using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    Idle,Run,Attack,Dead
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

    public EnemyState state;

    public EnemyData(GameObject _enemy,float hp, float _damage, int _dropMoney, Slider _hpSlider, float _createCoolTime)
    {
        enemyObject = _enemy;
        currentHp = hp;
        maxHp = currentHp;
        damage = _damage;
        dropMoney = _dropMoney;
        hpSlider = _hpSlider;
        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;
        createCoolTime = _createCoolTime;
    }
}
