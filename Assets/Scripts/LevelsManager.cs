using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelsManager : MonoBehaviour, IScene
{
    public static LevelsManager lm;
    private GameManager mGameManager;
    private WorldDatabase mWorldDatabase;
    public Button levelPrefab;
    public RectTransform levelsHolder;
    private Button[] mLevels;
    public Sprite textButtonSprite;
    public Sprite videoButtonSprite;
    public Image worldLogo;
    public Image worldCover;
    private BaseWorld mCurrentWorld;
    void Awake()
    {
        if (lm != null)
        {
            Destroy(lm);
        }
        else
        {
            lm = this;
        }
        mGameManager = GameManager.gm;
        if (mGameManager == null) return;
        mWorldDatabase = mGameManager.worldDatabase;
    }

    public void InitializeButtons(BaseWorld baseWorld)
    {
        mCurrentWorld = baseWorld;
        worldCover.sprite = baseWorld.worldCover;
        worldLogo.sprite = baseWorld.worldSprite;
        if (mLevels != null)
        {
            if (mLevels.Length != 0)
            {
                foreach (Button button in mLevels)
                {
                    Destroy(button.gameObject);
                    
                }
                mLevels = new Button[0];
            }
        }

        mLevels = new Button[baseWorld.levelsContent.Length];
        HorizontalLayoutGroup horizontalGroup = levelsHolder.GetComponent<HorizontalLayoutGroup>();
        float width = (levelPrefab.GetComponent<RectTransform>().sizeDelta.x*baseWorld.levelsContent.Length) + (horizontalGroup.spacing * (baseWorld.levelsContent.Length -1));
        float height = levelPrefab.GetComponent<RectTransform>().sizeDelta.y;
        RectTransform horizontalRect = horizontalGroup.GetComponent<RectTransform>();
        horizontalRect.sizeDelta = new Vector2(width, height);
        horizontalRect.anchoredPosition = new Vector2(5000f, horizontalRect.anchoredPosition.y);
        for (int i = baseWorld.levelsContent.Length - 1; i >= 0; i--)
        {
            mLevels[i] = Instantiate(levelPrefab);
            mLevels[i].interactable = baseWorld.levelsUnlocked[i];
            var i1 = i;
            mLevels[i].onClick.AddListener(() => {LevelClicked(baseWorld.levelsContent[i1]);});
            RectTransform levelRect = mLevels[i].GetComponent<RectTransform>();
            if (baseWorld.levelsContent[i].StartsWith("v")) //is a video
            {
                mLevels[i].image.sprite = videoButtonSprite;
            }
            else if (baseWorld.levelsContent[i].StartsWith("t")) //is a text
            {
                mLevels[i].image.sprite = textButtonSprite;
            }
            else if (baseWorld.levelsContent[i].StartsWith("q")) //is a quiz
            {
                mLevels[i].image.sprite = baseWorld.levelsDone[i] ? baseWorld.levelDone : baseWorld.levelNotDone;
            }
            else
            {
                Debug.LogError("FORMATO INVALIDO: " + baseWorld.name);
            }
            levelRect.SetParent(levelsHolder);
            levelRect.localScale = Vector3.one;
            levelRect.SetAsFirstSibling();
        }
        UpdateLevels();
        if (!GameScreen._tips2)
        {
            GameScreen._tips2 = true;
            GameScreen.gs.Tutorial2();
        }
    }

    public void UpdateLevels()
    {
        for (int i = mCurrentWorld.levelsContent.Length - 1; i >= 0; i--)
        {
            mLevels[i].interactable = mCurrentWorld.levelsUnlocked[i];
            if (mCurrentWorld.levelsContent[i].StartsWith("q")) //is a quiz
            {
                int code = int.Parse(mCurrentWorld.levelsContent[i].Substring(1, mCurrentWorld.levelsContent[i].Length - 1));
                BaseQuestion myQuestion = null;
                foreach (BaseQuestion baseQuestion in mGameManager.questionDatabase.questions)
                {
                    if (baseQuestion.id16 == code)
                    {
                        myQuestion = baseQuestion;
                        break;
                    }
                }
                if (myQuestion != null)
                {
                    mCurrentWorld.levelsDone[i] = myQuestion.completed;
                    mCurrentWorld.levelsUnlocked[i] = myQuestion.isUnlocked;
                    mLevels[i].image.sprite = myQuestion.completed ? mCurrentWorld.levelDone : mCurrentWorld.levelNotDone;
                }
                
            }
        }
        WorldsManager.wm.UpdateWorlds();
        StoreManager.sm.UpdateCatalog();
    }

    public void LevelClicked(string level)
    {
        int code = int.Parse(level.Substring(1, level.Length -1));
        if (level.StartsWith("v"))
        {
            BaseVideo myVideo = null;
            foreach (BaseVideo baseVideo in mGameManager.videoDatabase.videos)
            {
                if (baseVideo.id16 == code)
                {
                    myVideo = baseVideo;
                    break;
                }
            }
            Application.OpenURL(myVideo.url);
            //Handheld.PlayFullScreenMovie("Nova.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
        }
        else if (level.StartsWith("t"))
        {
            BaseText myText = null;
            foreach (BaseText baseText in mGameManager.textDatabase.texts)
            {
                if (baseText.id16 == code)
                {
                    myText = baseText;
                    break;
                }
            }
            TextDisplayManager.tdm.InitializeText(myText);
            GameScreen.gs.DoTransition(GameScreen.gs.textDisplayGroup);
        }
        else if (level.StartsWith("q"))
        {
            BaseQuestion myQuestion = null;
            foreach (BaseQuestion baseQuestion in mGameManager.questionDatabase.questions)
            {
                if (baseQuestion.id16 == code)
                {
                    myQuestion = baseQuestion;
                    break;
                }
            }
            QuizManager.qm.InitializeQuestion(myQuestion, mCurrentWorld);
            GameScreen.gs.DoTransition(GameScreen.gs.quizGroup);
        }
    }

    public void EnterScene()
    {
        GameScreen.gs.currentGroup = GameScreen.gs.levelGroup;
    }

    public void ExitScene()
    {

    }
}
