using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public struct ItemInfo
    {
        public int ID;
        public string Name;

        public int GrowTime;

        public string SeedPrice;
        public string SellPrice;
    }
}
