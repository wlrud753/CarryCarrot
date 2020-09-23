using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPerformer : MonoBehaviour
{
    PlayerInfo playerInfo;

    List<FarmField> farmFields;
    GameObject Farm;

    List<Item.ItemInfo> items;

    float PlantTimer, HarvestTimer, ClickTimer; // 기준 시간
    float pTimer,     hTimer,       cTimer; // 시간 체크용
    bool canPlant, canHarvest, canClick;

    int clickFarmAmount;
    List<FarmField> canClickFarms;

    void Start()
    {
        playerInfo = GameObject.Find("PlayerInfo").GetComponent<PlayerInfo>();

        farmFields = new List<FarmField>();
        Farm = GameObject.Find("Farm");
        //getFarmFields();

        farmFields.Add(GameObject.Find("FarmField").GetComponent<FarmField>()); // tmp

        canClickFarms = new List<FarmField>();

        // Read Skill Info
        PlantTimer = 20; HarvestTimer = 20; ClickTimer = 1f; clickFarmAmount = 1; //tmp

    }
    // 화면 상 보이는 밭 obj들을 리스트에 모두 저장
    void getFarmFields()
    {
        FarmField tmp;
        for(int i = 0; i < Farm.transform.childCount; i++)
        {
            if (Farm.transform.GetChild(i).name.Contains("FarmField"))
            {
                tmp = Farm.transform.GetChild(i).GetComponent<FarmField>();
                farmFields.Add(tmp);
            }
        }
    }

    void Update()
    {
        // 각 스킬들 Timer 체크 > 쿨타임 충족하면 스킬별 bool값 true 할당
        if (canPlant == false)
        {
            pTimer += Time.deltaTime;
            if(pTimer > PlantTimer)
            {
                pTimer = 0;
                canPlant = true;
            }
        }
        if (canHarvest == false)
        {
            hTimer += Time.deltaTime;
            if(hTimer > HarvestTimer)
            {
                hTimer = 0;
                canHarvest = true;
            }
        }
        if (canClick == false)
        {
            cTimer += Time.deltaTime;
            if(cTimer > ClickTimer)
            {
                cTimer = 0;
                canClick = true;
            }
        }

        if (canPlant && CanAutoPlant())
        {
            canPlant = false;
            AutoPlant();
        }
        if (canHarvest && CanAutoHarvest())
        {
            canHarvest = false;
            AutoHarvest();
        }
        if (canClick)
        {
            canClick = false;
            AutoClick();
        }
    }

    // Expand 채워야 함. Expand 끝나고 farmFields 재할당해줘야 함
    void ExpandFarm()
    {

    }

    bool CanAutoPlant()
    {
        for (int i = 0; i < farmFields.Count; i++)
        {
            if (farmFields[i].isPlanted == false)
                return true;
        }
        return false;
    }
    bool CanAutoHarvest()
    {
        for (int i = 0; i < farmFields.Count; i++)
        {
            if (farmFields[i].isAllGrown)
                return true;
        }

        return false;
    }

    void AutoPlant()
    {
        int expenIdx = 0;
        items = playerInfo.getHavingItemInfo(); // 유저가 실제 보유하고 있는 작물들의 목록만 get (num > 0인 작물들)

        // 보유작물 중 가장 비싼 작물을 찾아낸다.
        for(int i = 1; i < items.Count; i++)
        {
            if(NumberConverter.minusBtwUnit(items[expenIdx].SellPrice, items[i].SellPrice).Equals(""))
            {
                expenIdx = i;
            }
        }

        // 빈 밭에 가장 비싼 작물을 심는다.
        for(int i = 0; i < farmFields.Count; i++)
        {
            if(farmFields[i].isPlanted == false)
            {
                farmFields[i].startGrow(items[expenIdx]);
                break;
            }
        }
    }
    void AutoHarvest()
    {
        // 수확 가능한 밭을 찾아 수확.
        for (int i = 0; i < farmFields.Count; i++)
        {
            if (farmFields[i].isAllGrown)
            {
                farmFields[i].Harvest();
                break;
            }
        }
    } 
    // 밭 여러 개에서도 적용되는지 확인하면 끝.
    void AutoClick()
    {
        canClickFarms.Clear();

        // 작물이 성장하고 있는 밭들만 찾아내 canClickFarms에 add
        for(int i = 0; i < farmFields.Count; i++)
        {
            if (farmFields[i].isPlanted && !farmFields[i].isAllGrown)
                canClickFarms.Add(farmFields[i]);
        }
        // 작물 성장 중인 밭이 있는 경우, 클릭 가능한 밭 개수만큼 클릭
        if (canClickFarms.Count > 0)
        {
            // 성장 중인 밭 개수보다 스킬이 클릭해주는 밭 개수가 더 작은 경우
            // 성장 중인 밭 개수를 클릭 횟수의 기준으로 설정
            int[] idx = (clickFarmAmount < canClickFarms.Count) ?
                new int[clickFarmAmount] : new int[canClickFarms.Count];

            // 클릭해줄 밭의 idx를 임의로 추출
            for (int i = 0; i < idx.Length; i++)
            {
                idx[i] = Random.Range(0, canClickFarms.Count);
                // idx 중복체크
                for (int j = 0; j < i; j++)
                {
                    if(idx[i] == idx[j])
                    {
                        i--;
                        break;
                    }
                }
            }

            for(int i = 0; i < idx.Length; i++)
            {
                canClickFarms[idx[i]].Click();
            }
        }
    }


    public void updateSkill(int _id, int _lv) // SkillShopSlot에서 호출함
    {
        // 각 스킬별 레벨-효과 환산식 적용
        switch (_id)
        {
            case 0: // 밭 크기 up
                //ExpandFarm(_lv?);
                break;
            case 1: // 클릭시 시간 단축량 up
                //playerInfo.GrowHelpLvUp(_increment amount);
                break;
            case 2: // 자동 클릭 (클릭 쿨타임 감소)
                //ClickTimer -= Amount;
                break;
            case 3: // 자동 클릭 (클릭하는 밭 개수 증가)
                //clickFarmAmount++;
                //if (clickFarmAmount > farmFields.Count)
                //    clickFarmAmount = farmFields.Count;
                break;
            case 4: // 자동 파종 (중간 텀 단축)
                //plantTimer -= Amount;
                break;
            case 5: // 자동 수확 (중간 텀 단축)
                //HarvestTimer -= Amount;
                break;
        }
    }
}
