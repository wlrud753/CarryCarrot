using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmWindowClosingArea : MonoBehaviour
{
    public GameObject FarmInfoGO;

    private void OnMouseDown()
    {
        FarmInfoGO.SetActive(false);
    }
}
