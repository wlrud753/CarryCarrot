using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Image IMG;
    public TextMeshProUGUI text;

    // for Mouse Event
    GameObject mouseObj;
    bool MouseDown;
    float timer;

    float maxY;
    BoxCollider2D boxCol;
    Bounds bound;
    float thisMaxY;
    float differ;
    Vector2 vec;

    // for Item
    public Item.ItemInfo item;

    void Start()
    {
        scrollRect = GameObject.Find("Farm UI").transform.Find("Inventory").transform.Find("Scroll Rect").GetComponent<ScrollRect>();
        mouseObj = GameObject.Find("MouseObject").transform.Find("IMG").gameObject;

        maxY = scrollRect.gameObject.GetComponent<BoxCollider2D>().bounds.max.y;
        boxCol = this.GetComponent<BoxCollider2D>();
        bound = boxCol.bounds;
    }
    private void Update()
    {
        thisMaxY = this.transform.position.y + bound.max.y;
        if (thisMaxY >= maxY)
        {
            differ = thisMaxY - maxY;
            vec = boxCol.offset; vec.y = -(differ/2); boxCol.offset = vec;
            vec = boxCol.size; vec.y = 150 - differ; boxCol.size = vec;
        }
        else
        {
            vec = boxCol.offset; vec.y = 0; boxCol.offset = vec;
            vec = boxCol.size; vec.y = 150; boxCol.size = vec;
        }
    }

    public void setSlot(Item.ItemInfo _item, int _num)
    {
        this.item = _item;
        this.name = "vegi" + string.Format("{0:D3}", item.ID);
        IMG.sprite = Resources.Load<Sprite>("Icon/vegi" + string.Format("{0:D3}", item.ID));
        text.text = _num.ToString();
    }

    public void setText(int _num)
    {
        text.text = _num.ToString();
    }

    #region Mouse Event
    private void OnMouseDown()
    {
        MouseDown = true;
        timer = 0;
    }
    private void OnMouseUp()
    {
        if(mouseObj.activeSelf)
            StartCoroutine(mouseUpEvent());
    }
    IEnumerator mouseUpEvent()
    {
        yield return new WaitUntil(() => mouseObj.GetComponent<MouseObj>().jobDone == true);
        MouseDown = false;
        mouseObj.SetActive(false);
        scrollRect.vertical = true;
    }
    private void OnMouseExit()
    {
        MouseDown = false;
    }
    private void OnMouseOver()
    {
        if (MouseDown)
        {
            timer += Time.deltaTime;
            if(timer > 0.8f)
            {
                scrollRect.vertical = false;

                mouseObj.SetActive(true);
                mouseObj.GetComponent<SpriteRenderer>().sprite = IMG.sprite;
                mouseObj.GetComponent<MouseObj>().item = this.item;
            }
        }
    }
    #endregion
}
