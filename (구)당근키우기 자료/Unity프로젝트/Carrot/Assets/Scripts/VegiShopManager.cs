using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegiShopManager : MonoBehaviour
{
    GameObject Slots;

    // Start is called before the first frame update
    void Start()
    {
        GameObject tmpGO;

        Slots = this.transform.Find("Scroll Rect").transform.Find("Slots").gameObject;

        for(int i = 1; i <= 12; i++)
        {
            tmpGO = Instantiate(Resources.Load("Prefabs/Vegi Shop Slot")) as GameObject;
            tmpGO.name = "Vegi Shop Slot " + i;
            tmpGO.transform.SetParent(Slots.transform);
            tmpGO.GetComponent<VegiShopSlot>().Init(i);
        }
    }
    

    // Read를 여기서 진행할까??
    // Vegi List를 여기서 읽어서, id를 각 slot마다 뿌려주고, 상세 read는 각 slot이 진행하는?
}
