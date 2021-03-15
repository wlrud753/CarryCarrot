using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSystem : MonoBehaviour
{
    #region Line Var
    LineRenderer line;

    bool ActiveLine;
    
    int lineIDX;
    bool AddVertex;
    List<string> sheepNames;

    EdgeCollider2D edgeCollider;
    #endregion

    #region RayCast
    Camera MainCam;
    RaycastHit2D rayHit;
    Vector3 MousePos;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        line = this.GetComponent<LineRenderer>();
        sheepNames = new List<string>();

        edgeCollider = this.GetComponent<EdgeCollider2D>();

        MainCam = Camera.main;
    }
    

    public void ActivateLine()
    {
        ActiveLine = true;
    }

    public int LineEvent() // Meet Wolf: return -1 ; else: return 1
    {
        int result = 1;

        if (ActiveLine)
        {
            MousePos = MainCam.ScreenToWorldPoint(Input.mousePosition);
            rayHit = Physics2D.Raycast(MousePos, transform.forward);

            if (rayHit)
            {
                if (rayHit.collider.tag == "Sheep")
                {
                    AddSheep();
                }
                else if (rayHit.collider.tag == "Wolf") // 마우스가 Wolf와 부딪힌 경우
                {
                    result = -1;
                }
            }

            MousePos = MainCam.ScreenToWorldPoint(Input.mousePosition);

            line.SetPosition(lineIDX, MousePos);
        }

        return result;
    }

    public int GetResultOfStage()
    {
        if (WolfInLine) // No Getting Score, Just Reset
        {
            return -1;
        }
        else // Get Score
        {
            return sheepNames.Count;
        }
    }

    #region Trigger Event
    bool WolfInLine;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wolf") // 연결선이 Wolf와 부딪힌 경우
        {
            if(!WolfInLine)
                WolfInLine = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Wolf")
        {
            if(WolfInLine)
                WolfInLine = false;
        }
    }
    #endregion

    void AddSheep()
    {
        // Check Sheep collided first
        if (!sheepNames.Contains(rayHit.collider.name))
        {
            sheepNames.Add(rayHit.collider.name);
            AddVertex = true;
        }

        // If Sheep collided first, add Sheep in line vertices
        if (AddVertex)
        {
            PlusLineVertex();

            UpdateCollider(rayHit.collider.transform.position);

            AddVertex = false;
        }
    }

    #region Add Line Vertex
    void PlusLineVertex()
    {
        line.SetPosition(lineIDX, rayHit.collider.transform.position);
        line.positionCount = ++lineIDX + 1;

        line.endWidth += 10f;
    }
    void UpdateCollider(Vector2 newPoint)
    {
        if(edgeCollider.points[1] == Vector2.zero)
        {
            Vector2[] OnFirstUpdate = edgeCollider.points;
            OnFirstUpdate[0] = newPoint; OnFirstUpdate[1] = newPoint;
            edgeCollider.points = OnFirstUpdate;
            return;
        }
        if (edgeCollider.points[0] == edgeCollider.points[1])
        {
            Vector2[] OnSecondUpdate = edgeCollider.points;
            OnSecondUpdate[1] = newPoint;
            edgeCollider.points = OnSecondUpdate;
            return;
        }

        Vector2[] points = new Vector2[edgeCollider.points.Length + 1];

        int i;
        for (i = 0; i < edgeCollider.points.Length; i++)
        {
            points[i] = edgeCollider.points[i];
        }
        points[i] = newPoint;
        
        edgeCollider.points = points;

        System.GC.Collect();
    }
    #endregion

    #region Reset
    public void ResetLine()
    {
        ResetVar();
        ResetCollider();
    }
    void ResetVar()
    {
        ActiveLine = false;
        
        lineIDX = 0;
        line.positionCount = lineIDX + 1;
        line.startWidth = 10f; line.endWidth = 15f;
        sheepNames.Clear();

        WolfInLine = false;
    }
    void ResetCollider()
    {
        Vector2[] points = new Vector2[2];

        edgeCollider.points = points;

        System.GC.Collect();
    }
    #endregion
}
