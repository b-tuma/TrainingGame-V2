using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WorldsManager : MonoBehaviour, IScene
{
    public static WorldsManager wm;
    private GameManager mGameManager;
    private WorldDatabase mWorldDatabase;
    public Button worldPrefab;
    public RectTransform emptyWorld;
    public RectTransform worldsHolder;
    private Button[] mWorlds;

    void Awake()
    {
        if (wm != null)
        {
            Destroy(wm);
        }
        else
        {
            wm = this;
        }
        mGameManager = GameManager.gm;
        if (mGameManager == null) return;
        mWorldDatabase = mGameManager.worldDatabase;
        mWorlds = new Button[mWorldDatabase.worlds.Count];

        if (mWorldDatabase.worlds.Count % 2 == 0)
        {
            RectTransform ew = Instantiate(emptyWorld);
            ew.SetParent(worldsHolder);
            ew.localScale = Vector3.one;
            ew.SetAsFirstSibling();
        }

        for (int i = mWorldDatabase.worlds.Count - 1; i >= 0; i--)
        {
            BaseWorld world = mWorldDatabase.worlds[i];
            mWorlds[i] = Instantiate(worldPrefab);
            RectTransform worldRect = mWorlds[i].GetComponent<RectTransform>();
            mWorlds[i].onClick.AddListener(() => { WorldClick(world); });
            mWorlds[i].image.sprite = world.worldSprite;
            mWorlds[i].interactable = world.isUnlocked;
            int levelsCompleteCount = 0;
            foreach (bool levelDone in world.levelsDone)
            {
                if (levelDone) levelsCompleteCount++;
            }
            mWorlds[i].GetComponentInChildren<Text>().text = levelsCompleteCount + "/" + world.levelsContent.Length;
            worldRect.SetParent(worldsHolder);
            worldRect.localScale = Vector3.one;
            worldRect.SetAsFirstSibling();
        }
    }

    public void UpdateWorlds()
    {
        for (int i = mWorldDatabase.worlds.Count - 1; i >= 0; i--)
        {
            mWorlds[i].interactable = mWorldDatabase.worlds[i].isUnlocked;
            int levelsCompleteCount = 0;
            foreach (bool levelDone in mWorldDatabase.worlds[i].levelsDone)
            {
                if (levelDone) levelsCompleteCount++;
            }
            if (mWorlds[i] != null)
            {
                mWorlds[i].GetComponentInChildren<Text>().text = levelsCompleteCount + "/" + mWorldDatabase.worlds[i].levelsContent.Length;
            }
            
        }
    }

    public void WorldClick(BaseWorld myWorld)
    {
        LevelsManager.lm.InitializeButtons(myWorld);
        GameScreen.gs.DoTransition(GameScreen.gs.levelGroup);
    }

    public void ExitScene()
    {
    }

    public void EnterScene()
    {
    }
}
