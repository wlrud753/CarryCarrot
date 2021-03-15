using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    float WagonMaxHP, WagonNowHP;
    public float WagonSpeed;

    public void setWagonData()
    {
        WagonMaxHP = 100;
        WagonNowHP = WagonMaxHP;
        WagonSpeed = 0.01f;
    }

    public float getMaxHP()
    {
        return WagonMaxHP;
    }
    public float getHP()
    {
        return WagonNowHP;
    }
    public void LossHP(float _loss)
    {
        WagonNowHP -= _loss;
        if(WagonNowHP <= 0)
        {
            // GameOver
            GameObject.Find("Wagon").GetComponent<WagonSystem>().stopMoving();
        }
    }
}
