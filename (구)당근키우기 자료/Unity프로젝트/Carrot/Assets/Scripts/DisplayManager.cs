using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    Camera cam;

    GameObject helper;

    Vector3 FarmPos, ShopPos;

    WaitForSeconds wait;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        helper = this.transform.Find("Helper").gameObject;
        FarmPos = new Vector3(0, 0, -10f);
        ShopPos = new Vector3(800f, 0, -10f);

        wait = new WaitForSeconds(0.001f);
    }

    public void toShop()
    {
        StartCoroutine(MovePos(cam.transform.position, ShopPos));
    }
    public void toFarm()
    {
        StartCoroutine(MovePos(cam.transform.position, FarmPos));
    }

    IEnumerator MovePos(Vector3 startPos, Vector3 targetPos)
    {
        helper.SetActive(true);

        Vector3 tmp;
        float count = 0;
        while(count <= 1)
        {
            count += 0.1f;
            tmp = Vector3.Lerp(startPos, targetPos, count);
            cam.transform.position = tmp;
            yield return wait; 
        }

        helper.SetActive(false);
    }
}
