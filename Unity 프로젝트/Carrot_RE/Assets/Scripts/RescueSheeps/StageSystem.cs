using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSystem : MonoBehaviour
{
    public int SlotCount;

    StageGOPlacement stageGOPlacement;

    // Start is called before the first frame update
    void Start()
    {
        SlotCount = 9;
        stageGOPlacement = this.GetComponent<StageGOPlacement>();
    }

    public void ResetStage()
    {
        stageGOPlacement.DestroySlotGO();
        string[] slots = SetStage();
        stageGOPlacement.MakeSlotGO(slots, SlotCount);
    }

    string[] SetStage()
    {
        int wolfCount = (int)Random.Range(0.25f * SlotCount, 0.35f * SlotCount);
        string[] slots = new string[SlotCount];
        for (int i = 0; i < slots.Length; i++)
            slots[i] = "Sheep";

        int randNum;
        while (wolfCount > 0)
        {
            randNum = Random.Range(0, SlotCount);
            if (slots[randNum] == "Sheep")
            {
                slots[randNum] = "Wolf";
                wolfCount--;
            }
        }

        return slots;
    }
}
