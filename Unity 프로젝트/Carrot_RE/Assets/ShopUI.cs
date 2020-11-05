using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopUI : MonoBehaviour
{
    WaitForSeconds wait;

    Vector3 originVec, downVec;
    TextMeshProUGUI buttonText;

    void Start()
    {
        wait = new WaitForSeconds(0.001f);

        originVec = new Vector3(0, 0, 0);
        downVec = new Vector3(0, -325, 0);

        buttonText = this.transform.Find("Background").transform.Find("Canvas").
            transform.Find("Button").transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void MoveShopUI()
    {
        if (this.gameObject.transform.position.Equals(originVec))
        {
            StartCoroutine(MovePos(originVec, downVec));
            buttonText.text = "▲";
        }
        else
        {
            StartCoroutine(MovePos(downVec, originVec));
            buttonText.text = "▼";
        }
    }

    IEnumerator MovePos(Vector3 startPos, Vector3 targetPos)
    {
        Vector3 tmp;
        float count = 0;
        while (count <= 1)
        {
            count += 0.1f;
            tmp = Vector3.Lerp(startPos, targetPos, count);
            this.transform.position = tmp;
            yield return wait;
        }
    }
}
