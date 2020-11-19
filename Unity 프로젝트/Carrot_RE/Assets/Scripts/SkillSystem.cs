using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    ClickSystem clickSystem;
    List<FarmManager> farms;

    bool ACstart;
    public float AutoClickDurationTime;
    float acTimer;
    public int ClickAmount;

    bool REstart;
    public float RiseEarningDurationTime;
    float reTimer;
    public float EarnMult;

    bool DTstart;
    public float DeclinePlantTimeDurationTime;
    float dtTimer;
    public float TimeMult;

    WaitForSeconds wait;

    void Start()
    {
        clickSystem = GameObject.Find("System").transform.Find("Click System").GetComponent<ClickSystem>();
        farms = new List<FarmManager>();
        getFarms();

        ACstart = false;
        acTimer = 0;
        wait = new WaitForSeconds(0.0001f);
    }
    void getFarms()
    {
        GameObject FarmList = GameObject.Find("Farm List");
        FarmManager tmpFM;
        farms.Clear();

        for(int i = 0; i < FarmList.transform.childCount; i++)
        {
            tmpFM = FarmList.transform.GetChild(i).GetComponent<FarmManager>();
            farms.Add(tmpFM);
        }
    }

    void Update()
    {
        if (ACstart && acTimer == 0)
        {
            StartCoroutine(AutoClick());
        }
        if(REstart)
        {
            if(reTimer == 0)
                RiseEarning();

            if (reTimer < RiseEarningDurationTime)
                reTimer += Time.deltaTime;
            else
            {
                EORE();
                REstart = false;
                reTimer = 0;
            }
        }
        if(DTstart)
        {
            if(dtTimer == 0)
                DeclinePlantTime();

            if (dtTimer < DeclinePlantTimeDurationTime)
                dtTimer += Time.deltaTime;
            else
            {
                EODT();
                DTstart = false;
                dtTimer = 0;
            }
        }
    }
    #region Skill Performing Func
    IEnumerator AutoClick()
    {
        // 초당 ClickAmount번 클릭 > (1/ClickAmount)초 체크하기 위한 변수
        float secTimer = 0;

        // secTimer의 경우, 버려지는 시간이 생김. 매 (1/ClickAmount)보다 커지는 경우, 커진 만큼이 버려짐.
        // 총 지속시간이 길수록, 버려지는 시간들에 따라 Click 수행 횟수가 적어짐. (Ex. 10초 동작 시 - 5번 가량의 (1/ClickAmount)초가 버려져서 5번의 클릭이 미수행)
        // 버려지는 시간을 보완하기 위한 변수
        float supplement = 0;

        while(acTimer < AutoClickDurationTime + 0.1f) // + 0.1f: supplement와 함께, 버려지는 시간에 대한 보완. 알고리즘 문제로 클릭 횟수가 줄어드는 것보단 늘어나는게 더 바람직.
        {
            if(secTimer > 1f / ClickAmount)
            {
                clickSystem.Click();
                supplement += secTimer - 1f / ClickAmount;
                if (supplement > 1f / ClickAmount)
                {
                    clickSystem.Click();
                    supplement = 0;
                }
                secTimer = 0;
            }
            secTimer += Time.deltaTime;
            acTimer += Time.deltaTime;
            yield return wait;
        }
        ACstart = false;
    }

    /* 스킬 적용 중에 농장 추가/레벨업 하는 경우의 Coverage
     * 농장 추가: 기본적으로 모든 농장은 farms에 할당돼있음. LV 0이기 때문에 아무 효과가 없는 것. 따라서 농장 추가, 곧 LV 0 > 1이 된다고 해도, Mult값은 타 농장들과 동일하게 적용되고 있음.
     * (농장 추가는 예외 케이스가 아님)
     * 레벨업하는 경우: 애초에 Farm의 기본 정보 * Mult를 기준으로 함.
     * Farm의 기본 정보가 바뀌면 -> Mult에 상관없이 변동이 일어남.
     * (이 역시 예외 케이스가 아님)
     */
    void RiseEarning()
    {
        for(int i = 0; i < farms.Count; i++)
        {
            farms[i].MultipleEarn(EarnMult);
        }
    }
    void EORE()
    {
        for(int i = 0; i < farms.Count; i++)
        {
            farms[i].MultipleEarn(1 / EarnMult);
        }
    }
    void DeclinePlantTime()
    {
        for(int i = 0; i < farms.Count; i++)
        {
            farms[i].MultipleTime(TimeMult);
        }
    }
    void EODT()
    {
        for(int i = 0; i < farms.Count; i++)
        {
            farms[i].MultipleTime(1 / TimeMult);
        }
    }
    #endregion

    #region Interact Button
    public void ActiveAutoClick()
    {
        if (!ACstart)
        {
            ACstart = true;
            acTimer = 0;
        }
    }
    public void ActiveRiseEarning()
    {
        if (!REstart)
        {
            REstart = true;
            reTimer = 0;
            getFarms(); // 실제 플레잉에선 필요없는 부분. 활성 여부와 무관하게 모든 농장은 farms에 add됨.
        }
    }
    public void ActiveDeclinePlantTime()
    {
        if (!DTstart)
        {
            DTstart = true;
            dtTimer = 0;
            getFarms();
        }
    }
    #endregion
}
