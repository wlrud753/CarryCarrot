using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopWindow : MonoBehaviour
{
    GameObject shopButton;
    GameObject shopWindow;

    Vector2 ButtonPos, CenterPos;

    WaitForSeconds wait;

    void Start()
    {
        shopWindow = GameObject.Find("Shop").transform.Find("Shop Window").gameObject;

        float xPos, yPos;
        xPos = 400;
        yPos = shopWindow.transform.Find("Background").GetComponent<BoxCollider2D>().bounds.size.y / 2;
        ButtonPos = new Vector2(300f, -550f);
        CenterPos = new Vector2(xPos, yPos);

        wait = new WaitForSeconds(0.001f);

    }

    public void Open()
    {
        // 버튼 휘리릭
        StartCoroutine(OpenWindow());
    }
    public void Close()
    {
        StartCoroutine(CloseWindow());
        // 버튼 호로록
    }

    #region ButtonAnim
    IEnumerator ButtonOpen()
    {

    }
    IEnumerator ButtonClose()
    {

    }
    #endregion

    #region WindowAnim
    Vector2 scaleVec = Vector2.up;
    IEnumerator OpenWindow()
    {
        scaleVec.x = 0f;
        while(scaleVec.x < 1f)
        {
            shopWindow.transform.localScale = scaleVec;
            scaleVec.x += (1 / 4f);
            yield return wait;
        }
        scaleVec.x = 1f;
        shopWindow.transform.localScale = scaleVec;
    }
    IEnumerator CloseWindow()
    {
        scaleVec.x = 1f;
        while (scaleVec.x > 0f)
        {
            shopWindow.transform.localScale = scaleVec;
            scaleVec.x -= (1 / 4f);
            yield return wait;
        }
        scaleVec.x = 0f;
        shopWindow.transform.localScale = scaleVec;
    }
    #endregion
}
