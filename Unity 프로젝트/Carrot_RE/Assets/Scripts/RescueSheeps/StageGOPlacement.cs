using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGOPlacement : MonoBehaviour
{
    const string SHEEP_PATH = "Prefabs/Rescue Sheeps/Sheep", WOLF_PATH = "Prefabs/Rescue Sheeps/Wolf";

    Transform Slots;

    Bounds SlotArea;

    // Start is called before the first frame update
    void Start()
    {
        Slots = GameObject.Find("Slots").transform;

        SlotArea = GameObject.Find("Slots").transform.Find("Slot Area").GetComponent<BoxCollider2D>().bounds;
    }

    public void MakeSlotGO(string[] _slots, int _slotCount)
    {
        // Slot GO 관련 변수
        int _size = (int)Mathf.Sqrt(_slotCount);
        GameObject[] slots = new GameObject[_slotCount];
        GameObject slot;
        GameObject parentGO = GameObject.Find("Slots");

        // Position 관련 변수
        Vector3 divisionOfArea = (SlotArea.max - SlotArea.min) / _size;
        float revisionX = divisionOfArea.x / 2, revisionY = divisionOfArea.y / 2;
        Vector3 slotPos = new Vector3();

        // Slot GO 생성
        for (int i = 0; i < slots.Length; i++)
        {
            // Generate Slot GO
            slot = Instantiate(Resources.Load("Prefabs/Rescue Sheeps/" + _slots[i])) as GameObject;
            slot.name = _slots[i] + i;
            slot.transform.SetParent(parentGO.transform);

            // Position of GO
            slotPos.x = (SlotArea.min.x + revisionX) + divisionOfArea.x * (i % _size);
            slotPos.y = (SlotArea.min.y + revisionY) + divisionOfArea.y * (i / _size);
            slotPos.z = 0f;
            slot.transform.position = slotPos;
        }
    }

    #region Pattern of Placement
    void PlaceSquare()
    {

    }
    void PlaceDiamond()
    {

    }
    void PlaceTriangle()
    {

    }
    void PlaceHexagon()
    {

    }
    #endregion

    public void DestroySlotGO()
    {
        // Destroy All
        for (int i = 0; i < Slots.childCount; i++)
        {
            if (Slots.GetChild(i).name != "Slot Area")
                Destroy(Slots.GetChild(i).gameObject);
        }
    }
}
