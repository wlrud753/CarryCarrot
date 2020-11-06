﻿using System.Collections;
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
        shopButton = this.transform.Find("Shop Button").gameObject;
        shopWindow = this.transform.Find("Shop Window").gameObject;

        float xPos, yPos;
        xPos = 0;
        yPos = -450;
        //yPos = -shopWindow.transform.Find("Background").GetComponent<BoxCollider2D>().bounds.size.y / 2; Debug.Log(yPos);
        ButtonPos = new Vector2(300f, -550f);
        CenterPos = new Vector2(xPos, yPos);

        wait = new WaitForSeconds(0.001f);

    }

    public void Open()
    {
        // 버튼 휘리릭
        StartCoroutine(calcArcVec_Open());
        StartCoroutine(OpenWindow());
    }
    public void Close()
    {
        StartCoroutine(CloseWindow());
        StartCoroutine(calcArcVec_Close());
        // 버튼 호로록
    }

    #region ButtonAnim

    // Button Rotation
    // 0.375초간 회전...? Time.deltaTime * 240 << 상수로 저장한 다음 일괄적으로 쓸 수 있게 하자.
    IEnumerator ButtonOpen()
    {
        yield return null;
    }
    IEnumerator ButtonClose()
    {
        yield return null;
    }

    // Button Position
    /*
     * 타원 방정식
     * x = Center-of-Oval.x* sin t, y = Center - of - Oval.y * cos t (0 <= t< 2pi)
    */
    Vector2 arcVec = new Vector2();
    const float d2r = Mathf.Deg2Rad;
    IEnumerator calcArcVec_Open()
    {
        for (float d = 90f; d >= 0f; d -= Time.deltaTime * 240) // 0.375초간 이동
        {
            arcVec.x = ButtonPos.x * Mathf.Sin(d * d2r);
            arcVec.y = (CenterPos.y - ButtonPos.y) * Mathf.Cos(d * d2r) - 550f;
            shopButton.transform.position = arcVec;
            yield return wait;
        }
    }
    IEnumerator calcArcVec_Close()
    {
        for (float d = 0f; d <= 90f; d += Time.deltaTime * 240) // 0.375초간 이동
        {
            arcVec.x = ButtonPos.x * Mathf.Sin(d * d2r);
            arcVec.y = (CenterPos.y - ButtonPos.y) * Mathf.Cos(d * d2r) - 550f;
            shopButton.transform.position = arcVec;
            yield return wait;
        }
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
