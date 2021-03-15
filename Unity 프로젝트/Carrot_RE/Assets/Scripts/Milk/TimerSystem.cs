using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerSystem : MonoBehaviour
{
    public Slider TimerBar;

    public float TimerSpeed;
    float timerSpeed;
    const float BaseSpeed_Combo = 1.1f;

    bool TimerMove;
    int TimerDir;

    float HitCenter;

    bool optimizer_FixedDir;

    GradeSystem gradeSystem;

    WaitForSeconds wait;

    // Start is called before the first frame update
    void Start()
    {
        TimerBar = GameObject.Find("Canvas").transform.Find("Timer Bar").GetComponent<Slider>();

        timerSpeed = TimerSpeed;

        gradeSystem = GameObject.Find("System").transform.Find("Grade System").GetComponent<GradeSystem>();

        wait = new WaitForSeconds(0.001f);
    }

    #region Set
    public void setTimerArea()
    {
        // Slider 값 설정
        TimerBar.maxValue = 1000;
        switch(Random.Range(0, 2))
        {
            case 0:
                TimerBar.value = TimerBar.minValue;
                TimerDir = 1;
                break;
            case 1:
                TimerBar.value = TimerBar.maxValue;
                TimerDir = -1;
                break;
        }

        // Hit Area 설정
        HitCenter = Random.Range(150, 851);
    }
    public void setTimerMove(bool _timerMove)
    {
        TimerMove = _timerMove;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (TimerMove)
        {
            TimerBar.value += Time.deltaTime * timerSpeed * TimerDir;

            if(TimerBar.value >= TimerBar.maxValue || TimerBar.value <= 0)
            {
                TimerDir *= -1;

                // for Pattern
                if (FixedDir && optimizer_FixedDir)
                {
                    StartCoroutine(FixDir());
                }
            }
        }
    }

    #region Timer Move
    IEnumerator MoveTimer()
    {
        TimerBar.value += Time.deltaTime * timerSpeed * TimerDir;
        if (TimerBar.value >= TimerBar.maxValue || TimerBar.value <= 0)
            TimerDir *= -1;
        yield return wait;
    }
    #endregion

    #region Timer Speed
    public void ChangeTimerSpeed(float _Coef)
    {
        timerSpeed *= _Coef;
    }
    public void ResetTimerSpeed(int _combo)
    {
        timerSpeed = TimerSpeed * Mathf.Pow(BaseSpeed_Combo, _combo);
    }
    #endregion

    #region Timer Hit
    public string TimerHit()
    {
        TimerMove = false;

        return FindHitArea(TimerBar.value);
    }
    string FindHitArea(float _value)
    {
        // Result := Hit 판정의 정중앙으로부터 얼마나 떨어져있는가
        // -25 ~ +25 : Perfect
        // -75 ~ +75 : Great
        // -150 ~ +150: Good
        // 그 외: Bad
        float Result = Mathf.Abs(HitCenter - _value);

        if(Result < 25)
            return "PERFECT";
        else if(Result < 75)
            return "GREAT";
        else if(Result < 150)
            return "GOOD";
        else
            return "BAD";
    }
    #endregion

    #region For Pattern
    // Fixed Direction
    bool FixedDir;
    public void setFixedDir(bool _active)
    {
        FixedDir = _active;
        optimizer_FixedDir = _active;
    }
    IEnumerator FixDir()
    {
        // 단순히 '양 끝에 도달했을 때, 반대편 끝으로 이동'을 수행
        // 이때, 양 끝 도달 시 확정적으로 TimerDir *= -1이 수행, 역방향이 돼버림 (in Update)
        // Ex. 1000 도달 > 0으로 이동 && 방향은 +1 에서 -1이 되어, 0에서 -1 방향으로 이동하게 됨
        // 정방향 맞추고자 TimerDir *= -1을 재수행
        // FixedDir은 특수 Case이므로 본 함수 내에서 관련 정보를 모두 제어하는 것이 맞다고 생각
        // Update의 일반적인 Code는 그대로 수행하고, 본 함수에서 특이 Case 관련 조정 수행

        // StopTimer 패턴과 겹치는 경우 :: 1000 도달 > 0으로 이동 >>> 1000에서 Stop, 0에서 Stop >>> 1000이면 0으로 이동, 0이면 1000으로 이동
        // 해당 현상이 반복, 실제 화면상에서도 굉장히 이상한 형태로 나타남
        // 이를 방지코자 '양 끝에서 이동은 1번만' 일어나도록 optimizer와 WaitUtil로 호출 빈도 및 함수 종료 제어

        optimizer_FixedDir = false;

        if (TimerBar.value >= TimerBar.maxValue)
            TimerBar.value = TimerBar.minValue;
        else if (TimerBar.value <= TimerBar.minValue)
            TimerBar.value = TimerBar.maxValue;

        TimerDir *= -1;

        yield return new WaitUntil(() => TimerBar.value != TimerBar.minValue && TimerBar.value != TimerBar.maxValue);
        optimizer_FixedDir = true;
    }

    // Move HitCenter
    public int HitCenterDir;
    public float HitCenterSpeed;
    public float MoveHitCenter()
    {
        HitCenter += Time.deltaTime * HitCenterSpeed * HitCenterDir;

        if (HitCenter <= 150f || HitCenter >= 850f)
            HitCenterDir *= -1;

        return HitCenter;
    }
    #endregion

    #region Get
    public float getHitCenter() { return HitCenter; }
    public float getTimerValue() { return TimerBar.value; }
    #endregion
}
