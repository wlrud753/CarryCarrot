using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{

    public float RemainTime;
    float remainTimer;

    bool isTargetted = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //remainTimer += Time.deltaTime;
        //if(remainTimer >= RemainTime)
        //{
        //    Destroy(this.gameObject);
        //}
    }

    #region Rescue
    public void Rescuing()
    {
        this.isTargetted = true;
    }
    public void StopRescue()
    {
        this.isTargetted = false;
    }
    #endregion

    #region Target
    public void Targetted()
    {
        isTargetted = true;
    }
    public bool IsTargetted()
    {
        return isTargetted;
    }
    #endregion

    #region Kill
    public void Kill()
    {
        // Kill
        Destroy(this.gameObject);
    }
    #endregion

    public float getRemainingTime()
    {
        return RemainTime - remainTimer;
    }
}
