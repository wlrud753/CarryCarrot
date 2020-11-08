using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmWindowClosingArea : MonoBehaviour
{
    FarmInfoWindow FarmInfo;

    private void Start()
    {
        FarmInfo = GameObject.Find("Farm Info").GetComponent<FarmInfoWindow>();
    }

    private void OnMouseDown()
    {
        FarmInfo.Close();
    }
}
