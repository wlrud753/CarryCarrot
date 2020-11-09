using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmManager : MonoBehaviour
{
    PlayerInfo playerInfo;

    Farm farm;

    float timer;
    Slider slider;

    FarmInfoWindow FarmInfo;

    void Start()
    {
        playerInfo = GameObject.Find("Player Info").GetComponent<PlayerInfo>();
        farm = new Farm(0, 0, 10, 2);
        // 요녀석이 달린 객체 이름만 전달 >> Farm Class에서 값 전부 설정.
        // 고렇게 Farm() 초기화 함수 바꾸자구

        slider = this.transform.Find("Slider").GetComponent<Slider>();
        timer = farm.getPlantTime();
        setSlider();

        FarmInfo = GameObject.Find("Farm Info").GetComponent<FarmInfoWindow>();
    }
    void setSlider()
    {
        slider.maxValue = farm.getPlantTime();
    }

    void Update()
    {
        if(timer <= 0)
        {
            timer = farm.getPlantTime();
            playerInfo.getCarrot(farm.getEarning());
        }

        timer -= Time.deltaTime;
        slider.value = timer;
    }

    private void OnMouseDown()
    {
        FarmInfo.Open(this.farm);
    }
}
