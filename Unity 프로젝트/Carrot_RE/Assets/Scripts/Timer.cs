using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    bool start;

    float timer;

    public TextMeshProUGUI timeTxt;

    // Start is called before the first frame update
    void Start()
    {
        start = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
            timer += Time.deltaTime;

        timeTxt.text = ((int)timer).ToString();
    }

    public void TimeStop()
    {
        start = false;
    }
    public void TimeStart()
    {
        start = true;
    }
}
