using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public WorldDatabase worldDatabase;
    public QuestionDatabase questionDatabase;
    public VideoDatabase videoDatabase;
    public TextDatabase textDatabase;
    public StoreDatabase storeDatabase;
    private bool mIsLoading;
    public string lastScene;
    public float transitionSpeed = 1f;
    public Ease transitionInEase = Ease.InSine;
    public Ease transitionOutEase = Ease.OutSine;

    void Awake()
    {
        if (gm != null)
        {
            Destroy(gm);
        }
        else
        {
            gm = this;
        }

        worldDatabase = GetComponent<WorldDatabase>();
        questionDatabase = GetComponent<QuestionDatabase>();
        videoDatabase = GetComponent<VideoDatabase>();
        textDatabase = GetComponent<TextDatabase>();
        storeDatabase = GetComponent<StoreDatabase>();

        GameObject[] allObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        if (allObjects != null)
        {
            foreach (GameObject myObject in allObjects)
            {
                if (myObject.layer == Layers.BaseLayer)
                {
                    DontDestroyOnLoad(myObject);
                }
            }
        }
    }

    public void LoadLevel(string levelToLoad)
    {
        if (mIsLoading) return;
        if (string.IsNullOrEmpty(levelToLoad)) return;
        mIsLoading = true;
        lastScene = Application.loadedLevelName;
        StartCoroutine(LoadLevelRoutine(levelToLoad));
    }

    IEnumerator LoadLevelRoutine(string level)
    {
        Debug.Log("GameManager: Loading Level[" + level + "]...");
        FindObjectOfType<CanvasGroup>().DOFade(0f, transitionSpeed / 2f).SetEase(transitionInEase);
        yield return new WaitForSeconds(transitionSpeed / 2f);
        Application.LoadLevel(level);
        //AsyncOperation async = Application.LoadLevelAsync(level);
        //yield return async;
        mIsLoading = false;
        Debug.Log("GameManager: Level[" + level + "] loaded.");
    }

    void Start()
    {
        LoadLevel("MainMenu");
    }
}
