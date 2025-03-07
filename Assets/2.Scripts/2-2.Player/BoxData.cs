using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[SerializeField]
public class BoxData 
{
    public GameObject boxObject;

    //박스 현재 체력
    public float currentHp;

    //박스 최대 체력
    public float maxHp;

    //박스 레벨 상태
    public int boxLevel;

    //레벨업 하는데 들어가는 비용
    public int levelUpMoney;

    //박스 Hp UI 보여주는 Slider
    public Slider hpBoxSlider;

    public Sprite sprite;

    //초기 박스 데이터
    public BoxData(GameObject obj, float health, int level, Slider hpSlider)
    {
        boxObject = obj;
        maxHp = health;
        currentHp = maxHp;
        boxLevel = level;
        hpBoxSlider = hpSlider;
        hpBoxSlider.maxValue = maxHp;
        hpBoxSlider.value = currentHp;
    }
}