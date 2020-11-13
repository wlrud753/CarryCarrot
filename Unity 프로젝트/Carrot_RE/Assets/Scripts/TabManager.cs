using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TabManager : MonoBehaviour
{
    GameObject ShopWindow;

    int Tab;

    string rectName, slotName;

    private void Start()
    {
        ShopWindow = this.transform.Find("Shop Window").gameObject;

        Tab = 4;

        SetButton();
        SetSlotRect();
    }

    #region Setting
    // 전체적으로 파일에서 정보 읽어오기 구현해야 함
    // TAB IDX 맞춰서 적절한 정보 넣도록 구현
    void SetButton()
    {
        GameObject Buttons = this.transform.Find("Shop Window").Find("Tab Button").Find("Buttons").gameObject;
        GridLayout grid = Buttons.GetComponent<GridLayout>();

        for(int i = 0; i < Buttons.transform.childCount; i++)
        {
            Buttons.transform.GetChild(i).name = "순회 " + i;
        }
    }
    void SetSlotRect()
    {
        GameObject ScrollRect;

        for(int i = 0; i < Tab; i++)
        {
            ScrollRect = Instantiate(Resources.Load("Prefabs/Scroll Rect")) as GameObject;
            ScrollRect.name = "탭 " + i;
            ScrollRect.transform.SetParent(ShopWindow.transform);
            ScrollRect.transform.localScale = Vector3.one;
            SetSlot(ScrollRect.transform.Find("Slots").gameObject, i);

            if(i != 0)
            {
                Vector3 Pos = ScrollRect.transform.position;
                Pos.x = -1000f;
                ScrollRect.transform.localPosition = Pos;
            }
        }
    }
    void SetSlot(GameObject SlotPanel, int _tabIDX)
    {
        GameObject Slot;

        int slotCount = 3; // _tabIDX 기반으로 슬롯 개수 READ

        for(int i = 0; i < slotCount; i++)
        {
            Slot = Instantiate(Resources.Load("Prefabs/Slot" /* +i */)) as GameObject;
            Slot.name = "슬롯" + _tabIDX + i;
            Slot.transform.Find("Slot").Find("Text").GetComponent<TextMeshProUGUI>().text = "슬롯" + _tabIDX + i;
            Slot.transform.SetParent(SlotPanel.transform);
            Slot.transform.localScale = Vector3.one;
        }
    }
    #endregion

    public void TabChange(int _tabIDX)
    {
        GameObject tmpGO;
        Vector3 Pos;

        for(int i = 0; i < ShopWindow.transform.childCount; i++)
        {
            tmpGO = ShopWindow.transform.GetChild(i).gameObject;
            if (tmpGO.name.Contains("탭"))
            {
                if (tmpGO.name.Contains(_tabIDX.ToString()))
                {
                    Pos = tmpGO.transform.position;
                    Pos.x = 0f;
                    tmpGO.transform.position = Pos;
                }
                else
                {
                    Pos = tmpGO.transform.position;
                    Pos.x = -1000f;
                    tmpGO.transform.position = Pos;
                }
            }
        }
    }
}
