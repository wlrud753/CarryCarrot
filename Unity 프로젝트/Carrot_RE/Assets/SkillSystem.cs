using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    ClickSystem clickSystem;
    List<FarmManager> farms;

    bool ACstart;
    public float AutoClickTimer;
    float acTimer;
    public int ClickAmount;

    WaitForSeconds wait;

    void Start()
    {
        clickSystem = GameObject.Find("System").transform.Find("Click System").GetComponent<ClickSystem>();
        getFarms();

        ACstart = false;
        acTimer = 0;
        wait = new WaitForSeconds(0.0001f);
    }
    void getFarms()
    {
        GameObject FarmList = GameObject.Find("Farm List");
        FarmManager tmpFM;
        farms = new List<FarmManager>();

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
        
    }
    IEnumerator AutoClick()
    {
        // 초당 ClickAmount번 클릭 > (1/ClickAmount)초 체크하기 위한 변수
        float secTimer = 0;

        // secTimer의 경우, 버려지는 시간이 생김. 매 (1/ClickAmount)보다 커지는 경우, 커진 만큼이 버려짐.
        // 총 지속시간이 길수록, 버려지는 시간들에 따라 Click 수행 횟수가 적어짐. (Ex. 10초 동작 시 - 5번 가량의 (1/ClickAmount)초가 버려져서 5번의 클릭이 미수행)
        // 버려지는 시간을 보완하기 위한 변수
        float supplement = 0;

        while(acTimer < AutoClickTimer + 0.1f) // + 0.1f: supplement와 함께, 버려지는 시간에 대한 보완. 알고리즘 문제로 클릭 횟수가 줄어드는 것보단 늘어나는게 더 바람직.
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

    public void ActiveAutoClick()
    {
        if (!ACstart)
        {
            ACstart = true;
            acTimer = 0;
        }
    }
}
