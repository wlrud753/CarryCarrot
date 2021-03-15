using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WagonSystem : MonoBehaviour
{
    Wagon wagon;

    List<int> PathIDX;
    int nowPathIDX;
    GameObject Cell;

    Slider WagonHPBar;

    WaitForSeconds LossDelay, waitMS, wait;

    // Start is called before the first frame update
    void Start()
    {
        wagon = this.GetComponent<Wagon>();

        Cell = GameObject.Find("Cell");

        WagonHPBar = GameObject.Find("Canvas").transform.Find("Wagon HP Bar").GetComponent<Slider>();

        LossDelay = new WaitForSeconds(0.3f);
        waitMS = new WaitForSeconds(0.001f);
        wait = waitMS;
    }

    #region Set
    public void setWagon(List<int> _path)
    {
        setPathInWagon(_path);

        SetInitPos();

        wagon.setWagonData();

        setWagonHPBar();

        System.GC.Collect();
    }
    void setPathInWagon(List<int> _path)
    {
        PathIDX = _path;
        nowPathIDX = 0;
    }
    void SetInitPos()
    {
        this.transform.position = Cell.transform.Find("Cell0").transform.position;
        isMoving = false;
    }
    #endregion

    // tagerPos에 도착하면 > FindNextPos해서 이동. PathIDX 내내
    IEnumerator Move;
    private void Update()
    {
        if (PathIDX != null)
        {
            if (!isMoving && nowPathIDX < PathIDX.Count - 1)
            {
                nowPos = Cell.transform.Find("Cell" + PathIDX[nowPathIDX]).transform.position;
                nextPos = Cell.transform.Find("Cell" + PathIDX[++nowPathIDX]).transform.position;

                Move = WagonMove();
                StartCoroutine(Move);
            }
            if(nowPathIDX == PathIDX.Count - 1 && this.transform.position == nextPos) // 모든 경로 수행하면, nextPos에는 '도착 지점'의 Pos가 들어있게 된다.
            {
                Debug.Log("Clear");

                PathIDX = null;

                // Next Stage
                // GameObject.Find("System").transform.Find("Path Maker").GetComponent<PathMaker>().Make(6);
            }
        }
    }

    #region Wagon Moving
    bool isMoving;
    Vector3 nowPos, nextPos;
    IEnumerator WagonMove()
    {
        isMoving = true;
        StartCoroutine(MovePos(nowPos, nextPos));
        yield return wait;
    }
    IEnumerator MovePos(Vector3 startPos, Vector3 targetPos)
    {
        Vector3 tmp;
        float count = 0;
        while (count <= 1)
        {
            count += wagon.WagonSpeed;
            tmp = Vector3.Lerp(startPos, targetPos, count);
            this.transform.position = tmp;
            yield return wait;
        }

        isMoving = false;
    }

    public void stopMoving()
    {
        //isMoving = true;
        StopAllCoroutines();

        // For Testing
        //Invoke("ReStart", 1f);
        //wagon.setWagonData();
    }
    public void ReStart()
    {
        isMoving = false;
    }
    #endregion

    #region Wagon HP
    void setWagonHPBar()
    {
        WagonHPBar.maxValue = wagon.getMaxHP();
        WagonHPBar.value = wagon.getHP();
    }
    void renewWagonHPBar()
    {
        WagonHPBar.value = wagon.getHP();
    }
    #endregion

    #region Wagon`s Collision Event
    float LossCount = 0;
    float LossTick = 0.1f;
    bool isLossing = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Obj")
        {
            if (!isLossing)
            {
                LossCount += Time.deltaTime;
                if (LossCount > LossTick)
                {
                    isLossing = true;
                    wagon.LossHP(5); renewWagonHPBar();
                    StartCoroutine(LossEffect());
                }
            }
        }
    }
    IEnumerator LossEffect()
    {
        wait = LossDelay;
        this.GetComponent<SpriteRenderer>().color = Color.red;

        yield return wait;

        wait = waitMS;
        this.GetComponent<SpriteRenderer>().color = Color.white;

        LossCount = 0;
        isLossing = false;
    }
    #endregion
}
