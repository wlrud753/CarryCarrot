using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSystem : MonoBehaviour
{
    PlayerInfo playerInfo;

    int ClickEarning;

    // Start is called before the first frame update
    void Start()
    {
        playerInfo = GameObject.Find("Player Info").GetComponent<PlayerInfo>();

        ClickEarning = 5;
    }

    private void OnMouseDown()
    {
        Click();
    }

    public void Click()
    {
        playerInfo.getCarrot(ClickEarning);
    }
}
