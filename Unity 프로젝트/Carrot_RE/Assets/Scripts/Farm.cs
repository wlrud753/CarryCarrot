using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm
{
    public Farm()
    {

    }
    public Farm(int _id = 0, int _lv = 0, int _earning = 0, float _planttime = 0f)
    {
        // 객체 이름만 가지고 정보 다 계산/읽어서 설정하도록..

        this.FarmID = _id;
        this.FarmLV = _lv;
        this.FarmEarning = _earning;
        this.PlantTime = _planttime;
    }

    int FarmID;
    int FarmLV;

    int FarmEarning;
    float PlantTime;

    public void LvUp()
    {
        this.FarmID++;
        // Compute Earning, Time
    }

    #region Getter
    public int getID()
    {
        return this.FarmID;
    }
    public int getLV()
    {
        return this.FarmLV;
    }
    public int getEarning()
    {
        return this.FarmEarning;
    }
    public float getPlantTime()
    {
        return this.PlantTime;
    }
    #endregion
}
