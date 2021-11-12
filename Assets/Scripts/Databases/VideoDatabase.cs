using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class VideoDatabase : MonoBehaviour
{
    public List<BaseVideo> videos = new List<BaseVideo>(); 
}

[System.Serializable]
public class BaseVideo
{
    public int id16;
    public string title;
    public bool isUnlocked;
    public string url;
}