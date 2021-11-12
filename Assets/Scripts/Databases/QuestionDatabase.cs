using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class QuestionDatabase : MonoBehaviour
{
    public List<BaseQuestion> questions = new List<BaseQuestion>();
    public float timeBeforeCountdown = 4f;
}

[System.Serializable]
public class BaseQuestion
{
    public int id16;
    public string question;
    public bool isUnlocked;
    public bool failed;
    public string[] alternative = new string[5];
    public int correctAlternative;
    public int maxScore;
    public int minScore;
    public float time;
    public bool completed;
}