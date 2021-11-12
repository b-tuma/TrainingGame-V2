using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextsManager : MonoBehaviour, IScene
{
    public static TextsManager tm;
    private GameManager mGameManager;
    private TextDatabase mTextDatabase;

    public RectTransform textsHolder;
    public Button textPrefab;
    public Button readButton;
    private BaseText mSelectedText;
    public Text titleShow;
    private Button[] mTexts;

    void Awake()
    {
        if (tm != null)
        {
            Destroy(tm);
        }
        else
        {
            tm = this;
        }
        mGameManager = GameManager.gm;
        if (mGameManager == null) return;
        mTextDatabase = mGameManager.textDatabase;

        mTexts = new Button[mTextDatabase.texts.Count];
        GridLayoutGroup grid = textsHolder.GetComponent<GridLayoutGroup>();
        RectTransform rect = textsHolder.GetComponent<RectTransform>();
        float rows = Mathf.CeilToInt(mTextDatabase.texts.Count/grid.constraintCount);
        float width = rect.sizeDelta.x;
        float height = ((rows + 1f)*textPrefab.GetComponent<RectTransform>().sizeDelta.y) + (rows*grid.spacing.y);
        rect.sizeDelta = new Vector2(width, height);
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -5000f);

        for (int i = mTextDatabase.texts.Count - 1; i >= 0; i--)
        {
            mTexts[i] = Instantiate(textPrefab);
            var i1 = i;
            mTexts[i].onClick.AddListener(() => {TextPressed(mTextDatabase.texts[i1]);});
            mTexts[i].interactable = mTextDatabase.texts[i].isUnlocked;
            RectTransform textRect = mTexts[i].GetComponent<RectTransform>();
            textRect.SetParent(textsHolder);
            textRect.localScale = Vector3.one;
            textRect.SetAsFirstSibling();
        }
    }

    public void ReadText()
    {
        if (mSelectedText != null)
        {
            TextDisplayManager.tdm.InitializeText(mSelectedText);
            GameScreen.gs.DoTransition(GameScreen.gs.textDisplayGroup);
            titleShow.text = "";
            readButton.interactable = false;
        }
    }

    public void TextPressed(BaseText baseText)
    {
        if (baseText != mSelectedText)
        {
            mSelectedText = baseText;
            titleShow.text = baseText.title;
            readButton.interactable = true;
        }
        else
        {
            TextDisplayManager.tdm.InitializeText(mSelectedText);
            GameScreen.gs.DoTransition(GameScreen.gs.textDisplayGroup);
            titleShow.text = "";
            readButton.interactable = false;
        }
    }

    public void ExitScene()
    {
        titleShow.text = "";
        readButton.interactable = false;
        mSelectedText = null;
    }

    public void EnterScene()
    {
        GameScreen.gs.currentGroup = GameScreen.gs.textsGroup;
    }
}
