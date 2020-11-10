using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSystem : MonoBehaviour
{
    PlayerInfo playerInfo;

    int ClickLV;

    int ClickEarning;

    public float ClickMult;

    // Start is called before the first frame update
    void Start()
    {
        playerInfo = GameObject.Find("Player Info").GetComponent<PlayerInfo>();

        ClickEarning = 5;
        ClickMult = 1; // * User LV 효과
    }

    public void ClickLvUp()
    {
        this.ClickLV++;
        // Compute ClickEarning
    }

    public void MultipleClickEarning(float _mult)
    {
        ClickMult *= _mult;
    }

    private void OnMouseDown()
    {
        Click();
    }

    public void Click()
    {
        playerInfo.getCarrot((int)(ClickEarning * ClickMult));
    }
}
