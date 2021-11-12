using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class StoreDatabase : MonoBehaviour
{
    public List<BaseProduct> products = new List<BaseProduct>();
}

[System.Serializable]
public class BaseProduct
{
    public int id16;
    public string name;
    public int price;
    public string description;
    public Sprite image;
}
