using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public class EnemyData 
{
    public float currentHp;

    public float maxHp;

    public float damage;

    public int dropMoney;

    public Slider hpSlider;

    public float createCoolTime;

    public EnemyData(float hp, float _damage, int _dropMoney, Slider _hpSlider, float _createCoolTime)
    {
        currentHp = hp;
        maxHp = currentHp;
        damage = _damage;
        dropMoney = _dropMoney;
        hpSlider = _hpSlider;
        hpSlider.value = currentHp;
        hpSlider.maxValue = maxHp;
        createCoolTime = _createCoolTime;
    }
}
