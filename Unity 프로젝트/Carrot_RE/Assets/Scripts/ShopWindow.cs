using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopWindow : MonoBehaviour
{
    GameObject shopButton;
    GameObject shopWindow;

    Vector2 ButtonPos, CenterPos;

    WaitForSeconds wait, Delay;

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
        Delay = new WaitForSeconds(0.08f);

        shopWindow.transform.localScale = new Vector3(0f, 1f, 1f);
    }

    bool openSwitch;
    public void Open()
    {
        openSwitch = false;
        // 버튼 휘리릭
        //StartCoroutine(RotateOpenButton());
        StartCoroutine(calcArcVec_Open());

        StartCoroutine(OpenWindow());
    }
    bool closeSwitch;
    public void Close()
    {
        closeSwitch = false;
        StartCoroutine(CloseWindow());
        StartCoroutine(calcArcVec_Close());
        //StartCoroutine(RotateCloseButton());
        // 버튼 호로록
    }

    #region ButtonAnim
    // Button Rotation
    // Time.deltaTime * 240 < 90 동안 360도 회전
    IEnumerator RotateOpenButton()
    {
        Vector3 zRot = new Vector3(0f, 0f, -12f);
        for(float r = 0f; r < 90/60f; r += Time.deltaTime * 4) // Time.deltaTime 기준이라.. 횟수가 일정하지 않네
        {
            shopButton.transform.Rotate(zRot);
            yield return wait;
        }
        shopButton.transform.Rotate(zRot);
        yield return wait;
    }
    IEnumerator RotateCloseButton()
    {
        yield return new WaitUntil(() => closeSwitch == true);

        Vector3 zRot = new Vector3(0f, 0f, 12f);
        for (float r = 0f; r < 90 / 60f; r += Time.deltaTime * 4)
        {
            shopButton.transform.Rotate(zRot);
            yield return wait;
        }
        shopButton.transform.Rotate(zRot);
        yield return wait;
    }

    // Button Position
    /*
     * 타원 방정식
     * x = Center-of-Oval.x* sin t, y = Center - of - Oval.y * cos t (0 <= t< 2pi)
    */
    Vector2 arcVec = new Vector2();
    const float d2r = Mathf.Deg2Rad;
    float standardTime, degree; Vector3 zrot = new Vector3(0f, 0f, 0f);
    IEnumerator calcArcVec_Open()
    {
        for (float d = 90f; d >= 0f; d -= Time.deltaTime * 240) // 0.375초간 이동
        {
            arcVec.x = ButtonPos.x * Mathf.Sin(d * d2r);
            arcVec.y = (CenterPos.y - ButtonPos.y) * Mathf.Cos(d * d2r) - 550f;
            shopButton.transform.position = arcVec;
            yield return wait;
        }
        yield return Delay;
        openSwitch = true;
    }
    IEnumerator calcArcVec_Close()
    {
        yield return new WaitUntil(() => closeSwitch == true);

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
        yield return new WaitUntil(() => openSwitch == true);

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

        yield return Delay;
        closeSwitch = true;
    }
    #endregion
}
