using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_Sheep : MonoBehaviour
{
    StageSystem stageSystem;
    LineSystem lineSystem;

    #region Game Process Var
    bool GameProgress;
    bool NeedReset;
    WaitForSeconds WaitForReset; WaitForSeconds WaitForReplay;
    public float ResettingTime;
    public float ReplayingTime;
    #endregion

    bool TimingStage;
    public float StageTimer;
    float stageTimer;

    bool ActiveLine;

    TMPro.TextMeshProUGUI wolfText;
    TMPro.TextMeshProUGUI timeText;

    // Start is called before the first frame update
    void Start()
    {
        stageSystem = GameObject.Find("System").transform.Find("StageSystem").GetComponent<StageSystem>();
        lineSystem = GameObject.Find("System").transform.Find("LineSystem").GetComponent<LineSystem>();

        WaitForReset = new WaitForSeconds(ResettingTime);
        WaitForReplay = new WaitForSeconds(ReplayingTime);

        wolfText = GameObject.Find("Canvas").transform.Find("Got Wolf").GetComponent<TMPro.TextMeshProUGUI>();
        timeText = GameObject.Find("Canvas").transform.Find("Time Text").GetComponent<TMPro.TextMeshProUGUI>();

        Set();
        NeedReset = true;
    }

    void Set()
    {
        stageSystem.ResetStage();
        lineSystem.ResetLine();

        stageTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameProgress)
        {
            Stage(); // Rename...
            Line(); // Rename...
        }
        
        if (NeedReset)
        {
            NeedReset = false;
            StartCoroutine(ResetStage());
        }
    }

    #region Game Progress
    void Stage()
    {
        stageTimer += Time.deltaTime;
        if(stageTimer >= StageTimer)
        {
            NeedReset = true;
        }
        SetStageTimeText();
    }
    float difference;
    void SetStageTimeText()
    {
        difference = StageTimer - stageTimer;
        if(difference < 0f)
        {
            difference = 0;
        }
        timeText.text = difference.ToString("F2");
    }

    void Line()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lineSystem.ActivateLine();
        }
        if (Input.GetMouseButton(0))
        {
            if(lineSystem.LineEvent() == -1) // Mouse(Touch) move meets wolf
            {
                NeedReset = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(lineSystem.GetResultOfStage() == -1) // Rope meets wolf
            {

            }
            else // Scoring
            {

            }
            NeedReset = true;
        }
    }
    #endregion

    IEnumerator ResetStage() // IEnumerater... get waiting time
    {
        GameProgress = false;
        
        wolfText.text = "Reset Start...";
        yield return WaitForReset;

        stageSystem.ResetStage();
        stageTimer = 0f;
        lineSystem.ResetLine();
        
        wolfText.text = "Wait For Replay...";
        yield return WaitForReplay;
        
        GameProgress = true;
        wolfText.text = "Start!";
    }
}
