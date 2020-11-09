using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    int Carrot;

    public TextMeshProUGUI CarrotTxt;

    // Start is called before the first frame update
    void Start()
    {
        //CarrotTxt = GameObject.Find("Info UI").transform.Find("Canvas").
        //    Find("Carrot Text").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        CarrotTxt.text = this.Carrot.ToString();
    }

    #region Carrot
    public void getCarrot(int _amount)
    {
        this.Carrot += _amount;
    }
    public bool useCarrot(int _amount) // 임시...
    {
        if(this.Carrot - _amount >= 0)
        {
            this.Carrot -= _amount;
            return true;
        }
        return false;
    }
    #endregion
}
