using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TextDatabase : MonoBehaviour
{
    public List<BaseText> texts = new List<BaseText>();
}

[System.Serializable]
public class BaseText
{
    public int id16;
    public string title;
    public bool isUnlocked;
    public RectTransform textContent;
}