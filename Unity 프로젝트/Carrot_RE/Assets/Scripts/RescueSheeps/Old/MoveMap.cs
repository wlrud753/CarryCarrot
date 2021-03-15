using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMap : MonoBehaviour
{
    Camera MainCam;

    float depth;

    // for Move
    bool canMove;
    Vector3 MouseStart, MouseMove;

    // for Binding
    Bounds Bound;
    Vector3 min_bound, max_bound;
    float half_width, half_height;

    // Start is called before the first frame update
    void Start()
    {
        MainCam = Camera.main;

        depth = this.transform.position.z;

        // for Move
        MouseStart = new Vector3(); MouseMove = new Vector3();

        // for Binding
        setBound(GameObject.Find("Map").GetComponent<BoxCollider2D>().bounds);
        half_height = MainCam.orthographicSize;
        half_width = half_height * Screen.width / Screen.height;

        StartMoving();
    }
    void setBound(Bounds _bound)
    {
        Bound = _bound;
        min_bound = Bound.min;  max_bound = Bound.max;
    }


    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            StartCoroutine(DragMove());
        }   
    }

    #region Drag
    bool getDown;
    IEnumerator DragMove()
    {
        // GetMouseButtonDown Param :: 0:left click, 1: right click, 2: middle click
        if (Input.GetMouseButtonDown(1))
        {
            getDown = true;

            MouseStart.Set(Input.mousePosition.x, Input.mousePosition.y, depth);

            MouseStart = MainCam.ScreenToWorldPoint(MouseStart);
            MouseStart.z = this.transform.position.z;
        }
        else if (Input.GetMouseButton(1) && getDown) // 실질적인 Move를 수행
        {
            MouseMove.Set(Input.mousePosition.x, Input.mousePosition.y, depth);
            MouseMove = MainCam.ScreenToWorldPoint(MouseMove);
            MouseMove.z = this.transform.position.z;

            //this.transform.position = this.transform.position - (MouseMove - MouseStart);
            MainCam.transform.position = MainCam.transform.position - (MouseMove - MouseStart);
            Lock_in_Bound();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            getDown = false;
            /* 우클릭 후, 카메라 이동하고 있을때 좌클릭 누르고 있는 경우,
             * 카메라 이동 끝나는 시점에 눌려 있는 좌클릭 마우스 좌표가 MouseMove로 초기화,
             * -(MouseMove) 연산을 거쳐 카메라 위치가 0,0이 아닌, -MouseMove 좌표로 이동이 됨.
             * 따라서 "우클릭 후 > 좌클릭을 누르고 있는 경우" 자체를 차단하기 위해
             * "누르고 있는 경우" 자체를 삭제함. 해당 변수가 getDown
             * 무조건 마우스 버튼 down 과정을 거쳐야만 누르고 있는 상태의 코드가 실행됨.
             */
        }
        yield return null;
    }
    Vector3 tmp;
    void Lock_in_Bound()
    {
        tmp = MainCam.transform.position;

        // X-axis
        if (tmp.x > max_bound.x - half_width)
            tmp.x = max_bound.x - half_width;
        else if (tmp.x < min_bound.x + half_width)
            tmp.x = min_bound.x + half_width;

        // Y-axis
        if (tmp.y > max_bound.y - half_height)
            tmp.y = max_bound.y - half_height;
        else if (tmp.y < min_bound.y + half_height)
            tmp.y = min_bound.y + half_height;

        MainCam.transform.position = tmp;
    }
    #endregion

    #region Control State of Move
    public void StopMoving()
    {
        canMove = false;
    }
    public void StartMoving()
    {
        canMove = true;
    }
    #endregion
}
