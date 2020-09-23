using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillShopSlot : MonoBehaviour
{
    PlayerInfo playerInfo;

    SkillPerformer skillPerformer;

    Image IMG;
    TextMeshProUGUI Name, LvText, Description, LvUpText;

    int id, Lv;
    string Skname, description, LvPrice;

    public void Init(int _id)
    {
        Assign();
        ReadInfo(_id);
        setVal();
    }
    void Assign()
    {
        playerInfo = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();
        skillPerformer = GameObject.Find("Skill Performer").GetComponent<SkillPerformer>();

        IMG = this.transform.Find("IMG").GetComponent<Image>();
        Name = this.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        LvText = this.transform.Find("Lv Text").GetComponent<TextMeshProUGUI>();
        Description = this.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        LvUpText = this.transform.Find("Lv Up").transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }
    void setVal()
    {
        //IMG.sprite = Resources.Load<Sprite>("SkIcon/sk" + this.id);
        Name.text = this.Skname; Name.fontStyle = FontStyles.Bold;
        LvText.text = string.Format("Lv {0}", this.Lv);
        Description.text = string.Format("효과\n{0}", description);
        LvUpText.text = string.Format("레벨업\n({0})", LvPrice);
    }

    public void LvUp()
    {
        if (!playerInfo.canUseCarrot(LvPrice))
        {
            // 돈이 부족해요!
            return;
        }
        playerInfo.useCarrot(this.LvPrice);

        this.Lv++;
        // 효과 업글 필요... >> skill performer에 렙업 정보 전달.

        // temp
        description += "\n렙1업";
        LvPrice = NumberConverter.multipleBtwUnit(LvPrice, 1.5f);

        LvText.text = string.Format("Lv {0}", this.Lv);
        Description.text = string.Format("효과\n{0}", description);
        LvUpText.text = string.Format("레벨업\n({0})", LvPrice);
    }

    void ReadInfo(int _id)
    {
        this.id = _id;

        Lv = 1;
        Skname = "스킬 이름";
        description = "스킬 효과 작성해주세요.";
        LvPrice = "100";
    }
}
