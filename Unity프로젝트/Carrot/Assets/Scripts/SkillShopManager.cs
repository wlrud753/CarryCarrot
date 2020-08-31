using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShopManager : MonoBehaviour
{
    GameObject Slots;

    // Start is called before the first frame update
    void Start()
    {
        GameObject tmpGO;

        Slots = this.transform.Find("Scroll Rect").transform.Find("Slots").gameObject;

        for (int i = 1; i <= 7; i++)
        {
            tmpGO = Instantiate(Resources.Load("Prefabs/Skill Shop Slot")) as GameObject;
            tmpGO.name = "Skill Shop Slot " + i;
            tmpGO.transform.SetParent(Slots.transform);
            tmpGO.GetComponent<SkillShopSlot>().Init(i);
        }
    }
}
