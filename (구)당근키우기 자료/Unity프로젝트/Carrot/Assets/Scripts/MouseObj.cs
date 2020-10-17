using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseObj : MonoBehaviour
{
    Vector3 Pos;

    public Item.ItemInfo item;

    [HideInInspector]
    public bool jobDone;

    // for Collsion
    GameObject collisionGO;
    bool checkCol;

    void Start()
    {
        checkCol = false;
        jobDone = true;
    }

    void Update()
    {
        // Position
        Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Pos.z = 0f;
        this.transform.position = Pos;

        // Mouse Event
        if (checkCol)
        {
            jobDone = false;
            if (Input.GetMouseButtonUp(0))
            {
                collisionGO.GetComponent<FarmField>().startGrow(this.item);
            }
            jobDone = true;
        }
    }

    #region Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("FarmField"))
        {
            collisionGO = collision.gameObject;
            checkCol = true;
        }
        else
        {
            checkCol = false;
            collisionGO = null;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("FarmField"))
        {
            checkCol = false;
            collisionGO = null;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!checkCol && collision.gameObject.tag.Equals("FarmField"))
        {
            collisionGO = collision.gameObject;
            checkCol = true;
        }
    }
    #endregion
}
