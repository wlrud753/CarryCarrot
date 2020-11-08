using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmInfoWindow : MonoBehaviour
{
    GameObject FarmWindow;

    WaitForSeconds wait;
    
    void Start()
    {
        FarmWindow = this.transform.Find("Farm Info Window").gameObject;
        wait = new WaitForSeconds(0.001f);
    }

    public void Open(Farm _farm)
    {
        // Farm 내용으로 text 초기화
        Activate();
    }
    public void Close()
    {
        // text 초기화
        Deactivate();
    }

    #region Text
    #endregion

    #region Act, Deact
    void Activate()
    {
        FarmWindow.SetActive(true);
        StartCoroutine(activeAnim(0));
    }
    void Deactivate()
    {
        StartCoroutine(activeAnim(1));
    }
    IEnumerator activeAnim(int _type) // 0: act, 1: deact
    {
        Vector2 scale = new Vector2(1f, 1f);
        if (_type == 0)
        {
            scale.y = 0f;
            while (scale.y < 1f)
            {
                FarmWindow.transform.localScale = scale;
                scale.y += 1 / 3f;
                yield return wait;
            }
            scale.y = 1f;
        }
        else
        {
            scale.y = 1f;
            while (scale.y > 0f)
            {
                FarmWindow.transform.localScale = scale;
                scale.y -= 1 / 3f;
                yield return wait;
            }
            scale.y = 0f;
            FarmWindow.SetActive(false);
        }
        FarmWindow.transform.localScale = scale;
    }
    #endregion
}
