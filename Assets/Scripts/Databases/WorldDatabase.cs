using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[ExecuteInEditMode]
public class WorldDatabase : MonoBehaviour
{
    public List<BaseWorld> worlds = new List<BaseWorld>();
}

[System.Serializable]
public class BaseWorld
{
    public int id16;
    public string name;
    public Sprite worldSprite;
    public Sprite levelNotDone;
    public Sprite levelDone;
    public Sprite worldCover;
    public Color worldColor;
    public string[] levelsContent;
    public bool[] levelsDone;
    public bool[] levelsUnlocked;
    public bool isUnlocked;
}