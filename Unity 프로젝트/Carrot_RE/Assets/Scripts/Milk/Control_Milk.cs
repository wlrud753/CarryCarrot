using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_Milk : MonoBehaviour
{
    // 게임 흐름을 주관하는 Class
    // Data 흐름을 제어하고, 각 Data Class들의 결과값을 바탕으로 GUI를 변경 || 새로운 Pattern을 전개를 호출

    // System
    TimerSystem timerSystem;
    GradeSystem gradeSystem;
    PatternSystem patternSystem;

    // GUI
    public TimerGUI milkTimer;
    public HitArea hitArea;

    // Perform Count
    public int PerformCount;
    int performCount;

    // Overall-In-Play var
    string Hit;
    int Combo = 0;
    float SpeedbyCombo = 1.08f;

    // Score
    int FinalScore;
    string Grade;

    WaitForSeconds RestartDelay;

    // Start is called before the first frame update
    void Start()
    {
        timerSystem = GameObject.Find("System").transform.Find("Timer System").GetComponent<TimerSystem>();
        gradeSystem = GameObject.Find("System").transform.Find("Grade System").GetComponent<GradeSystem>();
        patternSystem = GameObject.Find("System").transform.Find("Pattern System").GetComponent<PatternSystem>();

        milkTimer = GameObject.Find("Timer GUI").GetComponent<TimerGUI>();
        hitArea = GameObject.Find("TimerBar").transform.Find("Hit Area").GetComponent<HitArea>();

        RestartDelay = new WaitForSeconds(1f);

        Play_Milk();
    }

    #region Play
    public void Play_Milk()
    {
        if (performCount == PerformCount)
            Debug.Log("Game End");
        else
        {
            play();
        }
    }
    // perform actual Play
    void play()
    {
        performCount++;
        timerSystem.setTimerArea();

        // about setting GUI on Bar
        milkTimer.setTimer();
        hitArea.setHitArea(timerSystem.getHitCenter());

        // about Pattern Data
        patternSystem.PatternDelayReset();

        // Start
        milkTimer.setTimerMove(true);
        timerSystem.setTimerMove(true);
    }

    // for Testing Func :: Perform when 'Re' button pressed :: 'Re' button being for testing
    public void Re() 
    {
        performCount = 0;
        Combo = 0;
        FinalScore = 0;
        timerSystem.ResetTimerSpeed(0);
        patternSystem.StopPattern(0);
        Play_Milk();
    }
    IEnumerator ReStart()
    {
        yield return RestartDelay;
        Play_Milk();
    }
    #endregion

    #region Action Event
    // Action Event of Pressing Hit Button
    public void HitTimer()
    {
        // Get Result of Hit (BAD, GOOD, GREAT, PERFECT)
        Hit = timerSystem.TimerHit();

        // About Combo, Timer Speed related with Combo
        if (Hit == "BAD")
        {
            Combo = 0;
            timerSystem.ResetTimerSpeed(Combo);
            patternSystem.StopPattern(Combo);
        }
        else
        {
            Combo++;
            timerSystem.ChangeTimerSpeed(SpeedbyCombo);
        }

        // About Timer Pattern
        if(Combo >= 4)
            patternSystem.RandomPattern(Combo);
            

        // About Score
        FinalScore += gradeSystem.Scoring(Hit, Combo);
        Grade = gradeSystem.Grading(performCount, FinalScore);

        // modify GUI
        GameObject.Find("Canvas").transform.Find("Hit").GetComponent<TMPro.TextMeshProUGUI>().text = Hit;
        GameObject.Find("Canvas").transform.Find("Score").GetComponent<TMPro.TextMeshProUGUI>().text = FinalScore.ToString();
        GameObject.Find("Canvas").transform.Find("Grade").GetComponent<TMPro.TextMeshProUGUI>().text = Grade;

        StartCoroutine(ReStart());
    }
    #endregion
}
