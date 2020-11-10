using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 상속용
public class ShopSlot : MonoBehaviour
{
    PlayerInfo playerInfo;

    Button BuyButton;
    TextMeshProUGUI buttonText;

    int Price;

    void Start()
    {
        playerInfo = GameObject.Find("Player Info").GetComponent<PlayerInfo>();

        //BuyButton = this.transform.Find("Buy Button").GetComponent<Button>();
        //buttonText = BuyButton.gameObject.transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }
    public void SetPrice(int _price)
    {
        this.Price = _price;
        // Button Text 설정
    }

    private void Update()
    {
        // 구매 버튼 활성화/비활성화
        //if (BuyCheck(Price) && BuyButton.interactable == false)
        //{
        //    BuyButton.interactable = true;
        //}
        //else if(!BuyCheck(Price) && BuyButton.interactable == true)
        //{
        //    BuyButton.interactable = false;
        //}
    }

    // 구매 체크
    bool BuyCheck(int _need)
    {
        if (playerInfo.CanUseCarrot(_need))
            return true;

        return false;
    }

    // 구매에 따른 Carrot 사용
    void Buy(int _need)
    {
        playerInfo.useCarrot(_need);
    }

    // 각 Slot script에서 제어
    /*
     * 구매에 따른 변동 객체 콜
     * 객체 변동사항 전달
     * 텍스트 변경
     */
}
