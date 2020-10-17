using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopModManager : MonoBehaviour
{
    GameObject VegiShop, SkillShop;
    Vector3 downPos, displayPos;

    GameObject helper;
    WaitForSeconds wait;

    // Start is called before the first frame update
    void Start()
    {
        VegiShop = GameObject.Find("Shop").transform.Find("Vegi Shop").gameObject;
        SkillShop = GameObject.Find("Shop").transform.Find("Skill Shop").gameObject;

        downPos = new Vector3(800, -1280, 0);
        displayPos = new Vector3(800, -60, 0);

        helper = GameObject.Find("Display Manager").transform.Find("Helper").gameObject;
        wait = new WaitForSeconds(0.001f);
    }

    public void ChangeMod()
    {
        StartCoroutine(SwitchPos());
    }
    IEnumerator SwitchPos()
    {
        helper.SetActive(true);

        GameObject onDown, onDisplay;
        if (VegiShop.transform.position.Equals(downPos))
        {
            onDown = VegiShop; onDisplay = SkillShop;
        }
        else
        {
            onDown = SkillShop; onDisplay = VegiShop;
        }

        const float plusAmount = 0.08f;
        Vector3 tmp;
        float count = 0;
        while (count <= 1)
        {
            count += plusAmount;

            tmp = Vector3.Lerp(displayPos, downPos, count);
            onDisplay.transform.position = tmp;

            yield return wait;
        }

        count = 0;
        while (count <= 1)
        {
            count += plusAmount;

            tmp = Vector3.Lerp(downPos, displayPos, count);
            onDown.transform.position = tmp;

            yield return wait;
        }

        helper.SetActive(false);
    }
}
