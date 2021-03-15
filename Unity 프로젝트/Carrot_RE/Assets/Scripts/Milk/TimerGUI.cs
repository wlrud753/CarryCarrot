using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerGUI : MonoBehaviour
{
    TimerSystem timerSystem;

    Bounds TimerArea;
    float Meter;

    bool TimerMove;

    // Start is called before the first frame update
    void Start()
    {
        timerSystem = GameObject.Find("System").transform.Find("Timer System").GetComponent<TimerSystem>();

        TimerArea = GameObject.Find("TimerBar").transform.Find("bar").GetComponent<BoxCollider2D>().bounds;
        Meter = (TimerArea.max.x - TimerArea.min.x) / 1000;
    }

    Vector2 Pos = new Vector2();
    public void setTimer()
    {
        Pos = GameObject.Find("TimerBar").transform.position;
        Pos.x = TimerArea.min.x + timerSystem.TimerBar.value * Meter;
        this.transform.position = Pos;
    }

    public void setTimerMove(bool _timerMove)
    {
        TimerMove = _timerMove;
    }

    void Update()
    {
        if (TimerMove)
        {
            Pos.x = TimerArea.min.x + timerSystem.TimerBar.value * Meter;
            this.transform.position = Pos;
        }
    }
}
