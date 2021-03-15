using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitArea : MonoBehaviour
{
    Bounds TimerArea;
    float Meter;

    Vector3 Pos;

    // Start is called before the first frame update
    void Start()
    {
        TimerArea = GameObject.Find("TimerBar").transform.Find("bar").GetComponent<BoxCollider2D>().bounds;
        Meter = (TimerArea.max.x - TimerArea.min.x) / 1000;

        Pos = new Vector3();
    }

    public void setHitArea(float _hitCenter)
    {
        float xPos = TimerArea.min.x + _hitCenter * Meter;

        Pos = this.transform.position;
        Pos.x = xPos;
        this.transform.position = Pos;
    }
}
