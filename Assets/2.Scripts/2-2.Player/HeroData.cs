using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public class HeroData : MonoBehaviour
{
    public float currentHp;

    public float maxHp;

    public float damage;

    public float attackRange;

    public float attackCoolTime;

    public float critcleRate;

    public float critcleDamage;

    public Slider hpSlider;

    public HeroData(float hp, float _damage, float _attackRange, float _attackCoolTime, float _rate,float _critcleDamage,Slider _hpSlider)
    {
        currentHp = hp;
        maxHp = currentHp;
        damage = _damage;
        attackRange = _attackRange;
        attackCoolTime = _attackCoolTime;
        hpSlider = _hpSlider;
        critcleRate = _rate;
        critcleDamage = _critcleDamage;
        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;
    }
}
