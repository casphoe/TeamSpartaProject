using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//박스를 관리하는 스크립트 입니다 추가,제거
public class BoxManager : MonoBehaviour
{
    GameObject boxPrefab;

    Transform truck;

    //박스 높이
    float boxHeight = 1.5f;

    private float boxMoveSpeed = 3f;

    //박스를 관리하기 위한 리스트
    private List<GameObject> boxes = new List<GameObject>();

    private List<BoxData> boxDataList = new List<BoxData>();

    public static BoxManager instance;

    private void Awake()
    {
        boxPrefab = Resources.Load<GameObject>("Prefabs/Box/Box");
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        truck = GameManager.instance.towerObject.transform;
        for (int i = 0; i < 5; i++)
        {
            AddBox(50, 1);
        }
    }

    //박스 추가 (기존 박스 위에 추가 되는 형식)
    public void AddBox(float hp,int level)
    {
        Vector3 newBoxPoistion = truck.position + Vector3.up * (boxDataList.Count * boxHeight);
        Debug.Log(newBoxPoistion);
        GameObject newBox = Instantiate(boxPrefab, newBoxPoistion, Quaternion.identity, truck.transform.GetChild(0).transform);
        Slider boxHpSlider = newBox.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Slider>();
        boxDataList.Add(new BoxData(newBox, hp, level, boxHpSlider)); //새로운 박스를 리스트 끝에 추가
    }
    
    //Hp가 0인 박스를 제거하는 함수 (Hp 0인 박스 제거)
    public void RemoveBox(BoxData targetBox)
    {
        if (boxDataList.Contains(targetBox))
        {
            boxDataList.Remove(targetBox);
            Destroy(targetBox.boxObject);
            StartCoroutine(RepositionBoxes());
        }
    }

    //박스 Hp 감소 해주는 함수
    public void DamageBox(int index, float damage)
    {
        //boxDataList 인덱스 안에 있는 박스가 아닌 것이 데미지를 받을 경우 return 시켜서 함수가 실행 안되게 넘김
        if (index < 0 || index >= boxDataList.Count) return;

        boxDataList[index].currentHp -= damage;
        boxDataList[index].hpBoxSlider.value -= damage;

        if (boxDataList[index].currentHp <= 0)
        {
            RemoveBox(boxDataList[index]);
        }
    }
    //박스가 제거 되면 자동 정렬 해주는 함수
    private IEnumerator RepositionBoxes()
    {
        bool moving = true;
        while(moving)
        {
            moving = false;
            for(int i = 0; i < boxDataList.Count; i++)
            {
                Vector3 targetPosition = truck.position + Vector3.up * (i * boxHeight);
                boxes[i].transform.position = Vector3.Lerp(boxes[i].transform.position, targetPosition, Time.deltaTime * boxMoveSpeed);

                if (Vector3.Distance(boxes[i].transform.position, targetPosition) > 0.01f)
                    moving = true;
            }
        }
        yield return null;
    }
}