using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Farm : MonoBehaviour
{
    PlayerInfo playerInfo;

    int FarmID;

    int FarmLV;
    public int FarmEarning;

    Slider slider;
    public float PlantTimer;
    float timer;

    GameObject FarmInfoWindow;

    void Start()
    {
        playerInfo = GameObject.Find("Player Info").GetComponent<PlayerInfo>();
        slider = this.transform.Find("Slider").GetComponent<Slider>();
        FarmInfoWindow = GameObject.Find("Farm Info").transform.Find("Farm Info Window").gameObject;

        timer = PlantTimer;

        setSlider();
    }
    void setSlider()
    {
        slider.maxValue = PlantTimer;
    }

    void Update()
    {
        if(timer <= 0)
        {
            timer = PlantTimer;
            playerInfo.getCarrot(FarmEarning);
        }

        timer -= Time.deltaTime;
        slider.value = timer;
    }

    private void OnMouseDown()
    {
        FarmInfoWindow.SetActive(true);
    }
}
