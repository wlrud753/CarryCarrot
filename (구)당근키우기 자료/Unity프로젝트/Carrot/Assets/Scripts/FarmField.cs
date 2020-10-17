using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmField : MonoBehaviour
{
    PlayerInfo playerInfo;

    Item.ItemInfo item;

    // check
    public bool isPlanted;
    bool overHalfSwitch;
    public bool isAllGrown;

    public float timer;
    SpriteRenderer CropIMG;

    // slider
    public Slider timeSlider;

    private void Start()
    {
        playerInfo = GameObject.FindObjectOfType<PlayerInfo>();

        isPlanted = false;
        overHalfSwitch = false;
        isAllGrown = false;

        CropIMG = this.transform.Find("Crop").GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isPlanted)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                // text 바꿔줘야 함
                timeSlider.value = (timer > 0) ? timer : 0;
                if (timer < this.item.GrowTime/2 && !overHalfSwitch)
                {
                    // CropIMG 변경
                    CropIMG.sprite = Resources.Load<Sprite>("GrIcon/hlgr_vegi" + string.Format("{0:D3}", this.item.ID));
                    overHalfSwitch = true;
                }
            }
            else
            {
                if (!isAllGrown)
                {
                    // End of Growing
                    isAllGrown = true;

                    timeSlider.enabled = false;

                    // CropIMG 변경
                    CropIMG.sprite = Resources.Load<Sprite>("GrIcon/allgr_vegi" + string.Format("{0:D3}", this.item.ID));
                }
            }
        }
    }

    public void startGrow(Item.ItemInfo _item)
    {
        if (this.isPlanted)
            return;

        this.item = _item;
        timer = this.item.GrowTime;

        // PlayerInfo에서, 해당 아이템 찾아서 개수 줄어들게 하기.
        if (!playerInfo.useItem(_item.ID)) // 개수 줄어들 수 있으면, 줄어들고 return True. 아니면 그냥 return False
            return;

        isPlanted = true;
        overHalfSwitch = false; // Harvest 없이 Obj 사라지는, 추가적인 이벤트 감안.
        isAllGrown = false;

        // 작물 GO 활성화
        CropIMG.gameObject.SetActive(true);

        // CropIMG 설정
        string imgName = "stgr_root";
        switch (this.item.ID)
        {
            case 1:
            case 2:
                imgName = "stgr_root"; break;
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
                imgName = "stgr_plant"; break;
            case 9:
            case 10:
            case 11:
            case 12:
                imgName = "stgr_tree"; break;
        }
        CropIMG.sprite = Resources.Load<Sprite>("GrIcon/" + imgName);
        // 시간 text(미완), 게이지 설정(완)

        timeSlider.maxValue = this.item.GrowTime;
        timeSlider.enabled = true;
    }

    private void OnMouseUp()
    {
        if (!isAllGrown)
        {
            Click();
        }
        else
        {
            Harvest();
        }
    }
    public void Click()
    {
        timer -= (1.0f + playerInfo.GrowHelp);
        timeSlider.value = (timer > 0) ? timer : 0;
    }
    public void Harvest()
    {
        playerInfo.plusCarrot(this.item.SellPrice);

        // Icon 찾아서, 해당 Icon 띠링 위로 올라가는 효과

        // 초기화
        isPlanted = false;
        overHalfSwitch = false;
        isAllGrown = false;

        // 작물 GO 비활성화
        CropIMG.sprite = null;
        CropIMG.gameObject.SetActive(false);
    }
}
