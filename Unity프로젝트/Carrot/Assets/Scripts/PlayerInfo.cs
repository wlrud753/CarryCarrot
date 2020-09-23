using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    class ItemSlotInfo
    {
        Item.ItemInfo itemInfo;
        int num;

        public Item.ItemInfo GetItem() { return itemInfo; }
        public int GetNum() { return num; }

        public void SetItem(Item.ItemInfo _itemInfo) { this.itemInfo = _itemInfo; }
        public void SetNum(int _num) { this.num = _num; }

        public void use() { num--; }
        public void get(int _num) { num += _num; }
    }
    List<ItemSlotInfo> itemSlotInfos;
    GameObject inventoryGO;

    // Money
    public string Carrot;

    public float GrowHelp;

    // Start is called before the first frame update
    void Start()
    {
        inventoryGO = GameObject.Find("Farm UI").transform.Find("Inventory").transform.
            Find("Scroll Rect").transform.Find("Items").gameObject;

        ReadInfo();
        ReadInventory();
        setSlots();
    }

    #region Slots
    void setSlots ()
    {
        GameObject tmp;
        for (int i = 0; i < itemSlotInfos.Count; i++)
        {
            if (itemSlotInfos[i].GetNum() < 1)
                continue;

            tmp = Instantiate(Resources.Load("Prefabs/ItemSlot")) as GameObject;
            tmp.transform.SetParent(inventoryGO.transform);
            //tmp.GetComponent<ItemSlot>().item = itemSlotInfos[i].itemInfo;
            tmp.GetComponent<ItemSlot>().setSlot(itemSlotInfos[i].GetItem(), itemSlotInfos[i].GetNum());
        }
    }
    void removeSlots()
    {
        GameObject tmp;
        for(int i = 0; i < inventoryGO.transform.childCount; i++)
        {
            tmp = inventoryGO.transform.GetChild(i).gameObject;
            Destroy(tmp);
        }
    }
    #endregion

    #region Item
    public void getItem(Item.ItemInfo _item, int _amount)
    {
        bool haveIt = false;

        for (int i = 0; i < itemSlotInfos.Count; i++)
        {
            if (itemSlotInfos[i].GetItem().ID == _item.ID)
            {
                itemSlotInfos[i].get(_amount);
                haveIt = true;
                break;
            }
        }
        if (!haveIt)
        {
            ItemSlotInfo tmp = new ItemSlotInfo();

            tmp.SetItem(_item);
            tmp.SetNum(_amount);

            itemSlotInfos.Add(tmp);

            SortItemList();
        }
        removeSlots();
        setSlots();
    }
    public bool useItem(int _id)
    {
        for (int i = 0; i < itemSlotInfos.Count; i++)
        {
            if (itemSlotInfos[i].GetItem().ID == _id)
            {
                if (itemSlotInfos[i].GetNum() != 0)
                {
                    itemSlotInfos[i].use();
                    inventoryGO.transform.Find("vegi" + string.Format("{0:D3}", itemSlotInfos[i].GetItem().ID)).
                        GetComponent<ItemSlot>().setText(itemSlotInfos[i].GetNum());
                    return true;
                }
                else
                    return false;
            }
        }
        return false;
    }

    public void updateItemInfo(Item.ItemInfo _newitemInfo)
    {
        for (int i = 0; i < itemSlotInfos.Count; i++)
        {
            if (itemSlotInfos[i].GetItem().ID == _newitemInfo.ID)
            {
                itemSlotInfos[i].SetItem(_newitemInfo);
                removeSlots();
                setSlots();
                break;
            }
        }
    }

    void SortItemList()
    {
        itemSlotInfos.Sort(delegate (ItemSlotInfo A, ItemSlotInfo B)
        {
            if (A.GetItem().ID > B.GetItem().ID) return 1;
            else if (A.GetItem().ID < B.GetItem().ID) return -1;
            return 0;
        });
    }
    #endregion

    #region Carrot
    public string getCarrot() { return this.Carrot; }
    public void plusCarrot(string _carrot) { this.Carrot = NumberConverter.plusBtwUnit(this.Carrot, _carrot); }
    // can Use... 꼭 필요한가 싶네요 계산은 1번만 해도 되는데
    public bool canUseCarrot(string _amount) { if (NumberConverter.minusBtwUnit(this.Carrot, _amount).Equals("")) return false; return true; }
    public void useCarrot(string _amount) { this.Carrot = NumberConverter.minusBtwUnit(this.Carrot, _amount); }
    #endregion

    #region GrowHelp
    public void GrowHelpLvUp(int _increment)
    {
        GrowHelp += _increment;
    }
    #endregion

    #region Read
    void ReadInfo()
    {
        this.Carrot = "10";
    }

    void ReadInventory()
    {
        itemSlotInfos = new List<ItemSlotInfo>();

        ItemSlotInfo tmpSlotInfo;
        Item.ItemInfo tmpItem;

        // tmp..
        //for (int i = 0; i < 12; i++)
        //{
        //    tmpSlotInfo = new ItemSlotInfo();

        //    tmpItem = new Item.ItemInfo();
        //    tmpItem.ID = i + 1;
        //    tmpItem.Name = "ha" + i;
        //    tmpItem.GrowTime = (i + 10) * ((i+1) * (i+1));
        //    tmpItem.SellPrice = (i + 10).ToString();

        //    tmpSlotInfo.SetItem(tmpItem);
        //    tmpSlotInfo.SetNum(Random.Range(0, 15));
        //    if (tmpSlotInfo.GetNum() != 0)
        //        itemSlotInfos.Add(tmpSlotInfo);
        //}

        //tmpSlotInfo = new ItemSlotInfo();
        //tmpItem = new Item.ItemInfo();
        //tmpItem.ID = 1;
    }
    #endregion

    #region Write
    void WriteInfo()
    {

    }

    void WriteInventory()
    {

    }
    #endregion

    #region Getter
    public List<Item.ItemInfo> getHavingItemInfo()
    {
        List<Item.ItemInfo> items = new List<Item.ItemInfo>();

        for(int i = 0; i < itemSlotInfos.Count; i++)
        {
            if(itemSlotInfos[i].GetNum() > 0)
            {
                items.Add(itemSlotInfos[i].GetItem());
            }
        }

        return items;
    }
    #endregion
}
