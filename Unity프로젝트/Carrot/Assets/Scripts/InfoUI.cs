using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoUI : MonoBehaviour
{
    PlayerInfo playerInfo;

    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        playerInfo = GameObject.FindObjectOfType<PlayerInfo>();
        text = this.transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = NumberConverter.ToDisplayNum(playerInfo.getCarrot());
    }
}
