using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VegiShopSlot : MonoBehaviour
{
    PlayerInfo playerInfo;

    int Lv;
    Item.ItemInfo item;

    string LvPrice;

    Image IMG;

    TextMeshProUGUI LvTxt, Name, Price, GrowTime;
    TextMeshProUGUI BuySeed, LvUp;


    public void Init(int _id)
    {
        item = ReadVegiInfo(_id);

        Assign();

        SetDisplay();
    }
    void Assign()
    {
        playerInfo = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();

        IMG = this.transform.Find("Image").GetComponent<Image>();
        LvTxt = this.transform.Find("Vegi Lv").GetComponent<TextMeshProUGUI>();
        Name = this.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        Price = this.transform.Find("Price").GetComponent<TextMeshProUGUI>();
        GrowTime = this.transform.Find("Grow time").GetComponent<TextMeshProUGUI>();

        BuySeed = this.transform.Find("Buy Seed").transform.Find("Text").GetComponent<TextMeshProUGUI>();
        LvUp = this.transform.Find("Lv up").transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }
    void SetDisplay()
    {
        IMG.sprite = Resources.Load<Sprite>("Icon/vegi" + string.Format("{0:D3}", item.ID));
        LvTxt.text = string.Format("Lv {0}", this.Lv);
        Name.text = this.item.Name; Name.fontStyle = FontStyles.Bold;
        Price.text = string.Format("가격: {0}", NumberConverter.ToDisplayNum(this.item.SellPrice));
        GrowTime.text = string.Format("성장시간: {0}초", this.item.GrowTime);
        BuySeed.text = string.Format("씨앗 구매\n({0})", this.item.SeedPrice);
        LvUp.text = string.Format("레벨업\n({0}C)", NumberConverter.ToDisplayNum(LvPrice));
    }

    public void Buy_Seed()
    {
        if (!playerInfo.canUseCarrot(this.item.SeedPrice))
        {
            // 돈이 부족해요!
            return;
        }
        playerInfo.useCarrot(this.item.SeedPrice);
        playerInfo.getItem(this.item.ID, 1);
    }

    public void Lv_Up()
    {
        // 최대 레벨 체크
        if(this.Lv == 10)
        {
            // 맥스 레벨이에요! 알림
            return;
        }

        if (!playerInfo.canUseCarrot(LvPrice))
        {
            // 돈이 부족해요!
            return;
        }
        playerInfo.useCarrot(LvPrice);

        this.Lv++;
        this.item.GrowTime--;
        //this.item.SellPrice = NumberConverter.plusBtwUnit(this.item.SellPrice, this.item.SellPrice);
        //this.item.SellPrice = NumberConverter.multipleBtwUnit(this.item.SellPrice, 30f);
        //this.LvPrice = NumberConverter.multipleBtwUnit(this.LvPrice, 2f);
        this.LvPrice = CalcLvUp(this.item.ID, this.Lv);
        this.item.SellPrice = CalcSellPrice(this.item.ID, this.Lv);

        playerInfo.updateItemInfo(this.item);
        LvTxt.text = string.Format("Lv {0}", this.Lv);
        Price.text = string.Format("가격: {0}", this.item.SellPrice);
        GrowTime.text = string.Format("성장시간: {0}초", this.item.GrowTime);
        LvUp.text = string.Format("레벨업\n({0})", LvPrice);
    }

    #region About Lv Design
    string CalcLvUp(int _id, int _lv)
    {
        double lvup = Mathf.Pow(1.2f, _lv * _id) * Mathf.Pow(17.08f - _id, _id) * Mathf.Pow(1.16f, _id - 1);

        return NumberConverter.numTounit(lvup, "");
    }
    string CalcSellPrice(int _id, int _lv)
    {
        double sellprice = Mathf.Pow(1.18f, _lv * _id) * Mathf.Pow(17 - _id, _id) * Mathf.Pow(1.16f, _id - 1);

        return NumberConverter.numTounit(sellprice, "");
    }
    #endregion


    #region Read
    Item.ItemInfo ReadVegiInfo(int _id)
    {
        Lv = 1; // read lv
        LvPrice = CalcLvUp(_id, this.Lv);
        
        //Lv = 1;
        //LvPrice = "10";

        Item.ItemInfo tmp = new Item.ItemInfo();
        //tmp.ID = 1;
        tmp.ID = _id;
        tmp.Name = "당근";
        tmp.SeedPrice = "1";
        tmp.GrowTime = 10;
        //tmp.SellPrice = "10";
        tmp.SellPrice = CalcSellPrice(tmp.ID, this.Lv);

        return tmp;
    }
    #endregion
}
