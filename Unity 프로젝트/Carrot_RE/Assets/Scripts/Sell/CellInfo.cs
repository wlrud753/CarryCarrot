using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellInfo : MonoBehaviour
{
    enum CellType { PATH, UNBREAKABLE, ITEM, NONE };
    enum PathDir { LEFT, RIGHT, UP, DOWN };

    CellType cellType;

    PathDir previousPathDir, nextPathDir;

    bool IsRoad = false;

    GameObject Obj;
    int ObjHP;

    // Start is called before the first frame update
    void Awake()
    {
        cellType = new CellType();
        cellType = CellType.NONE;

        Obj = this.transform.Find("Obj").gameObject;
    }

    #region Set Data
    // Setting IsRoad var
    public void setRoad()
    {
        cellType = CellType.PATH;
        IsRoad = true;
    }
    public void setPathDir(int _now_pre, int _next_now)
    {
        // _now_pre == nowIDX - preIDX
        switch (_now_pre)
        {
            case 1:
                previousPathDir = PathDir.LEFT;
                break;
            case -1:
                previousPathDir = PathDir.RIGHT;
                break;
            default: // UP or DOWN
                if (_now_pre > 0)
                    previousPathDir = PathDir.UP;
                else
                    previousPathDir = PathDir.DOWN;
                break;
        }

        // _next_now == nextIDX - nowIDX
        switch (_next_now)
        {
            case 1:
                nextPathDir = PathDir.RIGHT;
                break;
            case -1:
                nextPathDir = PathDir.LEFT;
                break;
            default:
                if (_next_now > 0)
                    nextPathDir = PathDir.DOWN;
                else
                    nextPathDir = PathDir.UP;
                break;
        }
    }
    #endregion

    #region Set Obj Type
    // Setting for each Object Type
    public void setStart()
    {
        this.gameObject.tag = "Start";

        this.GetComponent<SpriteRenderer>().color = Color.red;
    }
    public void setDestination()
    {
        this.gameObject.tag = "Destination";

        this.GetComponent<SpriteRenderer>().color = Color.red;
    }
    public void setObj()
    {
        if (cellType == CellType.UNBREAKABLE || cellType == CellType.ITEM)
            return;

        ObjHP = Random.Range(1, 10);
        Color c = new Color(25, 25, 25);
        c = c * (10 - ObjHP) / 255; c.a = 1f;
        if (Obj == null) Debug.Log("null");
        //Obj.GetComponent<SpriteRenderer>().color = c;
        this.GetComponent<SpriteRenderer>().color = c;

        this.gameObject.tag = "Obj";
    }
    public void setSpecialObj()
    {
        if(Random.Range(0,2) == 1) // 파괴 불가능 Obj
        {
            // Init Setting
            cellType = CellType.UNBREAKABLE;
            ObjHP = -1;
            this.GetComponent<BoxCollider2D>().enabled = false;

            // IMG
            //Obj.GetComponent<SpriteRenderer>().color = Color.yellow;
            this.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            // Init Setting
            cellType = CellType.ITEM;
            ObjHP = 1;

            // IMG
            //Obj.GetComponent<SpriteRenderer>().color = Color.cyan;
            this.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
    }
    #endregion

    #region Action Event
    private void OnMouseDown()
    {
        if(ObjHP > 0)
        {
            ObjHP--;
            
            // Click Event
            Color c = new Color(25, 25, 25);
            c = c * (10 - ObjHP) / 255; c.a = 1f;
            //Obj.GetComponent<SpriteRenderer>().color = c;
            this.GetComponent<SpriteRenderer>().color = c;

            // Change IMG
            if (ObjHP == 0)
            {
                this.gameObject.tag = "Untagged"; // Tag 설정해서 수정

                if (IsRoad)
                {
                    this.GetComponent<SpriteRenderer>().color = Color.red;
                    //revealPathDir();
                }
                else
                {
                    this.GetComponent<SpriteRenderer>().color = Color.green;
                }
            }
        }
    }
    // 이전 길, 다음 길 위치를 감안해서 현재 길의 IMG를 변경
    void revealPathDir()
    {
        // PathDir IMG GO
        GameObject Arrow = this.transform.Find("화살표").gameObject;
        Arrow.SetActive(true);

        // PathDir IMG in Flat case
        // 지금은 일단 임의로, 화살표_일 이란 GO 따로 넣어서 해놨음. Resources.Load 해오게 변경해야 함
        Sprite FlatArrowIMG = this.transform.Find("화살표_일").GetComponent<SpriteRenderer>().sprite;

        Vector3 Scale = Arrow.transform.localScale;
        switch (previousPathDir)
        {
            case PathDir.LEFT:
                switch (nextPathDir)
                {
                    case PathDir.UP:
                        Scale.x = 1; Scale.y = 1;
                        Arrow.transform.localScale = Scale;
                        break;
                    case PathDir.DOWN:
                        Scale.x = 1; Scale.y = -1;
                        Arrow.transform.localScale = Scale;
                        break;
                    case PathDir.RIGHT:
                        // Change IMG
                        Arrow.GetComponent<SpriteRenderer>().sprite = FlatArrowIMG;
                        Scale.x = 1; Scale.y = 1;
                        Arrow.transform.localScale = Scale;
                        break;
                }
                break;
            case PathDir.RIGHT:
                switch (nextPathDir)
                {
                    case PathDir.UP:
                        Scale.x = -1; Scale.y = 1;
                        Arrow.transform.localScale = Scale;
                        break;
                    case PathDir.DOWN:
                        Scale.x = -1; Scale.y = -1;
                        Arrow.transform.localScale = Scale;
                        break;
                    case PathDir.RIGHT:
                        // Change IMG
                        Arrow.GetComponent<SpriteRenderer>().sprite = FlatArrowIMG;
                        Scale.x = -1; Scale.y = 1;
                        Arrow.transform.localScale = Scale;
                        break;
                }
                break;
            case PathDir.UP:
                Arrow.transform.rotation = Quaternion.Euler(0, 0, 90);
                switch (nextPathDir)
                {
                    case PathDir.LEFT:
                        Scale.x = -1; Scale.y = 1;
                        Arrow.transform.localScale = Scale;
                        break;
                    case PathDir.RIGHT:
                        Scale.x = -1; Scale.y = -1;
                        Arrow.transform.localScale = Scale;
                        break;
                    case PathDir.DOWN:
                        // Change IMG
                        Arrow.GetComponent<SpriteRenderer>().sprite = FlatArrowIMG;
                        Scale.x = -1; Scale.y = 1;
                        Arrow.transform.localScale = Scale;
                        break;
                }
                break;
            case PathDir.DOWN:
                Arrow.transform.rotation = Quaternion.Euler(0, 0, 90);
                switch (nextPathDir)
                {
                    case PathDir.LEFT:
                        Scale.x = 1; Scale.y = 1;
                        Arrow.transform.localScale = Scale;
                        break;
                    case PathDir.RIGHT:
                        Scale.x = 1; Scale.y = -1;
                        Arrow.transform.localScale = Scale;
                        break;
                    case PathDir.UP:
                        // Change IMG
                        Arrow.GetComponent<SpriteRenderer>().sprite = FlatArrowIMG;
                        Scale.x = 1; Scale.y = 1;
                        Arrow.transform.localScale = Scale;
                        break;
                }
                break;
        }
    }
    #endregion
}
