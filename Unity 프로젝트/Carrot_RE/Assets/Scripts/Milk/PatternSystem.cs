using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSystem : MonoBehaviour
{
    int Combo;
    int PatternIDX;

    TimerSystem timerSystem;
    HitArea hitArea;

    // for Hide
    SpriteRenderer[] hitAreaGUI_SpriteRenderers;
    SpriteRenderer timerGUI_SpriteRenderer;

    Color hitAreaGUI_Color;
    Color timerGUI_Color;

    // for Stop
    int minValue, maxValue;

    #region Control Var of Pattern
    public bool performFixedDir;
    public bool performStopTimer;
    public bool performHideTimer;

    public bool performMoveHitArea;
    public bool performHideHitArea;
    #endregion

    #region Util Var
    bool needDelay;

    bool optimizer_fixDir;
    bool optimizer_stopTimer;
    bool optimizer_hideHitArea;

    // Wait
    WaitForSeconds wait;
    WaitForSeconds delayTime;
    WaitForSeconds stopDuration; // Stop Duration

    WaitForSeconds onHideWait; // Hide In-Out Term
    WaitForSeconds hideDuration; // Duration of Hide-State
    WaitForSeconds revealDuration; // Duration of Reveal-State
    #endregion

    void Start()
    {
        timerSystem = GameObject.Find("System").transform.Find("Timer System").GetComponent<TimerSystem>();
        hitArea = GameObject.Find("TimerBar").transform.Find("Hit Area").GetComponent<HitArea>();

        setGUI();

        setWaits();

        minValue = 0; maxValue = 1000;
        needDelay = true;
        optimizer_hideHitArea = true; optimizer_stopTimer = true; optimizer_fixDir = true;
    }
    void setGUI()
    {
        hitAreaGUI_SpriteRenderers = new SpriteRenderer[3];
        hitAreaGUI_SpriteRenderers[0] = GameObject.Find("TimerBar").transform.Find("Hit Area").Find("Good").GetComponent<SpriteRenderer>();
        hitAreaGUI_SpriteRenderers[1] = GameObject.Find("TimerBar").transform.Find("Hit Area").Find("Great").GetComponent<SpriteRenderer>();
        hitAreaGUI_SpriteRenderers[2] = GameObject.Find("TimerBar").transform.Find("Hit Area").Find("Perfect").GetComponent<SpriteRenderer>();
        timerGUI_SpriteRenderer = GameObject.Find("Timer GUI").GetComponent<SpriteRenderer>();
    }
    void setWaits()
    {
        wait = new WaitForSeconds(0.001f);
        delayTime = new WaitForSeconds(0.3f);

        stopDuration = new WaitForSeconds(0.5f);

        onHideWait = new WaitForSeconds(0.08f);
        hideDuration = new WaitForSeconds(1.3f);
        revealDuration = new WaitForSeconds(1.5f);
    }

    void Update()
    {
        // Stop Timer
        if (performStopTimer)
        {
            if (optimizer_stopTimer)
            {
                if (needDelay)
                {
                    StartCoroutine(Delay_Pattern());
                }
                else
                {
                    optimizer_stopTimer = false;
                    StartCoroutine(StopTimer(maxValue));
                    StartCoroutine(StopTimer(minValue));
                }
            }
        }
        
        // Fixed Direction
        if (performFixedDir)
        {
            if (optimizer_fixDir)
            {
                optimizer_fixDir = false;
                FixDir();
            }
        }

        // Hide Timer
        if (performHideTimer)
        {
            StartCoroutine(HideTimer());
        }

        // Move HitArea
        if (performMoveHitArea)
        {
            StartCoroutine(MoveHitArea());
        }
        
        // Hide HitArea
        if (performHideHitArea)
        {
            if (optimizer_hideHitArea)
            {
                optimizer_hideHitArea = false;
                StartCoroutine(HideHitArea());
            }
        }
    }

    /* 패턴 딜레이 :: StopTimer, HideHitArea
     * 첫 시작부터 해당 패턴들이 수행되면, 유저 반응 전에 || 마음의 준비 전에 패턴이 수행되는 격.
     * 제한된 Hit 시간 내에 '당황'이 들어가면, '불합리' '부조리'하다 여길 가능성이 높음
     * 이는 '반복된 플레이 유도', '도전 의식 고취'라는 의도를 저해하고, 오히려 그 반대를 유도할 수 있음
     * 따라서 패턴 전에 딜레이를 줌으로써 '게임이 시작됐고, 패턴이 나타날 것이다'라는 마음의 준비를 할 여유를 제공
     */
    IEnumerator Delay_Pattern()
    {
        minValue = -1; maxValue = -1; 
        yield return delayTime;
        
        minValue = 0; maxValue = 1000;
        needDelay = false;
    }

    #region RandomPattern
    // 콤보 >>> 패턴 임의 계산 :: Pattern에 해당하는 bool 값만 switching.
    public void RandomPattern(int _combo)
    {
        Combo = _combo;

        switch (_combo)
        {
            case 4: break;
            case 5: break;
            case 6: break;
            case 7: break;
        }
    }
    public void StopPattern(int _combo)
    {
        StopAllCoroutines();

        ResetVar(_combo);

        // Reset TimerSystem
        timerSystem.setFixedDir(false);
        timerSystem.ResetTimerSpeed(Combo);

        ResetColors();
    }
    void ResetVar(int _combo)
    {
        Combo = _combo;

        performFixedDir = false;
        performStopTimer = false;
        performHideTimer = false;

        performMoveHitArea = false;
        performHideHitArea = false;

        optimizer_fixDir = true;
        optimizer_stopTimer = true;
        optimizer_hideHitArea = true;
    }
    void ResetColors()
    {
        // Timer
        timerGUI_Color = timerGUI_SpriteRenderer.color;
        timerGUI_Color.a = 1f;
        timerGUI_SpriteRenderer.color = timerGUI_Color;

        // HitArea
        for(int i = 0; i < hitAreaGUI_SpriteRenderers.Length; i++)
        {
            hitAreaGUI_Color = hitAreaGUI_SpriteRenderers[i].color;
            hitAreaGUI_Color.a = 1f;
            hitAreaGUI_SpriteRenderers[i].color = hitAreaGUI_Color;
        }
    }

    public void PatternDelayReset()
    {
        needDelay = true;
    }
    #endregion

    // ↓ 각 패턴 수행 함수들
    #region HitArea
    IEnumerator MoveHitArea()
    {
        // 매 프레임마다 호출됨
        // TimerSystem.MoveHitCenter :: HitCenter를 일정한 방향, 속도로 이동시키는 함수
        // HitArea.setHitArea :: Param으로 HitAreaGUI를 이동
        hitArea.setHitArea(timerSystem.MoveHitCenter());
        yield return wait;
    }
    IEnumerator HideHitArea()
    {
        // Hide-Reveal 일체 과정 수행. 과정 수행 중에는 호출 안 되도록 optimizer로 컨트롤

        optimizer_hideHitArea = false;
        hitAreaGUI_Color = hitAreaGUI_SpriteRenderers[0].color;

        // Hide in
        while (true)
        {
            hitAreaGUI_Color.a -= 0.1f;
            hitAreaGUI_SpriteRenderers[0].color = hitAreaGUI_Color;
            hitAreaGUI_SpriteRenderers[1].color = new Color(133 / 255f, 214 / 255f, 76 / 255f, hitAreaGUI_Color.a);
            hitAreaGUI_SpriteRenderers[2].color = new Color(64 / 255f, 176 / 255f, 77 / 255f, hitAreaGUI_Color.a);

            yield return onHideWait;

            if (hitAreaGUI_Color.a <= 0)
            {
                hitAreaGUI_Color.a = 0;
                break;
            }
        }
        yield return hideDuration;

        // Hide out
        while (true)
        {
            hitAreaGUI_Color.a += 0.1f;
            hitAreaGUI_SpriteRenderers[0].color = hitAreaGUI_Color;
            hitAreaGUI_SpriteRenderers[1].color = new Color(133 / 255f, 214 / 255f, 76 / 255f, hitAreaGUI_Color.a);
            hitAreaGUI_SpriteRenderers[2].color = new Color(64 / 255f, 176 / 255f, 77 / 255f, hitAreaGUI_Color.a);

            yield return onHideWait;

            if (hitAreaGUI_Color.a >= 1f)
            {
                hitAreaGUI_Color.a = 1f;
                break;
            }
        }
        yield return revealDuration;

        optimizer_hideHitArea = true;
    }
    #endregion

    #region Timer
    IEnumerator HideTimer()
    {
        // 양 끝에서는 TimerGUI 보이고, 그 외에는 안 보이게 Color.Alpha 값 조절을 수행
        // 양 끝(0:1000) 기점 150 거리(150:850)부터, 서서히 보이고 || 안 보이고를 수행
        // 양 끝에 가까워질수록, 1 - (남은 거리/150f)을 Color.Alpha값으로 할당 (끝에 도달시 1 - 0/150f = 1)

        timerGUI_Color = timerGUI_SpriteRenderer.color;
        if (timerSystem.getTimerValue() >= 850f)
        {
            timerGUI_Color.a = 1 - (1000 - timerSystem.getTimerValue()) / 150f;
        }
        if (timerSystem.getTimerValue() <= 150f)
        {
            timerGUI_Color.a = 1 - (timerSystem.getTimerValue() / 150f);
        }
        timerGUI_SpriteRenderer.color = timerGUI_Color;

        yield return null;
    }
    #endregion

    #region Data
    IEnumerator StopTimer(int _value)
    {
        // TimerSystem.getTimerValue 값이 멈추길 원하는 지점 _value와 같아질 때까지 대기 후, 같아지면 정지를 수행
        // 정지 Call이 1번만 수행되도록 optimizer로 호출 빈도 조정
        // 정지 직후 >> TimerSystem.getTimerValue 값은 변경되지 않았음. 재호출 경우 고려, 해당 값과 _value가 달라질 때까지 대기 후 함수 종료
        
        optimizer_stopTimer = false;
        yield return new WaitUntil(() => (int)timerSystem.getTimerValue() == _value);

        timerSystem.ChangeTimerSpeed(0f);
        yield return stopDuration;
        timerSystem.ResetTimerSpeed(Combo);

        yield return new WaitUntil(() => !((int)timerSystem.getTimerValue() == _value));
        optimizer_stopTimer = true;
    }
    void FixDir()
    {
        // TimerSystem.setFixedDir >> TimerSystem.FixedDir := True 가 됨.
        // 재호출돼도 큰 영향은 없으나, 굳이 재호출이 필요하지 않음.
        // 불필요한 Performance 소모를 막고자 optimizer 설정 (시간 Cost > 공간 Cost 로 비중을 둠)

        optimizer_fixDir = false;
        timerSystem.setFixedDir(performFixedDir);
    }
    #endregion
}
